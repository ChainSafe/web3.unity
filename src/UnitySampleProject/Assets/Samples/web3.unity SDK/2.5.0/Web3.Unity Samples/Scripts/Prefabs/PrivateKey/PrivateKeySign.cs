using UnityEngine;
using Web3Unity.Scripts.Prefabs;
using ChainSafe.Gaming.UnityPackage;

/* This prefab script should be copied & placed on the root of an object.
Change the class name, variables and add any additional changes at the end of the execute function.
The initialize function should be called by a method of your choosing */

/// <summary>
/// Signs a message using a private key
/// </summary>
public class PrivateKeySign : MonoBehaviour
{
    private string privateKey = "0x78dae1a22c7507a4ed30c06172e7614eb168d3546c13856340771e63ad3c0081";
    private string message = "hello";
    private UnsortedSample logic;
    
    /// <summary>
    /// Starts the task, you can put this in the start function or call it from a button/event
    /// </summary>
    public void InitializeTask()
    {
        // Sets the sample behaviour & executes
        logic = new UnsortedSample(Web3Accessor.Web3);
        ExecuteSample();
    }
    
    /// <summary>
    /// Executes the prefab task and sends the result to the console, you can also save this into a variable for later use
    /// </summary>
    private void ExecuteSample()
    {
        var signedMessage = logic.PrivateKeySign(privateKey, message);
        SampleOutputUtil.PrintResult(signedMessage.ToString(), nameof(UnsortedSample), nameof(UnsortedSample.PrivateKeySign));
    }
}