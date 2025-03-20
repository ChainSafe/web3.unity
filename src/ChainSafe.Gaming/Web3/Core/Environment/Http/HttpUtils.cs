using System.Collections.Generic;
using System.Linq;

namespace ChainSafe.Gaming.Web3.Environment.Http
{
    public static class HttpUtils
    {
        public static string BuildUriParameters(Dictionary<string, string> parameters)
        {
            var parametersRaw = parameters
                .Where(p => !string.IsNullOrWhiteSpace(p.Value))
                .Select(pair => $"{pair.Key}={pair.Value}");

            return $"?{string.Join("&", parametersRaw)}";
        }
    }
}