using System.Collections.Generic;
using System.Linq;
using ChessScoreboard.Net.Core.Data.Models;
using GoogleSheets;

namespace ChessScoreboard.Net.Core.Data
{
    public class PlayerData
    {
        private readonly GoogleSheetsService GoogleSheetsService;

        public PlayerData(GoogleSheetsService googleSheetsService)
        {
            GoogleSheetsService = googleSheetsService;
            Refresh();
        }

        public ICollection<Player> Players { get; set; }

        public Player GetById(int id) => Players.SingleOrDefault(player => player.Id == id);

        public void AddPlayer(string name, double rating)
        {
            var player = new Player
            {
                Name = name,
                Rating = rating
            };

            Players.Add(player);
        }

        public async void UpdateSpreadsheet()
        {
            int lastRowToUpdate = Players.Any() ? Players.Count + 1 : Constants.Games.LastRow;

            string gamesRange = $"{Constants.Games.SheetName}!{Constants.Games.WinnerColumn}$2:{Constants.Games.StalemateColumn}${lastRowToUpdate}";

            IList<IList<object>> rows = RowConverter.ToRows(Players).ToList();

            // Rebuild the list without Id (Game #) since that column is calculated using row() - 1
            rows = rows.Select(row => new List<object> { row[1], row[2], row[3] } as IList<object>).ToList();

            await GoogleSheetsService.UpdateAsync(rows, Constants.SpreadsheetId, gamesRange);
        }

        private async void UpdateRatingsInSpreadsheet()
        {
            IList<IList<object>> rows = RowConverter.ToRows(Players.Select(player => player.Rating)).ToList();
        }

        public void Refresh() => Players = GoogleSheetsService.Get<Player>(Constants.SpreadsheetId, Constants.Players.FullRange).ToList();
    }
}
