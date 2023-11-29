using UnityEngine;

namespace MetaMask.Unity.Samples
{
    public class CanvasScaler : MonoBehaviour
    {
        #region Fields

        /// <summary>The cube prefab.</summary>
        public GameObject cube;
        /// <summary>Gets or sets the scale of the canvas.</summary>
        /// <value>The scale of the canvas.</value>
        public float scale = 1.0f;
        /// <summary>Boolean that controls wheter the UI should only scale on mobile</summary>
        public bool onlyScaleMobile;

        #endregion

        #region Unity Methods

        /// <summary>Updates the canvas's scale.</summary>
        private void Update()
        {
            var aspect = (float)Screen.width / Screen.height;
            if (aspect > 1.0f)
            {
                if (this.onlyScaleMobile)
                {
                    this.cube.transform.localScale = new Vector3(1f, 1f, 1f);
                    return;
                }
                float width = Screen.width;
                float dimension = width / this.scale;
                this.cube.transform.localScale = new Vector3(dimension, dimension, dimension);
            }
            else
            {
                this.cube.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            }
        }

        #endregion
    }
}