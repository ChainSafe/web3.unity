using System;
using System.Threading.Tasks;


public static class SafeDelay
{
    // Task.Delay doesn't work on WebGL
    public static async Task WaitForSeconds(float seconds)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        var now = Time.time;
        while (Time.time - now < seconds)
        {
            await Task.Yield();
        }
#else
        await Task.Delay(TimeSpan.FromSeconds(seconds));
#endif
    }
}