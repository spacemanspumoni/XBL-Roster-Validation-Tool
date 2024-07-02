using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
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
            var position = (long)reader["Position"];
            var batting = (long)reader["Batting"];
            var throwing = (long)reader["Throwing"];
            players.Add(new Player
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
                Position = position,
                Batting = batting,
                Throwing = throwing,
            });
        }

        return players;
    }
}