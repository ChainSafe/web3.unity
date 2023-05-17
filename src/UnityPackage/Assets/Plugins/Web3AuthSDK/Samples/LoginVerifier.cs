public class LoginVerifier {
    public string name { get; set; }
    public Provider loginProvider { get; set; }

    public LoginVerifier(string name, Provider loginProvider)
    {
        this.name = name;
        this.loginProvider = loginProvider;
    }
}