using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace MetaMask.Unity
{
    /// <summary>
    ///     A unity web request awaiter to hook them into async/await practice.
    /// </summary>
    public class UnityWebRequestAwaiter : INotifyCompletion
    {
        #region Fields

        /// <summary>Retrieves the content of the clipboard as text.</summary>
        /// <returns>The content of the clipboard as text.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the application isn't in foreground.</exception>
        private readonly UnityWebRequestAsyncOperation _asyncOp;

        /// <summary>Continues the action after the window is activated.</summary>
        /// <param name="continuation">The continuation to continue.</param>
        private Action _continuation;

        #endregion

        #region Constructors

        public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOp)
        {
            this._asyncOp = asyncOp;
            asyncOp.completed += OnRequestCompleted;
        }

        #endregion

        #region Public Methods

        public bool IsCompleted => this._asyncOp.isDone;

        /// <summary>Retrieves the result of the last operation.</summary>
        public void GetResult() { }

        #endregion

        /// <summary>Called when the web request is retrieved.</summary>
        /// <param name="continuation">The continuation to call when the request content is retrieved.</param>
        public void OnCompleted(Action continuation)
        {
            this._continuation = continuation;
        }

        /// <summary>Invoked when the request content is retrieved.</summary>
        /// <param name="obj">The object that is the source of the event.</param>
        private void OnRequestCompleted(AsyncOperation obj)
        {
            this._continuation();
        }
    }

    public static class UnityWebRequestAwaiterExtensionMethods
    {
        /// <summary>Provides an awaiter for a <see cref="UnityWebRequestAsyncOperation" />.</summary>
        /// <param name="asyncOp">The <see cref="UnityWebRequestAsyncOperation" /> to await.</param>
        /// <returns>An awaiter for the <see cref="UnityWebRequestAsyncOperation" />.</returns>
        public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
        {
            return new UnityWebRequestAwaiter(asyncOp);
        }
    }
}