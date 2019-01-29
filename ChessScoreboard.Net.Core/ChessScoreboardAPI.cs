using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using ChessScoreboard.Net.Core.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;

namespace ChessScoreboard.Net.Core
{
    public class ChessScoreboardAPI
    {
        //public IEnumerable<Game> GetGamesPlayed(List<Player> players)
        //{
        //    ValueRange response = SheetsService.Spreadsheets.Values.Get(Constants.SpreadsheetId, Constants.Games.FullRange).Execute();

        //    foreach (IList<object> row in response.Values)
        //    {
        //        if (row.Count == 0 || string.IsNullOrWhiteSpace(row[0].ToString()) || string.IsNullOrWhiteSpace(row[1].ToString()) || string.IsNullOrWhiteSpace(row[2].ToString()))
        //            break;

        //        int id = Convert.ToInt32(row[0]);

        //        string winnerName = Convert.ToString(row[1]);
        //        string loserName = Convert.ToString(row[2]);

        //        Player winner = players.FirstOrDefault(player => player.Name.Equals(winnerName, StringComparison.OrdinalIgnoreCase));
        //        Player loser = players.FirstOrDefault(player => player.Name.Equals(loserName, StringComparison.OrdinalIgnoreCase));

        //        bool wasStalemate = Convert.ToBoolean(row[3]);

        //        yield return new Game() { Id = id, Winner = winner, Loser = loser, WasAStalemate = wasStalemate };
        //    }
        //}

        //public IEnumerable<Player> GetPlayers()
        //{
        //    GetRequest request = SheetsService.Spreadsheets.Values.Get(Constants.SpreadsheetId, Constants.Players.FullRange);

        //    ValueRange response = request.Execute();

        //    foreach (IList<object> row in response.Values)
        //    {
        //        if (string.IsNullOrWhiteSpace(row[0].ToString()) || string.IsNullOrWhiteSpace(row[1].ToString()))
        //            break;

        //        int id = Convert.ToInt32(row[0]);

        //        double rating = Constants.BaseRating;
        //        if (row.Count >= 6 && !string.IsNullOrWhiteSpace(row[5].ToString()))
        //            rating = Convert.ToDouble(row[5]);

        //        yield return new Player { Id = id, Rating = rating, Name = row[1].ToString() };
        //    }
        //}

        //public void UpdateRatingsInSpreadsheet(List<Player> players)
        //{
        //    players = players.OrderBy(player => player.Rank).ToList();

        //    var requestBody = new ValueRange
        //    {
        //        Values = players.Select(player => new List<object>() { player.Rating } as IList<object>).ToList()
        //    };

        //    string ratingsRange = $"{Constants.Players.SheetName}!{Constants.Players.RatingColumn}$2:{Constants.Players.RatingColumn}${players.Count + 1}";

        //    UpdateSpreadsheet(requestBody, ratingsRange);
        //}

        //public void UpdatePlayersInSpreadsheet(List<Player> players)
        //{
        //    players = players.OrderBy(player => player.Rank).ToList();

        //    var requestBody = new ValueRange
        //    {
        //        Values = players.Select(player => new List<object>() { player.Name } as IList<object>).ToList()
        //    };

        //    string namesRange = $"{Constants.Players.SheetName}!{Constants.Players.NameColumn}$2:{Constants.Players.NameColumn}${players.Count + 1}";

        //    UpdateSpreadsheet(requestBody, namesRange);

        //    UpdateRatingsInSpreadsheet(players);
        //}

        //public void UpdateGamesInSpreadsheet(List<Game> games)
        //{
        //    string gamesRange = $"{Constants.Games.SheetName}!{Constants.Games.WinnerColumn}$2:{Constants.Games.StalemateColumn}$";
        //    gamesRange = games.Any() ? $"{gamesRange}{games.Count + 1}" : $"{gamesRange}{Constants.Games.LastRow}";


        //    var requestBody = new ValueRange { Values = new List<IList<object>> { new List<object> { "", "", "" } } };
        //    requestBody = new ValueRange { Values = games.Select(game => new List<object> { game.Winner.Name, game.Loser.Name, game.WasAStalemate } as IList<object>).ToList() };

        //    UpdateSpreadsheet(requestBody, gamesRange);
        //}

        //private void UpdateSpreadsheet(ValueRange valueRange, string range)
        //{
        //    UpdateRequest request = SheetsService.Spreadsheets.Values.Update(valueRange, Constants.SpreadsheetId, range);
        //    request.ValueInputOption = UpdateRequest.ValueInputOptionEnum.USERENTERED;

        //    request.Execute();

        //    FormatSpreadsheet();
        //}
    }
}