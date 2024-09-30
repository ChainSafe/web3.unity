using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.UnityPackage.UI;
using UnityEngine;
using UnityEngine.UI;

public abstract class SampleBase<T> : Web3BuilderServiceAdapter where T : SampleBase<T>
{
    [Serializable]
    public struct ButtonActionPair
    {
        [field: SerializeField] public Button Button { get; private set; }
        
        [field: SerializeField] public string MethodName { get; private set; }
    }

    [field: SerializeField] public ButtonActionPair[] ButtonActionPairs { get; private set; }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        foreach (var pair in ButtonActionPairs)
        {
            pair.Button.onClick.AddListener(delegate
            {
                TryExecute(pair.MethodName);
            });
        }
    }

    private async void TryExecute(string methodName)
    {
        try
        {
            LoadingOverlay.ShowLoadingOverlay();

            string message = await Execute(methodName);
            
            Debug.Log(message);
        }
        // Todo: display error via error overlay
        finally
        {
            LoadingOverlay.HideLoadingOverlay();
        }
    }
    
    private Task<string> Execute(string methodName)
    {
        var type = GetType();

        var method = type.GetMethod(methodName);

        if (method == null)
        {
            throw new Exception($"{methodName}() not found in {typeof(T)}.");
        }

        try
        {
            return (Task<string>) method.Invoke((T) this, null);
        }
        catch (Exception)
        {
            Debug.LogError($"Error invoking {methodName}() Method must be parameterless and return Task<string>.");
            
            throw;
        }
    }
}
