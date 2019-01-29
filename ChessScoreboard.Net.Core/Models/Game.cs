using Newtonsoft.Json;

namespace ChessScoreboard.Net.Core.Models
{
    public class Game
    {
        [JsonProperty("Winner")]
        public string WinnerName { get; set; }

        [JsonProperty("Loser")]
        public string LoserName { get; set; }

        [JsonProperty("Stalemate?")]
        public bool WasAStalemate { get; set; }

        [JsonIgnore]
        public int? Id { get; set; }
    }
}