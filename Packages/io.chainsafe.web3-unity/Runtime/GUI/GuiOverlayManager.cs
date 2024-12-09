using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace ChainSafe.Gaming.GUI
{
    public class GuiOverlayManager : MonoBehaviour
    {
        public GuiScreenFactory screenFactory;
        public Transform container;

        private readonly List<GuiInfoOverlay> activeOverlays = new();

        private ObjectPool<GuiInfoOverlay> pool;

        private int overlayCounter = 1000; // offset to detect when default integer value is sent to one of the methods

        private void Awake()
        {
            pool = new ObjectPool<GuiInfoOverlay>(CreateOverlay, InitOverlay, ReleaseOverlay, defaultCapacity: 2);
        }

        public int Show(GuiOverlayType type, string message, bool deactivateOnClick, Action onClose = null)
        {
            var overlay = pool.Get();
            activeOverlays.Add(overlay);
            overlay.Initialize(overlayCounter++, type, message, deactivateOnClick, onClose, OnReleaseRequested);
            return overlay.Id;
        }

        public void Hide(int overlayId)
        {
            var overlay = activeOverlays.Find(o => o.Id == overlayId);

            if (overlay == null)
            {
                throw new InvalidOperationException($"There is no active Overlay with id {overlayId} to hide.");
            }

            overlay.Hide();
            activeOverlays.Remove(overlay);
        }

        public void UpdateOverlay(int overlayId, string message)
        {
            var overlay = activeOverlays.Find(o => o.Id == overlayId);
            overlay.UpdateMessage(message);
        }

        private GuiInfoOverlay CreateOverlay()
        {
            var overlay = screenFactory.Build<GuiInfoOverlay>();
            overlay.transform.SetParent(container);
            return overlay;
        }

        private void InitOverlay(GuiInfoOverlay overlay)
        {
            overlay.transform.SetSiblingIndex(container.childCount); // push to end
            overlay.gameObject.SetActive(true);
        }

        private void ReleaseOverlay(GuiInfoOverlay overlay)
        {
            overlay.gameObject.SetActive(false);
        }

        private void OnReleaseRequested(GuiInfoOverlay overlay)
        {
            pool.Release(overlay);
        }
    }

    public enum GuiOverlayType
    {
        Error,
        Loading
    }
}