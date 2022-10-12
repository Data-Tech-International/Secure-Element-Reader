using Newtonsoft.Json;
using System;

namespace SecureElementReader.App.Models
{
    public class CommandsStatusResult
    {
        [JsonProperty("dateAndTime")]
        public DateTime DateAndTime { get; set; }

        [JsonProperty("commandId")]
        public Guid CommandId { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
