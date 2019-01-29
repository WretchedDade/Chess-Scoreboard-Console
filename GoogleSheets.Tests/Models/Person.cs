using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GoogleSheets.Tests.Models
{
    public class Person
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("First Name")]
        public string FirstName { get; set; }

        [JsonProperty("Last Name")]
        public string LastName { get; set; }
    }
}
