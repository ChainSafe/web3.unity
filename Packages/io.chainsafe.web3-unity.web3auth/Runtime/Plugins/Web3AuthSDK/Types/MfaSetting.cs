using System.Collections.Generic;
#nullable enable
public class MfaSetting
{
    public bool enable { get; set; }
    public int? priority { get; set; }
    public bool? mandatory { get; set; }

    // Constructor
    public MfaSetting(bool enable, int? priority, bool? mandatory)
    {
        this.enable = enable;
        this.priority = priority;
        this.mandatory = mandatory;
    }
}