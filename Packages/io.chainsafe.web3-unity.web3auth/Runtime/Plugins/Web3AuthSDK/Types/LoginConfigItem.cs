public class LoginConfigItem {
    public string verifier { get; set; }
    public TypeOfLogin typeOfLogin { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string clientId { get; set; }
    public string verifierSubIdentifier { get; set; }
    public string logoHover { get; set; }
    public string logoLight { get; set; }
    public string logoDark { get; set; }
    public bool mainOption { get; set; } = false;
    public bool showOnModal { get; set; } = true;
    public bool showOnDesktop { get; set; } = true;
    public bool showOnMobile { get; set; } = true;
}