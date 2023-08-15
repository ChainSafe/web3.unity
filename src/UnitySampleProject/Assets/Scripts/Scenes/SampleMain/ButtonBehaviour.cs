using UnityEngine;
using UnityEngine.UI;

namespace Samples.Behaviours
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(Execute);
        }

        protected abstract void Execute();
    }
}