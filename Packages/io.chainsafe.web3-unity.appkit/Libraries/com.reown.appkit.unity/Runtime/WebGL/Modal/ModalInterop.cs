using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using AOT;
using Newtonsoft.Json;
using Reown.AppKit.Unity.WebGl.Wagmi;

namespace Reown.AppKit.Unity.WebGl.Modal
{
    public static class ModalInterop
    {
#if UNITY_WEBGL 
        [DllImport("__Internal")]
#endif
        private static extern void ModalCall(int id, string methodName, string payload, InteropService.ExternalMethodCallback callback);

#if UNITY_WEBGL 
        [DllImport("__Internal")]
#endif
        private static extern void ModalSubscribeState(Action<string> callback);
        
        public static event Action<ModalState> StateChanged;
        
        private static readonly InteropService InteropService = new(ModalCall);
        
        private static bool _eventsInitialised;
        
        public static Task<TRes> InteropCallAsync<TReq, TRes>(string methodName, TReq requestParameter, CancellationToken cancellationToken = default)
        {
            return InteropService.InteropCallAsync<TReq, TRes>(methodName, requestParameter, cancellationToken);
        }
        
        // -- Events --------------------------------------------------

        public static void InitializeEvents()
        {
            if(_eventsInitialised)
                return;
            
            ModalSubscribeState(SubscribeStateCallback);
            
            _eventsInitialised = true;
        }
        
        [MonoPInvokeCallback(typeof(Action<string>))]
        public static void SubscribeStateCallback(string stateJson)
        {
            var state = JsonConvert.DeserializeObject<ModalState>(stateJson);
            StateChanged?.Invoke(state);
        }
        
        
        // -- Open Modal ----------------------------------------------

        public static async void Open(OpenModalParameters parameters = null)
        {
            await OpenModalAsync(parameters);
        }
        
        public static async Task OpenModalAsync(OpenModalParameters parameters = null)
        {
            await InteropCallAsync<OpenModalParameters, object>(ModalMethods.Open, parameters);
        }
        
        
        // -- Close Modal ---------------------------------------------
        
        public static async void Close()
        {
            await CloseModalAsync();
        }
        
        public static async Task CloseModalAsync()
        {
            await InteropCallAsync<object, object>(ModalMethods.Close, null);
        }
    }
}