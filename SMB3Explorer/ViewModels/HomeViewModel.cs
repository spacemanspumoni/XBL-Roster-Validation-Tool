using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Serilog;
using SMB3Explorer.Models.Internal;
using SMB3Explorer.Services.ApplicationContext;
using SMB3Explorer.Services.DataService;
using SMB3Explorer.Services.NavigationService;
using SMB3Explorer.Services.SystemIoWrapper;
using SMB3Explorer.Utils;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SMB3Explorer.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly IApplicationContext _applicationContext;
    private readonly IDataService _dataService;
    private readonly INavigationService _navigationService;
    private readonly ISystemIoWrapper _systemIoWrapper;

    private string _selectedXBLRosterFile = string.Empty;

    private ObservableCollection<TeamSelection> _teams = new();
    private TeamSelection? _selectedTeam;

    private ObservableCollection<Player> _pitchers = new();
    private ObservableCollection<Player> _positionPlayers = new();
    private ObservableCollection<ValidatedPlayer> _validatedPitchers = new();
    private ObservableCollection<ValidatedPlayer> _validatedPositionPlayers = new();

    private bool _interactionEnabled;

    public HomeViewModel(INavigationService navigationService, IDataService dataService,
        IApplicationContext applicationContext, ISystemIoWrapper systemIoWrapper)
    {
        _navigationService = navigationService;
        _dataService = dataService;
        _applicationContext = applicationContext;
        _systemIoWrapper = systemIoWrapper;
        
        Log.Information("Initializing HomeViewModel");

        _applicationContext.PropertyChanged += ApplicationContextOnPropertyChanged;

        GetSaveData();
    }

    public TeamSelection? SelectedTeam
    {
        get => _selectedTeam;
        set
        {
            SetField(ref _selectedTeam, value);
            _applicationContext.SelectedTeam = value;

            var teamName = value?.TeamName ?? "None";
            Log.Information("Set selected team to {TeamName}", teamName);
            OnPropertyChanged(nameof(TeamSelected));
        }
    }

    public ObservableCollection<Player> Pitchers
    {
        get => _pitchers;
        set {
            SetField(ref _pitchers, value);
            Log.Information($"{nameof(Pitchers)}: {value}");
            OnPropertyChanged(nameof(Pitchers));
        }
    }

    public ObservableCollection<Player> PositionPlayers
    {
        get => _positionPlayers;
        set
        {
            SetField(ref _positionPlayers, value);
            Log.Information($"{nameof(PositionPlayers)}: {value}");
            OnPropertyChanged(nameof(PositionPlayers));
        }
    }

    public ObservableCollection<ValidatedPlayer> ValidatedPitchers
    {
        get => _validatedPitchers;
        set
        {
            SetField(ref _validatedPitchers, value);
            Log.Information($"{nameof(ValidatedPitchers)}: {value}");
            OnPropertyChanged(nameof(ValidatedPitchers));
        }
    }
    public ObservableCollection<ValidatedPlayer> ValidatedPositionPlayers
    {
        get => _validatedPositionPlayers;
        set
        {
            SetField(ref _validatedPositionPlayers, value);
            Log.Information($"{nameof(ValidatedPositionPlayers)}: {value}");
            OnPropertyChanged(nameof(ValidatedPositionPlayers));
        } 
    }

    public ObservableCollection<TeamSelection> Teams
    {
        get => _teams;
        private set => SetField(ref _teams, value);
    }

    public bool InteractionEnabled
    {
        get => _interactionEnabled;
        set => SetField(ref _interactionEnabled, value);
    }

    public string SelectedXBLRosterFile
    {
        get => _selectedXBLRosterFile;
        set {
            _selectedXBLRosterFile = value;
            OnPropertyChanged(nameof(SelectedXBLRosterFile));
        }
    }

    private bool TeamSelected => SelectedTeam is not null;

    private void ApplicationContextOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(ApplicationContext.MostRecentFranchiseSeason):
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                });

                break;
            }
            case nameof(ApplicationContext.IsTeamSelected):
            {
                var players = _dataService.GetPlayers().Result;
                var pitchers = players.Where(p => new [] { "SP", "SP/RP", "RP", "CP" }.Contains(p.DisplayPitchPosition));
                var posPlayers = players.Where(p => p.Arm is not null);
                _pitchers = new ObservableCollection<Player>(pitchers);
                _positionPlayers = new ObservableCollection<Player>(posPlayers);
                Pitchers = _pitchers;
                PositionPlayers = _positionPlayers;
                ValidatePlayers(SelectedXBLRosterFile);

                break;
            }
        }
    }

    [RelayCommand]
    private void ManuallySelectXBLRosterCSV()
    {
        Mouse.OverrideCursor = Cursors.Wait;

        var directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        var filePathResult =
            SaveFile.GetUserProvidedFile(directoryPath, _systemIoWrapper, "XLSX files (*.xlsx)|*.xlsx");
        if (filePathResult.TryPickT1(out _, out var filePath) || string.IsNullOrEmpty(filePath))
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            return;
        }

        SelectedXBLRosterFile = filePath.ToString();
        ValidatePlayers(filePath);
    }

    private void ValidatePlayers(string filePath)
    {
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(filePath, false))
        {
            var pitcherRows = new[] { 6, 8, 10, 12, 16, 18, 20, 22 };
            var pitchers = pitcherRows.Select(row => BuildSheetPitcher(spreadsheetDocument, row));

            var playerRows = new[] { 26, 28, 30, 32, 34, 36, 38, 40, 42, 44, 46, 48, 50, 52 };
            var players = playerRows.Select(row => BuildSheetPlayer(spreadsheetDocument, row));

            _validatedPitchers = new ObservableCollection<ValidatedPlayer>(
                pitchers.Select(pitcher =>
                    new ValidatedPlayer(pitcher, _pitchers.FirstOrDefault(p => p.DisplayName == pitcher.Name))));
            ValidatedPitchers = _validatedPitchers;

            _validatedPositionPlayers = new ObservableCollection<ValidatedPlayer>(
                players.Select(player =>
                    new ValidatedPlayer(player, _positionPlayers.FirstOrDefault(p => p.DisplayName == player.Name))));
            ValidatedPositionPlayers = _validatedPositionPlayers;

            Mouse.OverrideCursor = Cursors.Arrow;
            return;
        }
    }

    SheetPlayer BuildSheetPitcher(SpreadsheetDocument document, int row)
    {
        return new SheetPlayer
        {
            Name = GetCellValue(document, $"B{row}"),
            PitchPosition = GetCellValue(document, $"H{row}"),
            Batting = GetCellValue(document, $"L{row}"),
            Throwing = GetCellValue(document, $"N{row}"),
            Power = (int)decimal.Parse(GetCellValue(document, $"P{row}")),
            Contact = (int)decimal.Parse(GetCellValue(document, $"S{row}")),
            Speed = (int)decimal.Parse(GetCellValue(document, $"V{row}")),
            Fielding = (int)decimal.Parse(GetCellValue(document, $"Y{row}")),
            Velocity = (int)decimal.Parse(GetCellValue(document, $"AE{row}")),
            Junk = (int)decimal.Parse(GetCellValue(document, $"AH{row}")),
            Accuracy = (int)decimal.Parse(GetCellValue(document, $"AK{row}")),
            Chemistry = GetCellValue(document, $"AN{row}"),
            Trait1 = GetCellValue(document, $"AS{row}"),
            Trait2 = GetCellValue(document, $"AZ{row}"),
            FourSeam = GetCellValue(document, $"BL{row}") == "TRUE",
            TwoSeam = GetCellValue(document, $"BM{row}") == "TRUE",
            Cutter = GetCellValue(document, $"BN{row}") == "TRUE",
            ChangeUp = GetCellValue(document, $"BO{row}") == "TRUE",
            Curve = GetCellValue(document, $"BP{row}") == "TRUE",
            Slider = GetCellValue(document, $"BQ{row}") == "TRUE",
            Fork = GetCellValue(document, $"BR{row}") == "TRUE",
            Screwball = GetCellValue(document, $"BS{row}") == "TRUE",
            ArmAngle = GetCellValue(document, $"BT{row}")
        };
    }

    SheetPlayer BuildSheetPlayer(SpreadsheetDocument document, int row)
    {
        return new SheetPlayer
        {
            Name = GetCellValue(document, $"B{row}"),
            PrimaryPosition = GetCellValue(document, $"H{row}"),
            SecondaryPosition = GetCellValue(document, $"J{row}"),
            Batting = GetCellValue(document, $"L{row}"),
            Throwing = GetCellValue(document, $"N{row}"),
            Power = (int)decimal.Parse(GetCellValue(document, $"P{row}")),
            Contact = (int)decimal.Parse(GetCellValue(document, $"S{row}")),
            Speed = (int)decimal.Parse(GetCellValue(document, $"V{row}")),
            Fielding = (int)decimal.Parse(GetCellValue(document, $"Y{row}")),
            Arm = (int)decimal.Parse(GetCellValue(document, $"AB{row}")),
            Chemistry = GetCellValue(document, $"AN{row}"),
            Trait1 = GetCellValue(document, $"AS{row}"),
            Trait2 = GetCellValue(document, $"AZ{row}")
        };
    }

    string GetCellValue(SpreadsheetDocument document, string addressName)
    {
        const string sheetName = "Roster";
        string? value = null;

        // Retrieve a reference to the workbook part.
        WorkbookPart? wbPart = document.WorkbookPart;
        // Find the sheet with the supplied name, and then use that 
        // Sheet object to retrieve a reference to the first worksheet.
        Sheet? theSheet = wbPart?.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName).FirstOrDefault();

        // Throw an exception if there is no sheet.
        if (theSheet is null || theSheet.Id is null)
        {
            throw new ArgumentException("sheetName");
        }
        // Retrieve a reference to the worksheet part.
        WorksheetPart wsPart = (WorksheetPart)wbPart!.GetPartById(theSheet.Id!);
        // Use its Worksheet property to get a reference to the cell 
        // whose address matches the address you supplied.
        
        foreach (var cell in wsPart.Worksheet?.Descendants<Cell>() ?? Enumerable.Empty<Cell>())
        {
            var cellReference = cell.CellReference;
            Console.WriteLine(cell.CellReference);
        }

        Cell? theCell = wsPart.Worksheet?.Descendants<Cell>()?.Where(c => c.CellReference == addressName).FirstOrDefault();
        // If the cell does not exist, return an empty string.
        if (theCell is null || theCell.InnerText.Length < 0)
        {
            return string.Empty;
        }
        value = theCell.InnerText;
        // If the cell represents an integer number, you are done. 
        // For dates, this code returns the serialized value that 
        // represents the date. The code handles strings and 
        // Booleans individually. For shared strings, the code 
        // looks up the corresponding value in the shared string 
        // table. For Booleans, the code converts the value into 
        // the words TRUE or FALSE.
        if (theCell.DataType is not null)
        {
            if (theCell.DataType.Value == CellValues.SharedString)
            {
                // For shared strings, look up the value in the
                // shared strings table.
                var stringTable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                // If the shared string table is missing, something 
                // is wrong. Return the index that is in
                // the cell. Otherwise, look up the correct text in 
                // the table.
                if (stringTable is not null)
                {
                    value = stringTable.SharedStringTable.ElementAt(int.Parse(value)).InnerText;
                }
            }
            else if (theCell.DataType.Value == CellValues.Boolean)
            {
                switch (value)
                {
                    case "0":
                        value = "FALSE";
                        break;
                    default:
                        value = "TRUE";
                        break;
                }
            }
        }

        return value;
    }

    private bool ShouldLoadPLayers()
    {
        return TeamSelected;
    }

    private void GetSaveData()
    {
        InteractionEnabled = true;
        _dataService.GetTeams()
            .ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    DefaultExceptionHandler.HandleException(_systemIoWrapper, "Failed to get teams.",
                        task.Exception);
                    return;
                }

                if (task.Result.Any())
                {
                    Log.Debug("{Count} Teams found", task.Result.Count);
                    Teams = new ObservableCollection<TeamSelection>(task.Result);
                    InteractionEnabled = true;
                }
            });
    }

    protected override void Dispose(bool disposing)
    {
        _applicationContext.PropertyChanged -= ApplicationContextOnPropertyChanged;
        base.Dispose(disposing);
    }
}