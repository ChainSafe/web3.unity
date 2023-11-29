using System;
using Newtonsoft.Json;

namespace evm.net.Models.ABI
{
    [JsonConverter(typeof(ABIDefConverter))]
    public class ABIDef
    {
        [JsonProperty("type")]
        public ABIDefType DefType { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("inputs")]
        public ABIParameter[] Inputs { get; set; }

        public ABIFunction AsFunction()
        {
            if (DefType == ABIDefType.Event || DefType == ABIDefType.Error)
                throw new ArgumentException("Invalid type for function");

            return (ABIFunction) this;
        }

        public ABIEvent AsEvent()
        {
            if (DefType != ABIDefType.Event)
                throw new ArgumentException("Invalid type for event");

            return (ABIEvent) this;
        }

        public ABIError AsError()
        {
            if (DefType != ABIDefType.Error)
                throw new ArgumentException("Invalid type for error");

            return (ABIError) this;
        }
    }
}