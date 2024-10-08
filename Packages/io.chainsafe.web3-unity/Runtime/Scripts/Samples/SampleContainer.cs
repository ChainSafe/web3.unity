using System.Linq;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using CWeb3 = ChainSafe.Gaming.Web3.Web3;

namespace ChainSafe.Gaming
{
    public class SampleContainer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        [SerializeField] private Button showScriptButton;

        [field: SerializeField] public Transform Container { get; private set; }

        private ISample _sample;

        public void Initialize(ISample sample)
        {
            _sample = sample;

            titleText.text = sample.Title;
            descriptionText.text = sample.Description;

#if UNITY_EDITOR
            showScriptButton.onClick.AddListener(delegate
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = sample as Object;
            });
#endif
        }

        public void Web3Initialized(CWeb3 web3)
        {
            gameObject.SetActive(_sample.DependentServiceTypes.All(t => web3.ServiceProvider.GetService(t) != null));
        }
    }
}
