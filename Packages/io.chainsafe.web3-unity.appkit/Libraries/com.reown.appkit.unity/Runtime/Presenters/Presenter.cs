using System;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity
{
    public abstract class PresenterBase : IDisposable
    {
        public virtual string Title { get; protected set; } = string.Empty;

        public virtual bool HeaderBorder { get; protected set; } = true;

        /// <summary>
        ///     If true, the close button will be shown in the modal header.
        /// </summary>
        public virtual bool EnableCloseButton { get; protected set; } = true;

        public RouterController Router { get; protected set; }

        public virtual VisualElement ViewVisualElement { get; protected set; }

        public bool IsVisible { get; private set; }

        private bool _disposed;

        /// <summary>
        ///     Called when the view becomes visible.
        /// </summary>
        public void OnVisible()
        {
            IsVisible = true;
            OnVisibleCore();
        }

        /// <summary>
        ///     Called when the view becomes hidden, but still remains in the Router stack.
        /// </summary>
        public void OnHide()
        {
            IsVisible = false;
            OnHideCore();
        }

        /// <summary>
        ///     Called when the view is completely removed from the Router stack.
        /// </summary>
        public void OnDisable()
        {
            IsVisible = false;
            OnDisableCore();
        }


        public void ShowViewVisualElement()
        {
            ViewVisualElement.style.display = DisplayStyle.Flex;
        }

        public void HideViewVisualElement()
        {
            ViewVisualElement.style.display = DisplayStyle.None;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                IsVisible = false;
            }

            _disposed = true;
        }

        protected abstract void OnVisibleCore();

        protected abstract void OnHideCore();

        protected abstract void OnDisableCore();
    }

    public class Presenter<TView> : PresenterBase where TView : VisualElement, new()
    {
        protected TView View { get; set; }

        protected VisualElement Parent { get; }

        private bool _disposed;

        public override VisualElement ViewVisualElement
        {
            get => View;
        }

        public Presenter(RouterController router, VisualElement parent, bool hideView = true)
        {
            Router = router;
            Parent = parent;
            BuildView(hideView);
        }

        /// <summary>
        ///     Builds the view and adds it to the parent view (usually <see cref="RouterController.RootVisualElement" /> of the Router).
        /// </summary>
        protected void BuildView(bool hideView)
        {
            View = CreateViewInstance();
            View.style.display = hideView ? DisplayStyle.None : DisplayStyle.Flex;
            Parent.Add(View);
        }

        /// <summary>
        ///     Implements the creation of the view instance.
        /// </summary>
        protected virtual TView CreateViewInstance()
        {
            return new TView();
        }

        /// <summary>
        ///     Called when the view becomes visible.
        /// </summary>
        protected override void OnVisibleCore()
        {
        }

        /// <summary>
        ///     Called when the view becomes hidden, but still remains in the Router stack.
        /// </summary>
        protected override void OnHideCore()
        {
        }

        /// <summary>
        ///     Called when the view is completely removed from the Router stack.
        /// </summary>
        protected override void OnDisableCore()
        {
        }

        /// <summary>
        ///     Router will dispose the presenter if it's no longer needed.
        ///     This happens when another presenter of the same typed is registered in the Router via <see cref="RouterController.RegisterDefaultModalViews" />
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (View != null)
                    Parent.Remove(View);
            }

            _disposed = true;
            base.Dispose(disposing);
        }
    }
}