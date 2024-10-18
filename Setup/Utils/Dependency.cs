using Newtonsoft.Json;

namespace Setup.Utils;

public struct Dependency
{
    [JsonProperty("path")]
    public string Path { get; private set; }

    [JsonProperty("namespaces")]
    public string[] Namespaces { get; private set; }
}