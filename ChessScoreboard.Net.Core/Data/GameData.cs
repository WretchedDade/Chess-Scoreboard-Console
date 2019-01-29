using System.Collections.Generic;
using System.Linq;
using ChessScoreboard.Net.Core.Data.Models;
using GoogleSheets;

namespace ChessScoreboard.Net.Core.Data
{
    public class GameData
    {
        private readonly GoogleSheetsService GoogleSheetsService;

        public GameData(GoogleSheetsService googleSheetsService)
        {
            GoogleSheetsService = googleSheetsService;
            Refresh();
        }

        public ICollection<Game> Games { get; set; }

        public Game GetById(int id) => Games.SingleOrDefault(game => game.Id == id);

        public void AddGame(string winnerName, string loserName, bool wasStalemate)
        {
            var game = new Game
            {
                WinnerName = winnerName,
                LoserName = loserName,
                WasAStalemate = wasStalemate
            };

            Games.Add(game);
        }

        public async void UpdateSpreadsheet()
        {
            int lastRowToUpdate = Games.Any() ? Games.Count + 1 : Constants.Games.LastRow;

            string gamesRange = $"{Constants.Games.SheetName}!{Constants.Games.WinnerColumn}$2:{Constants.Games.StalemateColumn}${lastRowToUpdate}";

            IList<IList<object>> rows = RowConverter.ToRows(Games).ToList();

            // Rebuild the list without Id (Game #) since that column is calculated using row() - 1
            rows = rows.Select(row => new List<object> { row[1], row[2], row[3] } as IList<object>).ToList();

            await GoogleSheetsService.UpdateAsync(rows, Constants.SpreadsheetId, gamesRange);
        }

        public void Refresh() => Games = GoogleSheetsService.Get<Game>(Constants.SpreadsheetId, Constants.Games.FullRange).ToList();
    }
}
