using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ChainSafe.Gaming;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.UI;
using ChainSafe.Gaming.Web3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Samples : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private SampleContainer sampleContainerPrefab;
    [SerializeField] private Button buttonPrefab;
    
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var samples = GetComponentsInChildren<ISample>();
        
        foreach (var sample in samples)
        {
            var methods = sample.GetType().GetMethods().Where(m =>
                m.ReturnType == typeof(Task<string>)
                && m.GetParameters().Length == 0
                && m.IsPublic).ToArray();

            if (!methods.Any())
            {
                continue;
            }
            
            var sampleContainer = Instantiate(sampleContainerPrefab, container);
            
            sampleContainer.Attach(sample);
            
            foreach (var method in methods)
            {
                var button = Instantiate(buttonPrefab, sampleContainer.Container);

                var buttonText = button.GetComponentInChildren<TMP_Text>();

                buttonText.text = method.Name;
                
                button.onClick.AddListener(delegate
                {
                    TryExecute(method, sample);
                });
            }
        }
    }

    private async void TryExecute(MethodInfo method, ISample instance)
    {
        try
        {
            LoadingOverlay.ShowLoadingOverlay();

            if (!Web3Unity.Connected && !(instance is ILightWeightSample))
            {
                throw new Web3Exception("Connection not found. Please connect your wallet first.");
            }
            
            string message = await Execute(method, instance);
            
            Debug.Log(message);
        }
        // Todo: display error via error overlay
        finally
        {
            LoadingOverlay.HideLoadingOverlay();
        }
    }
    
    private Task<string> Execute(MethodInfo method, ISample instance)
    {
        return (Task<string>) method.Invoke(instance, null);
    }
}
