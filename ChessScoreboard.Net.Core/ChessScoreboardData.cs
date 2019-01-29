using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using ChessScoreboard.Net.Core.Data.Models;
using GoogleSheets;

namespace ChessScoreboard.Net.Core
{
    public class ChessScoreboardData
    {
        private readonly GoogleSheetsService GoogleSheetsAPI;

        public List<Game> Games { get; set; }
        public List<Player> Players { get; set; }

        public ChessScoreboardData()
        {
            GoogleSheetsAPI = GoogleSheetsService.New("credentials.json", Constants.AppName, Constants.Scopes, Constants.CredDataStoreLocation);

            Refresh();
        }

        public void Refresh()
        {
            Players = GoogleSheetsAPI.Get<Player>(Constants.SpreadsheetId, Constants.Players.FullRange).ToList();
            Games = GoogleSheetsAPI.Get<Game>(Constants.SpreadsheetId, Constants.Games.FullRange).ToList();
        }

        public async void FormatSpreadsheet()
        {
            using (var httpClient = new HttpClient())
                await httpClient.GetAsync(Constants.AppScriptExecURL);
        }
    }
}
