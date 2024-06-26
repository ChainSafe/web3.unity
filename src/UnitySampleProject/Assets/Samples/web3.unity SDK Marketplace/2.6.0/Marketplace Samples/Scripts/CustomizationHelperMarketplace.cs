using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ChainSafe.Gaming.Marketplace
{
    /// <summary>
    /// Customization helper to assist with custom config colours.
    /// </summary>
    public static class CustomizationHelperMarketplace
    {
        /// <summary>
        /// Sets custom colours for menu and text objects.
        /// </summary>
        public static void SetCustomColours(
            TMP_FontAsset displayFont,
            List<GameObject> primaryBackgroundObjects, Color primaryBackgroundColour,
            List<GameObject> menuBackgroundObjects, Color menuBackgroundColour,
            List<GameObject> primaryTextObjects, Color primaryTextColour,
            List<GameObject> secondaryTextObjects, Color secondaryTextColour,
            List<GameObject> borderButtonObjects, Color borderButtonColour,
            List<GameObject> displayLineObjects)
        {
            var objectsAndColours = new List<(List<GameObject> objects, Color color)>
            {
                (primaryBackgroundObjects, primaryBackgroundColour),
                (menuBackgroundObjects, menuBackgroundColour),
                (primaryTextObjects, primaryTextColour),
                (secondaryTextObjects, secondaryTextColour)
            };

            foreach (var (objects, colour) in objectsAndColours)
            {
                foreach (var item in objects)
                {
                    var imageRenderer = item.GetComponent<Image>();
                    if (imageRenderer != null)
                    {
                        imageRenderer.color = colour;
                        var imageBorder = item.GetComponent<Outline>();
                        if (imageBorder != null)
                        {
                            imageBorder.effectColor = secondaryTextColour;
                        }
                    }

                    var textMeshPro = item.GetComponent<TextMeshProUGUI>();
                    if (textMeshPro != null)
                    {
                        textMeshPro.font = displayFont;
                        textMeshPro.color = colour;
                    }
                }
            }

            SetButtonsAndLines(borderButtonObjects, borderButtonColour, displayLineObjects, secondaryTextColour);
        }

        /// <summary>
        /// Sets border buttons & menu lines.
        /// </summary>
        private static void SetButtonsAndLines(
            List<GameObject> borderButtonObjects, Color borderButtonColour,
            List<GameObject> displayLineObjects, Color secondaryTextColour)
        {
            var objectsAndColours = new List<(List<GameObject> objects, Color color)>
            {
                (borderButtonObjects, borderButtonColour),
                (displayLineObjects, borderButtonColour)
            };
            foreach (var (objects, colour) in objectsAndColours)
            {
                foreach (var item in objects)
                {
                    var imageRenderer = item.GetComponent<Image>();
                    if (imageRenderer != null)
                    {
                        imageRenderer.color = colour;
                        var imageBorder = item.GetComponent<Outline>();
                        if (imageBorder != null)
                        {
                            imageBorder.effectColor = secondaryTextColour;
                        }
                    }
                }
            }
        }
    }
}