using System.Collections.Generic;
#nullable enable
public class WhiteLabelData {
    public string? appName { get; set; }
    public string? logoLight { get; set; }
    public string? logoDark { get; set; }
    public Web3Auth.Language? defaultLanguage { get; set; } = Web3Auth.Language.en;
    public Web3Auth.ThemeModes? mode { get; set; } = Web3Auth.ThemeModes.light;
    public Dictionary<string, string>? theme { get; set; }
    public string? appUrl { get; set; }
    public bool? useLogoLoader { get; set; } = false;
}