using Plugins.CountlySDK.Helpers;
using System;
using System.Threading.Tasks;

namespace Notifications
{
    public interface INotificationsService
    {
        bool IsInitializedWithoutError { get; set; }
        void GetToken(Action<string> result);
        void OnNotificationClicked(Action<string, int> result);
        void OnNotificationReceived(Action<string> result);
        Task<CountlyResponse> ReportPushActionAsync();
    }
}