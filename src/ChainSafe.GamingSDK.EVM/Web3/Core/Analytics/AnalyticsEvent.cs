using System;
using System.Collections.Generic;
using System.Text;

namespace ChainSafe.GamingWeb3.Analytics
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
                    var key = pair.Key ?? "Unnamed";
                    var value = pair.Value ?? "Unknown";

                    if (pair.Key == null || pair.Value == null)
                    {
                        Console.WriteLine($"Warning: Null detected in CustomProperties. Key: {pair.Key}, Value: {pair.Value}. Default values have been assigned.");
                    }

                    AppendMember(key, value);
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