using System.Collections;

using UnityEngine;

namespace MetaMask.Unity.Samples
{
    public class TweenIn : MonoBehaviour
    {
        #region Fields

        /// <summary>The audio source used to capture audio.</summary>
        private AudioSource _audioSource;
        /// <summary>The audio clip to play.</summary>
        public AudioClip audioClip;
        /// <summary>Gets or sets whether the audio should be played.</summary>
        /// <value>Whether the audio should be played.</value>
        public bool playAudio = false;
        /// <summary>Gets or sets a value indicating whether the tween should be tweened in on start.</summary>
        /// <value>A value indicating whether the tween should be tweened in on start.</value>
        public bool tweenInOnStart = true;
        /// <summary>The time it takes to tween in.</summary>
        public float tweenInTime = 1f;
        
        #endregion

        #region Unity Methods
        
        /// <summary>Starts the tween.</summary>
        private void Start()
        {
            if (this.tweenInOnStart)
            {
                StartCoroutine(LerpFunction(1, 1f, new Vector3(1f, 1f, 1f), 0));

                if (this.playAudio == true)
                {
                    this._audioSource = GetComponent<AudioSource>();
                    this._audioSource.PlayOneShot(this.audioClip);
                }
            }
        }

        #endregion

        #region Coroutines

        /// <summary>Lerps the local scale of the object.</summary>
        /// <param name="endValue">The end value of the lerp.</param>
        /// <param name="duration">The duration of the lerp.</param>
        /// <param name="startScale">The start scale of the lerp.</param>
        /// <param name="startValue">The start value of the lerp.</param>
        public IEnumerator LerpFunction(float endValue, float duration, Vector3 startScale, float startvalue)
        {
            float time = 0;
            float startValue = startvalue;
            while (time < duration)
            {
                transform.localScale = startScale * Mathf.Lerp(startValue, endValue, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            transform.localScale = startScale * endValue;
        }

        #endregion

        #region Public Methods

        /// <summary>Close the window.</summary>
        public void Close()
        {
            StartCoroutine(LerpFunction(0, 0.5f, new Vector3(1f, 1f, 1f), 1));
            if (this.playAudio == true)
            {
                this._audioSource = GetComponent<AudioSource>();
                this._audioSource.PlayOneShot(this.audioClip);
            }
        }

        /// <summary>Opens the popup.</summary>
        public void Open()
        {
            StartCoroutine(LerpFunction(1, this.tweenInTime, new Vector3(1f, 1f, 1f), 0));

            if (this.playAudio == true)
            {
                this._audioSource = GetComponent<AudioSource>();
                this._audioSource.PlayOneShot(this.audioClip);
            }
        }

        #endregion
    }
}