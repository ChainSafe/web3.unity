using System.Collections.Generic;

public class ExtraLoginOptions {
    public Dictionary<string, string> additionalParams { get; set; }
    public string domain { get; set; }
    public string client_id { get; set; }
    public string leeway { get; set; }
    public string verifierIdField { get; set; }
    public bool isVerifierIdCaseSensitive { get; set; }
    public Display display { get; set; }
    public Prompt prompt { get; set; }
    public string max_age { get; set; }
    public string ui_locales { get; set; }
    public string id_token { get; set; }
    public string id_token_hint { get; set; }
    public string login_hint { get; set; }
    public string acr_values { get; set; }
    public string scope { get; set; }
    public string audience { get; set; }
    public string connection { get; set; }
    public string state { get; set; }
    public string response_type { get; set; }
    public string nonce { get; set; }
    public string redirect_uri { get; set; }
}