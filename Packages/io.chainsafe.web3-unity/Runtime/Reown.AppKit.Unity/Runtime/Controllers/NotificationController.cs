using System;

namespace Reown.AppKit.Unity
{
    public sealed class NotificationController
    {
        public event EventHandler<NotificationEventArgs> Notification;

        public void Notify(NotificationType type, string message)
        {
            Notification?.Invoke(this, new NotificationEventArgs(type, message));
        }
    }

    public class NotificationEventArgs : EventArgs
    {
        public readonly NotificationType type;
        public readonly string message;

        public NotificationEventArgs(NotificationType type, string message)
        {
            this.type = type;
            this.message = message;
        }
    }

    public enum NotificationType
    {
        Info,
        Error,
        Success
    }
}