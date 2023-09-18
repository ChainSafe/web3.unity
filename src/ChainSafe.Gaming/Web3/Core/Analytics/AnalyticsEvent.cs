using System;
using System.Collections.Generic;
using System.Text;
using Nethereum.Model;

namespace ChainSafe.Gaming.Web3.Analytics
{
    public class AnalyticsEvent
    {
        public string ProjectId { get; set; }

        public string ChainId { get; set; }

        public string Rpc { get; set; }

        public string Client { get; set; }

        public string Version { get; set; }

        public string Player { get; set; }

        public string To { get; set; }

        public string Value { get; set; }

        public string GasLimit { get; set; }

        public string GasPrice { get; set; }

        public Dictionary<string, object> CustomProperties { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("{ ");
            AppendMember(nameof(Client), Client, isFirst: true);
            IfNotNullNorWhitespace(ProjectId, () => AppendMember(nameof(ProjectId), ProjectId));
            IfNotNullNorWhitespace(Version, () => AppendMember(nameof(Version), Version));
            IfNotNullNorWhitespace(Player, () => AppendMember(nameof(Player), Player));
            IfNotNullNorWhitespace(To, () => AppendMember(nameof(To), To));
            IfNotNullNorWhitespace(Value, () => AppendMember(nameof(Value), Value));
            IfNotNullNorWhitespace(GasLimit, () => AppendMember(nameof(GasLimit), GasLimit));
            IfNotNullNorWhitespace(GasPrice, () => AppendMember(nameof(GasPrice), GasPrice));

            if (CustomProperties != null)
            {
                foreach (var pair in CustomProperties)
                {
                    if (pair.Key == null || pair.Value == null)
                    {
                        // TODO: Should integrate logging mechanism. Currently skips if key/value pair not found
                        continue;
                    }

                    AppendMember(pair.Key, pair.Value);
                }
            }

            sb.Append(" }");
            return sb.ToString();

            void AppendMember(string name, object value, bool isFirst = false)
            {
                if (!isFirst)
                {
                    sb.Append(", ");
                }

                sb.Append(name);
                sb.Append(" = ");
                sb.Append(value);
            }

            void IfNotNullNorWhitespace(string value, Action then)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                then();
            }
        }
    }
}