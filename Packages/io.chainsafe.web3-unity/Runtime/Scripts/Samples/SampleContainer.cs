using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming
{
    public class SampleContainer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        
        [SerializeField] private Button showScriptButton;
        
        [field: SerializeField] public Transform Container { get; private set; }

        public void Attach(ISample instance)
        {
            titleText.text = instance.Title;
            descriptionText.text = instance.Description;
            
#if UNITY_EDITOR
            showScriptButton.onClick.AddListener(delegate
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = instance as Object;
            });
#endif
        }
    }
}
