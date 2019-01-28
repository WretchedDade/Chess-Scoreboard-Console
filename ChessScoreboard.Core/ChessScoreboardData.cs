using System.Collections.Generic;
using System.Linq;
using ChessScoreboard.Core.Models;

namespace ChessScoreboard.Core
{
    public class ChessScoreboardData
    {
        private readonly GoogleSheetsAPI GoogleSheetsAPI;

        public List<Game> Games { get; set; }
        public List<Player> Players { get; set; }

        public ChessScoreboardData()
        {
            GoogleSheetsAPI = GoogleSheetsAPI.ForChessScoreboard();

            Players = GoogleSheetsAPI.Get<Player>(Constants.SpreadsheetId, Constants.Players.FullRange).ToList();
            Games = GoogleSheetsAPI.Get<Game>(Constants.SpreadsheetId, Constants.Games.FullRange).ToList();
        }
    }
}
