using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming
{
    public class SampleContainer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        
        [SerializeField] private Button showScriptButton;
        
        [field: SerializeField] public Transform Container { get; private set; }

        public void Attach(ISample instance)
        {
            
        }
    }
}
