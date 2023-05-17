using System;

public class LoginParams
{
    public Provider loginProvider { get; set; }
    public string dappShare { get; set; }
    public ExtraLoginOptions extraLoginOptions { get; set; }
    public Uri redirectUrl { get; set; }
    public string appState { get; set; }
    public MFALevel mfaLevel { get; set; }
    public int sessionTime { get; set; }

    public Curve curve { get; set; }
}