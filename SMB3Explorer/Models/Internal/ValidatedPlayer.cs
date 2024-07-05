namespace SMB3Explorer.Models.Internal
{
    public class ValidatedPlayer
    {
        public ValidatedPlayer(SheetPlayer sheetPlayer, Player gamePlayer)
        {
            Name = sheetPlayer.Name;
            Power = ValidateNumberProperty(sheetPlayer.Power, gamePlayer.Power);
            Contact = ValidateNumberProperty(sheetPlayer.Power, gamePlayer.Power);
            Speed = ValidateNumberProperty(sheetPlayer.Power, gamePlayer.Power);
            Fielding = ValidateNumberProperty(sheetPlayer.Power, gamePlayer.Power);
            Arm = ValidateNumberProperty(sheetPlayer.Power, gamePlayer.Power);
            Velocity = ValidateNumberProperty(sheetPlayer.Power, gamePlayer.Power);
            Junk = ValidateNumberProperty(sheetPlayer.Power, gamePlayer.Power);
            Accuracy = ValidateNumberProperty(sheetPlayer.Power, gamePlayer.Power);
            PrimaryPosition = ValidateStringProperty(sheetPlayer.PrimaryPosition, gamePlayer.DisplayPrimaryPosition);
            SecondaryPosition = ValidateStringProperty(sheetPlayer.SecondaryPosition, gamePlayer.DisplaySecondaryPosition);
            PitchPosition = ValidateStringProperty(sheetPlayer.PitchPosition, gamePlayer.DisplayPitchPosition);
            Batting = ValidateStringProperty(sheetPlayer.Batting, gamePlayer.DisplayBatting);
            Throwing = ValidateStringProperty(sheetPlayer.Throwing, gamePlayer.DisplayThrowing);
            Chemistry = ValidateStringProperty(sheetPlayer.Chemistry, gamePlayer.DisplayChemistry);
            FourSeam = ValidateBoolProperty(sheetPlayer.FourSeam, gamePlayer.FourSeam);
            TwoSeam = ValidateBoolProperty(sheetPlayer.TwoSeam, gamePlayer.TwoSeam);
            Screwball = ValidateBoolProperty(sheetPlayer.Screwball, gamePlayer.Screwball);
            ChangeUp = ValidateBoolProperty(sheetPlayer.ChangeUp, gamePlayer.ChangeUp);
            Fork = ValidateBoolProperty(sheetPlayer.Fork, gamePlayer.Fork);
            Curve = ValidateBoolProperty(sheetPlayer.Curve, gamePlayer.Curve);
            Slider = ValidateBoolProperty(sheetPlayer.Slider, gamePlayer.FourSeam);
            Cutter = ValidateBoolProperty(sheetPlayer.Cutter, gamePlayer.Cutter);
            ArmAngle = ValidateStringProperty(sheetPlayer.ArmAngle, gamePlayer.DisplayArmAngle);
            Trait1 = ValidateStringProperty(sheetPlayer.Trait1, gamePlayer.Trait1);
            Trait2 = ValidateStringProperty(sheetPlayer.Trait2, gamePlayer.Trait2);
        }

        private ValidatedPlayerProperty<string?> ValidateNumberProperty(int sheetValue, long gameValue)
        {
            return new ValidatedPlayerProperty<string?>
            {
                Value = $"{sheetValue}",
                IsValid = sheetValue == gameValue,
                Delta = (sheetValue - gameValue).ToString()
            };
        }

        private ValidatedPlayerProperty<string?> ValidateStringProperty(string? sheetValue, string? gameValue)
        {
            return new ValidatedPlayerProperty<string?>
            {
                Value = $"{sheetValue}",
                IsValid = sheetValue == gameValue,
                Delta = gameValue
            };
        }

        private ValidatedPlayerProperty<bool> ValidateBoolProperty(bool sheetValue, bool gameValue)
        {
            return new ValidatedPlayerProperty<bool>
            {
                Value = sheetValue,
                IsValid = sheetValue == gameValue,
                Delta = string.Empty
            };
        }

        public string? Name { get; set; }
        public ValidatedPlayerProperty<string?>? Power { get; set; }
        public ValidatedPlayerProperty<string?>? Contact { get; set; }
        public ValidatedPlayerProperty<string?>? Speed { get; set; }
        public ValidatedPlayerProperty<string?>? Fielding { get; set; }
        public ValidatedPlayerProperty<string?>? Arm {  get; set; }
        public ValidatedPlayerProperty<string?>? Velocity { get; set; }
        public ValidatedPlayerProperty<string?>? Junk { get; set; }
        public ValidatedPlayerProperty<string?>? Accuracy { get; set; }
        public ValidatedPlayerProperty<string?>? PrimaryPosition { get; set; }
        public ValidatedPlayerProperty<string?>? SecondaryPosition { get; set; }
        public ValidatedPlayerProperty<string?>? PitchPosition { get; set; }
        public ValidatedPlayerProperty<string?>? Batting { get; set; }
        public ValidatedPlayerProperty<string?>? Throwing { get; set; }
        public ValidatedPlayerProperty<string?>? Chemistry { get; set; }
        public ValidatedPlayerProperty<bool>? FourSeam { get; set; }
        public ValidatedPlayerProperty<bool>? TwoSeam { get; set; }
        public ValidatedPlayerProperty<bool>? Screwball { get; set; }
        public ValidatedPlayerProperty<bool>? ChangeUp { get; set; }
        public ValidatedPlayerProperty<bool>? Fork { get; set; }
        public ValidatedPlayerProperty<bool>? Curve { get; set; }
        public ValidatedPlayerProperty<bool>? Slider { get; set; }
        public ValidatedPlayerProperty<bool>? Cutter { get; set; }
        public ValidatedPlayerProperty<string?>? ArmAngle { get; set; }
        public ValidatedPlayerProperty<string?>? Trait1 { get; set; }
        public ValidatedPlayerProperty<string?>? Trait2 { get; set; }
    }

    public class ValidatedPlayerProperty<T>
    {
        public T? Value { get; set; }
        public bool IsValid { get; set; }
        public string? Delta { get; set; }
        public string DisplayValue
        {
            get
            {
                if (IsValid) return Value?.ToString() ?? string.Empty;
                if (int.TryParse(Delta, out var delta))
                {
                    if (delta > 0) return $"{Value} (+{delta})";
                    if (delta < 0) return $"{Value} (-{delta})";
                }
                return $"{Value} ({Delta})";
            }
        }
    }
}
