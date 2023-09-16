using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

internal static class EditorUtilities
{
    public static async Task SendAndWait(UnityWebRequest request)
    {
        var _ = request.SendWebRequest();

        // Note: cannot run coroutines from within editor windows
        while (request.result == UnityWebRequest.Result.InProgress)
        {
            await Task.Yield();
        }
    }
}
