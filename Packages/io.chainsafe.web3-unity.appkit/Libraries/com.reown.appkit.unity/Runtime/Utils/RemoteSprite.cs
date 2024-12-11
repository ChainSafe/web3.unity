using System;
using System.Collections;
using System.Collections.Generic;
using Reown.Sign.Unity;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity.Utils
{
    public interface IImageHandler<in TImage>
    {
        void SetImageSprite(TImage image, Sprite sprite);
        void ClearImageSprite(TImage image);
    }

    public static class RemoteSpriteFactory
    {
        private static readonly Dictionary<string, object> UriSpritesMap = new();
        private static readonly Dictionary<Type, object> ImageHandlers = new();

        public static void RegisterImageHandler<TImage>(IImageHandler<TImage> handler) where TImage : class
        {
            ImageHandlers[typeof(TImage)] = handler;
        }

        public static RemoteSprite<TImage> GetRemoteSprite<TImage>(string uri) where TImage : class
        {
            if (!ImageHandlers.TryGetValue(typeof(TImage), out var handlerObj) || handlerObj is not IImageHandler<TImage> handler)
                throw new InvalidOperationException($"No handler registered for type {typeof(TImage).Name}.");

            if (UriSpritesMap.TryGetValue(uri, out var spriteObj) && spriteObj is RemoteSprite<TImage> sprite)
            {
                return sprite;
            }

            var newSprite = new RemoteSprite<TImage>(uri, handler);
            UriSpritesMap[uri] = newSprite;
            return newSprite;
        }
    }

    public class RemoteSprite<TImage> where TImage : class
    {
        private readonly IImageHandler<TImage> _imageHandler;
        private readonly LoadingAnimator _loadingAnimator;
        private readonly HashSet<TImage> _subscribedImages = new();
        private readonly string _uri;
        private bool _isLoaded;
        private bool _isLoading;
        private Sprite _sprite;

        internal RemoteSprite(string uri, IImageHandler<TImage> imageHandler)
        {
            _uri = uri;
            _imageHandler = imageHandler;
            _loadingAnimator = LoadingAnimator.Instance;
        }

        public void SubscribeImage(TImage image)
        {
            if (!_isLoaded && !_isLoading)
                UnityEventsDispatcher.Instance.StartCoroutine(LoadRemoteSprite());

            if (_isLoaded)
            {
                SetImage(image);
            }
            else
            {
                if (_loadingAnimator != null)
                    _loadingAnimator.Subscribe(image);
            }

            _subscribedImages.Add(image);
        }

        public void UnsubscribeImage(TImage image)
        {
            _imageHandler.ClearImageSprite(image);
            _subscribedImages.Remove(image);
        }

        private void SetImage(TImage image)
        {
            if (_loadingAnimator != null)
                _loadingAnimator.Unsubscribe(image);

            _imageHandler.SetImageSprite(image, _sprite);
        }

        private IEnumerator LoadRemoteSprite()
        {
            _isLoading = true;

            using (var uwr = UnityWebRequestTexture.GetTexture(_uri))
            {
                uwr.SetRequestHeader("accept", "image/jpeg,image/png");

                uwr.SetRequestHeader("x-project-id", AppKit.Config.projectId);
                uwr.SetRequestHeader("x-sdk-type", "appkit");
                uwr.SetRequestHeader("x-sdk-version", SignMetadata.Version);

                var origin = Application.identifier;
                if (!string.IsNullOrWhiteSpace(origin))
                {
                    uwr.SetRequestHeader("origin", origin);
                }  

                yield return uwr.SendWebRequest();

                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Failed to load remote sprite from {_uri}: {uwr.error}");
                }
                else
                {
                    // While UnityWebRequest creates texture in the background (on other thread), some finishing work is done on main thread.
                    // Skipping a few frames here to let Unity finish its work to reduce CPU spikes.
                    for (var i = 0; i < 5; i++)
                        yield return null;

                    var texture = DownloadHandlerTexture.GetContent(uwr);
                    var rect = new Rect(0.0f, 0.0f, texture.width, texture.height);
                    _sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 100.0f);
                    _isLoaded = true;

                    foreach (var image in _subscribedImages)
                        SetImage(image);
                }
            }

            _isLoading = false;
        }
    }

    public static class RemoteSpriteExtensions
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        internal static void Initialize()
        {
            RemoteSpriteFactory.RegisterImageHandler(new ImageHandlerUtk());
        }
    }

    public class ImageHandlerUtk : IImageHandler<Image>
    {
        public void SetImageSprite(Image image, Sprite sprite)
        {
            image.sprite = sprite;
        }

        public void ClearImageSprite(Image image)
        {
            if (image == null)
                return;

            image.sprite = null;
        }
    }
}