using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Relay
{
    public class RelayResponse
    {
        /// <summary>
        ///     DATA - task ID for the relayed transaction.
        /// </summary>
        [JsonProperty(PropertyName = "taskId")]
        public string TaskId { get; set; }
    }
}