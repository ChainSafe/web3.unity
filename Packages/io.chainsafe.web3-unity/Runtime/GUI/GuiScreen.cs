using UnityEngine;

namespace ChainSafe.Gaming.GUI
{
    public class GuiScreen : MonoBehaviour, IGuiScreen
    {
        [field: SerializeField] public GuiLayer Layer { get; set; }

        public Canvas Canvas; // set layer sort order to canvas sort order?
        public GameObject Content;
        public Animation Animation;

        private void OnEnable()
        {
            GuiManager.Instance.Coordination.Register(this);
        }

        private void OnDisable()
        {
            GuiManager.Instance.Coordination.Unregister(this);
        }

        private void OnValidate()
        {
            if (Content == gameObject)
            {
                Content = null;
                Debug.LogError("You can not use the same GuiScreen gameObject as content.", gameObject);
            }
        }

        public void OnShowing()
        {
            Canvas.enabled = true;
            Content.gameObject.SetActive(true);

            if (Animation)
            {
                Animation.Play();
            }
        }

        public void OnHiding()
        {
            Canvas.enabled = false;
            Content.gameObject.SetActive(false);

            if (Animation)
            {
                Animation.Stop();
            }
        }
    }
}