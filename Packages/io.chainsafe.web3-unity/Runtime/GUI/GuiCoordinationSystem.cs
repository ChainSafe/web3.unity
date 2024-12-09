using System.Collections.Generic;
using System.Linq;

namespace ChainSafe.Gaming.GUI
{
    public class GuiCoordinationSystem
    {
        private class ScreenRecord
        {
            public IGuiScreen Screen { get; }
            public bool Visible { get; set; }

            public ScreenRecord(IGuiScreen screen)
            {
                Screen = screen;
                Visible = false;
            }
        }

        private readonly List<ScreenRecord> activeScreens = new();
        private int currentVisibleSortOrder = int.MinValue;

        public void Register(IGuiScreen screen)
        {
            activeScreens.Add(new ScreenRecord(screen));
            UpdateStackVisibility();
        }

        public void Unregister(IGuiScreen screen)
        {
            var record = activeScreens.Find(r => r.Screen == screen);
            activeScreens.Remove(record);
            UpdateStackVisibility();
        }

        private static void ShowScreen(ScreenRecord activeScreen)
        {
            activeScreen.Visible = true;
            activeScreen.Screen.OnShowing();
        }

        private static void HideScreen(ScreenRecord activeScreen)
        {
            activeScreen.Visible = false;
            activeScreen.Screen.OnHiding();
        }

        private void UpdateStackVisibility()
        {
            var opaqueScreenLayers = activeScreens
                .Select(r => r.Screen.Layer)
                .Where(l => !l.Transparent);

            var newVisibleSortOrder = opaqueScreenLayers.Any()
                ? opaqueScreenLayers.Max(l => l.SortOrder)
                : int.MinValue;

            currentVisibleSortOrder = newVisibleSortOrder;

            foreach (var activeScreen in activeScreens)
            {
                var show = activeScreen.Screen.Layer.SortOrder >= currentVisibleSortOrder;

                if (show && !activeScreen.Visible)
                {
                    ShowScreen(activeScreen);
                    continue;
                }

                if (!show && activeScreen.Visible)
                {
                    HideScreen(activeScreen);
                    continue;
                }

                // ignore
            }
        }
    }
}