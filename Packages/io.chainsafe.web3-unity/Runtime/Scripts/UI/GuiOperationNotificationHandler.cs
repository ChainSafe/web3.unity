using ChainSafe.Gaming.GUI;
using ChainSafe.Gaming.Web3.Core.Operations;

namespace ChainSafe.Gaming.UnityPackage.UI
{
    public class GuiOperationNotificationHandler : IOperationNotificationHandler
    {
        private int overlayId;

        public void OnNotificationsAvailable()
        {
            overlayId = GuiManager.Instance.Overlays.Show(GuiOverlayType.Loading, string.Empty, false);
        }

        public void OnNotificationsOver()
        {
            GuiManager.Instance.Overlays.Hide(overlayId);
        }

        public void SetCurrentOperation(string message)
        {
            GuiManager.Instance.Overlays.UpdateOverlay(overlayId, message);
        }
    }
}