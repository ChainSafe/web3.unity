using UnityEngine;

public static class StringExtensions
{
    public static string ToHexString(this Color color)
    {
        var r = (int)(color.r * 255);
        var g = (int)(color.g * 255);
        var b = (int)(color.b * 255);
        var a = (int)(color.a * 255);

        return $"{r:X2}{g:X2}{b:X2}{a:X2}";
    }
}