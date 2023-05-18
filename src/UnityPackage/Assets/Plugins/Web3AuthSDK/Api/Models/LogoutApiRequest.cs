using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoutApiRequest
{
    public string key { get; set; }
    public string data { get; set; }
    public string signature { get; set; }
    public long timeout { get; set; }
}
