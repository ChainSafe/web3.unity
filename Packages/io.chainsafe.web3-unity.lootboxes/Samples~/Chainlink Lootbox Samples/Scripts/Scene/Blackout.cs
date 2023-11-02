using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace LootBoxes.Chainlink.Scene
{
    public class Blackout : MonoBehaviour
    {
        public Image image;
        public float Speed = 3f;
        public bool FadeOutInitially = true;

        private CancellationTokenSource activeTaskCancelSource = new();

        private float Alpha
        {
            get => image.color.a;
            set
            {
                var color = image.color;
                color.a = value;
                image.color = color;
            }
        }

        private void OnValidate()
        {
            if (!image)
            {
                return;
            }

            Alpha = 0f;
        }

        private void Awake()
        {
            Alpha = FadeOutInitially ? 1f : 0f;
        }

        public Task FadeOut(float timeScale = 1f)
        {
            CancelActiveTask();
            return FadeTo(1f, activeTaskCancelSource.Token, timeScale);
        }

        public Task FadeIn(float timeScale = 1f)
        {
            CancelActiveTask();
            return FadeTo(0f, activeTaskCancelSource.Token, timeScale);
        }

        private async Task FadeTo(float alpha, CancellationToken cancellationToken, float timeScale)
        {
            while (Mathf.Abs(alpha - Alpha) > 0.01f)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                Alpha = Mathf.Lerp(Alpha, alpha, Time.unscaledDeltaTime * Speed * timeScale);

                await Task.Yield();
            }

            // finalize
            Alpha = alpha;
        }

        private void CancelActiveTask()
        {
            activeTaskCancelSource.Cancel();
            activeTaskCancelSource = new CancellationTokenSource();
        }
    }
}