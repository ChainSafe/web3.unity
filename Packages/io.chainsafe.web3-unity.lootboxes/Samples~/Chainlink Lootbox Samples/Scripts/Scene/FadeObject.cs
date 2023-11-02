using UnityEngine;

namespace LootBoxes.Chainlink.Scene
{
    [RequireComponent(typeof(Renderer))]
    [ExecuteInEditMode]
    public class FadeObject : MonoBehaviour
    {
        public float Alpha;

        private new Renderer renderer;

        private void Awake()
        {
            renderer = GetComponent<Renderer>();
            Alpha = renderer.sharedMaterial.color.a;
        }

        private void Update()
        {
            // keep using shared material until alpha changes
            if (Mathf.Approximately(Alpha, renderer.sharedMaterial.color.a))
            {
                return;
            }

            // don't update unique material until alpha changes
            if (Mathf.Approximately(Alpha, renderer.material.color.a))
            {
                return;
            }

            var material = renderer.material;
            var color = material.color;
            color.a = Alpha;
            material.color = color;
        }
    }
}