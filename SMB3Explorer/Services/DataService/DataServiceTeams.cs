using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SMB3Explorer.Models.Internal;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Services.DataService;

public partial class DataService
{
    public async Task<List<TeamSelection>> GetTeams()
    {
        var command = Connection!.CreateCommand();
        var commandText = SqlRunner.GetSqlCommand(SqlFile.Teams);
        command.CommandText = commandText;
        var reader = await command.ExecuteReaderAsync();

        List<TeamSelection> teams = new();
        while (reader.Read())
        {
            var teamBytes = reader["teamId"] as byte[] ?? Array.Empty<byte>();
            var teamId = teamBytes.ToGuid();

            var teamName = reader["teamName"].ToString()!;

            var team = new TeamSelection
            {
                TeamId = teamId,
                TeamName = teamName,
            };
            teams.Add(team);
        }

        return teams;
    }
}