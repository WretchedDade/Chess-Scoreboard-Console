using Newtonsoft.Json;

namespace ChessScoreboard.Net.Core.Data.Models
{
    public class Player
    {
        [JsonProperty("Rank")]
        public int Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Wins")]
        public int Wins { get; set; }

        [JsonProperty("Losses")]
        public int Losses { get; set; }

        [JsonProperty("Stalemates")]
        public int Stalemates { get; set; }

        [JsonProperty("Rating")]
        public double Rating { get; set; }

        [JsonIgnore]
        public int Rank => Id;

        [JsonIgnore]
        public string FirstName => Name.Substring(0, Name.IndexOf(' '));
    }
}
