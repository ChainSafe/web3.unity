using UnityEngine;

namespace MetaMask.Unity.Samples
{
    public class BackgroundShape : MonoBehaviour
    {
        #region Fields

        /// <summary>The speed at which the camera moves.</summary>
        public float speed = 0.1f;
        /// <summary>The speed at which the camera rotates.</summary>
        public float degreesPerSecond = 15.0f;
        /// <summary>The X-axis animation curve.</summary>
        public AnimationCurve curveX;
        /// <summary>The initial position of the object.</summary>
        public Vector3 InitialPosition;
        /// <summary>The final position of the object.</summary>
        public Vector3 FinalPosition;
        /// <summary>Gets or sets the rotation of the object.</summary>
        /// <value>The rotation of the object.</value>
        public Vector3 Rotation;
        /// <summary>The position of the curve.</summary>
        private float evalpos;
        /// <summary>A boolean property that toggles between true and false.</summary>
        /// <value>True if the property is true, false otherwise.</value>
        private bool pingpong;

        #endregion

        #region Unity Methods

        /// <summary>Updates the object's position.</summary>
        private void Update()
        {
            // Spin object around Y-Axis
            transform.Rotate(this.Rotation * this.degreesPerSecond * Time.deltaTime);

            // Call evaluate on that time   
            this.evalpos += this.speed * Time.deltaTime;
            if (!this.pingpong)
            {
                transform.localPosition = Vector3.Lerp(this.InitialPosition, this.FinalPosition,
                    this.curveX.Evaluate(this.evalpos));
                if (transform.localPosition == this.FinalPosition)
                {
                    this.evalpos = 0;
                    this.pingpong = true;
                }
            }
            else
            {
                transform.localPosition =
                    Vector3.Lerp(this.FinalPosition, this.InitialPosition, this.curveX.Evaluate(this.evalpos));
                if (transform.localPosition == this.InitialPosition)
                {
                    this.evalpos = 0;
                    this.pingpong = false;
                }
            }
        }

        #endregion
    }
}