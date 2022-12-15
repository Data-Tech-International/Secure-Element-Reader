using Newtonsoft.Json;
using SecureElementReader.Enums;
using System;

namespace SecureElementReader.Models
{
    public class Command
    {
        [JsonProperty("commandId")]
        public Guid CommandId { get; set; } = Guid.NewGuid();

        [JsonProperty("type")]
        public CommandsType Type { get; set; }

        [JsonProperty("payload")]
        public string Payload { get; set; }

        [JsonProperty("uid")]
        public string UID { get; set; }
    }
}
