using System.Collections.Generic;

public class WhiteLabelData { 
    public string name { get; set; }
    public string logoLight { get; set; }
    public string logoDark { get; set; }
    public string defaultLanguage { get; set; } = "en";
    public bool dark { get; set; } = false;
    public Dictionary<string, string> theme { get; set; }
}