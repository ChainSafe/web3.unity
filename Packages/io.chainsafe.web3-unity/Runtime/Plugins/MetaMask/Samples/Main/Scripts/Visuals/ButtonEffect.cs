using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MetaMask.Unity.Samples
{
    public class ButtonEffect : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public enum ButtonState
        {
            Normal,
            Hover,
            Click
        }

        #region Fields

        /// <summary>The Normal Button Color.</summary>
        public Color NormalColor;
        /// <summary>The Hover Button Color.</summary>
        public Color HoverColor;
        /// <summary>The Click Button Color.</summary>
        public Color ClickColor;
        /// <summary>The target scale of the button.</summary>
        public float targetScale;
        /// <summary>The time it takes to lerp between two values.</summary>       
        public float timeToLerp = 0.25f;
        /// <summary>Gets or sets the value of the reverseValue field.</summary>
        /// <value>The value of the reverseValue field.</value>
        public float reverseValue;
        /// <summary>The particle systems to play.</summary>
        public ParticleSystem[] particle;
        /// <summary>The state of the button.</summary>
        public ButtonState buttonState;
        /// <summary>The button that displays the image.</summary>
        private Button image;
        /// <summary>The audio source for the button object.</summary>
        private AudioSource audioSource;
        /// <summary>The sound played when the mouse hovers over a button.</summary>
        public AudioClip hoverSound;
        /// <summary>The sound played when a button is clicked.</summary>
        public AudioClip clickSound;
        /// <summary>The sound to play when the user clicks on a negative button.</summary>
        public AudioClip clickSoundNegative;
        /// <summary>Gets or sets a value indicating whether animations are disabled.</summary>
        /// <value>true if animations are disabled; otherwise, false.</value>
        public bool DisableAnimation;
        public bool DisableStartAnimation;

        #endregion

        #region Unity Methods

        /// <summary>Called when the application is enabled.</summary>
        private void OnEnable()
        {
            this.image = GetComponent<Button>();
            
            if (!DisableStartAnimation)
                StartCoroutine(LerpFunction(1, 1f, new Vector3(1f, 1f, 1f), 0));
            
            this.audioSource = GetComponent<AudioSource>();
        }

        /// <summary>Updates the button's visual state.</summary>
        private void Update()
        {
            if (this.buttonState == ButtonState.Normal)
            {
                ColorBlock colors = this.image.colors;
                colors.normalColor = this.NormalColor;
                colors.selectedColor = this.NormalColor;
                this.image.colors = colors;
            }
        }

        #endregion

        #region Coroutines

        /// <summary>Lerps the local scale of the object.</summary>
        /// <param name="endValue">The end value of the lerp.</param>
        /// <param name="duration">The duration of the lerp.</param>
        /// <param name="startScale">The start scale of the object.</param>
        /// <param name="startValue">The start value of the lerp.</param>
        private IEnumerator LerpFunction(float endValue, float duration, Vector3 startScale, float startvalue)
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

        /// <summary>A coroutine that scales the button to a given scale.</summary>
        /// <param name="endValue">The scale to scale to.</param>
        /// <param name="duration">The duration of the scaling.</param>
        /// <param name="startScale">The starting scale of the button.</param>
        /// <returns>A coroutine that scales the button to a given scale.</returns>
        private IEnumerator ButtonClick(float endValue, float duration, Vector3 startScale)
        {
            float time = 0;
            float startValue = 1;
            while (time < 0.125f)
            {
                transform.localScale = startScale * Mathf.Lerp(startValue, endValue, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            transform.localScale = startScale * endValue;
            StartCoroutine(LerpFunction(this.targetScale, this.timeToLerp, new Vector3(1, 1, 1), 1));
        }

        #endregion

        #region Event Handlers

        /// <summary>Called when the pointer enters the button.</summary>
        /// <param name="eventData">The event data.</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            StartCoroutine(LerpFunction(this.targetScale, this.timeToLerp, new Vector3(1, 1, 1), 1));
            this.buttonState = ButtonState.Hover;
            ColorBlock colors = this.image.colors;
            colors.normalColor = this.HoverColor;
            colors.selectedColor = this.HoverColor;
            this.image.colors = colors;
            this.audioSource.PlayOneShot(this.hoverSound);
        }

        /// <summary>Called when the pointer exits the button.</summary>
        /// <param name="eventData">The event data.</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            StopAllCoroutines();
            StartCoroutine(LerpFunction(this.reverseValue, this.timeToLerp, transform.localScale, 1));
            this.buttonState = ButtonState.Normal;
            ColorBlock colors = this.image.colors;
            colors.normalColor = this.NormalColor;
            colors.selectedColor = this.NormalColor;
            this.image.colors = colors;
        }

        /// <summary>Handles the pointer click event.</summary>
        /// <param name="eventData">The pointer event data.</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (this.DisableAnimation)
            {
                return;
            }

            if (this.image.interactable)
            {
                foreach (ParticleSystem emitter in this.particle)
                {
                    emitter.Play();
                }

                StartCoroutine(ButtonClick(0.97f, this.timeToLerp, new Vector3(1.05f, 1.05f, 1.05f)));
                this.buttonState = ButtonState.Click;
                ColorBlock colors = this.image.colors;
                colors.normalColor = this.ClickColor;
                colors.selectedColor = this.ClickColor;
                this.image.colors = colors;
                this.audioSource.PlayOneShot(this.clickSound);
            }
            else
            {
                this.audioSource.PlayOneShot(this.clickSoundNegative);
            }
        }

        #endregion
    }
}