using ChainSafe.Gaming.UnityPackage;
using UnityEngine;
using Web3Unity.Scripts.Prefabs;

/* This prefab script should be copied & placed on the root of an object.
Change the class name, variables and add any additional changes at the end of the execute function.
The initialize function should be called by a method of your choosing */

/// <summary>
/// Encrypts a message with SHA3
/// </summary>
public class Sha3 : MonoBehaviour
{
    // Variables
    private string message = "It’s dangerous to go alone, take this!";
    private UnsortedSample logic;

    /// <summary>
    /// Starts the task, you can put this in the start function or call it from a button/event
    /// </summary>
    public void InitializeTask()
    {
        // Sets the sample behaviour & executes
        logic = new UnsortedSample(Web3Accessor.Web3);
        ExecuteTask();
    }

    /// <summary>
    /// Executes the prefab task and sends the result to the console, you can also save this into a variable for later use
    /// </summary>
    private void ExecuteTask()
    {
        var hash = logic.Sha3(message);
        SampleOutputUtil.PrintResult(hash, nameof(UnsortedSample), nameof(UnsortedSample.Sha3));
    }
}