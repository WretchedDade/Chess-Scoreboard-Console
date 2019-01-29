using Newtonsoft.Json;

namespace ChessScoreboard.Net.Core.Data.Models
{
    public class Game
    {
        [JsonProperty("#")]
        public int? Id { get; set; }

        [JsonProperty("Winner")]
        public string WinnerName { get; set; }

        [JsonProperty("Loser")]
        public string LoserName { get; set; }

        [JsonProperty("Stalemate?")]
        public bool WasAStalemate { get; set; }
    }
}