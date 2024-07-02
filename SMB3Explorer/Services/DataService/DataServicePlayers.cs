using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using SMB3Explorer.Enums;
using SMB3Explorer.Models.Internal;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Services.DataService;

public partial class DataService
{
    public async Task<List<Player>> GetPlayers()
    {
        var command = Connection!.CreateCommand();
        var commandText = SqlRunner.GetSqlCommand(SqlFile.Players);
        command.CommandText = commandText;
        command.Parameters.Add(new SqliteParameter("@teamID", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedTeam!.TeamId.ToBlob()
        });

        var reader = await command.ExecuteReaderAsync();

        List<Player> players = new();
        while (reader.Read())
        {
            var playerBytes = reader["Id"] as byte[] ?? Array.Empty<byte>();
            var playerId = playerBytes.ToGuid();
            var firstName = reader["FirstName"].ToString();
            var lastName = reader["LastName"].ToString();
            var power = (long)reader["Power"];
            var contact = (long)reader["Contact"];
            var speed = (long)reader["Speed"];
            var fielding = (long)reader["Fielding"];
            var arm = (long?)(reader["Arm"].GetType() != typeof(DBNull) ? reader["Arm"] : null);
            var velocity = (long?)(reader["Velocity"].GetType() != typeof(DBNull) ? reader["Velocity"] : null);
            var junk = (long?)(reader["Junk"].GetType() != typeof(DBNull) ? reader["Junk"] : null);
            var accuracy = (long?)(reader["Accuracy"].GetType() != typeof(DBNull) ? reader["Accuracy"] : null);
            var primaryPosition = (long?)(reader["PrimaryPosition"].GetType() != typeof(DBNull) ? reader["PrimaryPosition"] : null);
            var secondaryPosition = (long?)(reader["SecondaryPosition"].GetType() != typeof(DBNull) ? reader["SecondaryPosition"] : null);
            var pitchPosition = (long?)(reader["PitchPosition"].GetType() != typeof(DBNull) ? reader["PitchPosition"] : null);
            var batting = (long)reader["Batting"];
            var throwing = (long)reader["Throwing"];
            var chemistry = (long)reader["Chemistry"];
            var fourSeam = (long?)(reader["FourSeam"].GetType() != typeof(DBNull) ? reader["FourSeam"] : null);
            var twoSeam = (long?)(reader["TwoSeam"].GetType() != typeof(DBNull) ? reader["TwoSeam"] : null);
            var screwball = (long?)(reader["Screwball"].GetType() != typeof(DBNull) ? reader["Screwball"] : null);
            var changeup = (long?)(reader["ChangeUp"].GetType() != typeof(DBNull) ? reader["ChangeUp"] : null);
            var fork = (long?)(reader["Fork"].GetType() != typeof(DBNull) ? reader["Fork"] : null);
            var curve = (long?)(reader["Curve"].GetType() != typeof(DBNull) ? reader["Curve"] : null);
            var slider = (long?)(reader["Slider"].GetType() != typeof(DBNull) ? reader["Slider"] : null);
            var cutter = (long?)(reader["Cutter"].GetType() != typeof(DBNull) ? reader["Cutter"] : null);
            var armAngle = (long?)(reader["ArmAngle"].GetType() != typeof(DBNull) ? reader["ArmAngle"] : null);
            var traitsSerialized = reader["Traits"].GetType() != typeof(DBNull) ? reader["Traits"].ToString() : null;

            var player = new Player
            {
                Id = playerId,
                FirstName = firstName,
                LastName = lastName,
                Power = power,
                Contact = contact,
                Speed = speed,
                Fielding = fielding,
                Arm = arm,
                Velocity = velocity,
                Junk = junk,
                Accuracy = accuracy,
                PrimaryPosition = primaryPosition,
                SecondaryPosition = secondaryPosition,
                PitchPosition = pitchPosition,
                Batting = batting,
                Throwing = throwing,
                Chemistry = chemistry,
                FourSeam = HasPitch(fourSeam),
                TwoSeam = HasPitch(twoSeam),
                Screwball = HasPitch(screwball),
                ChangeUp = HasPitch(changeup),
                Fork = HasPitch(fork),
                Curve = HasPitch(curve),
                Slider = HasPitch(slider),
                Cutter = HasPitch(cutter),
                ArmAngle = armAngle
            };

            if (!string.IsNullOrEmpty(traitsSerialized))
            {
                var traits =
                    JsonConvert.DeserializeObject<PlayerTrait.DatabaseTraitSubtypePair[]>(traitsSerialized) ??
                    Array.Empty<PlayerTrait.DatabaseTraitSubtypePair>();
                player.Traits = traits
                    .Select(x => PlayerTrait.Smb4TraitMap[x])
                    .Distinct()
                    .ToArray();
            }

            players.Add(player);
        }

        return players;
    }

    internal bool HasPitch(long? pitchOptionValue)
    {
        return pitchOptionValue.HasValue && pitchOptionValue.Value == 1;
    }
}