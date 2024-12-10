using UnityEngine;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.Rendering;

namespace Reown.AppKit.Unity.Utils
{
    /// <summary>
    ///     A utility class for encoding QR codes
    /// </summary>
    public class QRCode
    {
        public static Texture2D EncodeTexture(string textForEncoding, int width = 512, int height = 512)
        {
            return EncodeTexture(textForEncoding, Color.black, Color.white, width, height);
        }

        public static Texture2D EncodeTexture(string textForEncoding, Color fgColor, Color bgColor, int width = 512, int height = 512)
        {
            var pixels = EncodePixels(textForEncoding, fgColor, bgColor, width, height);

            var texture = new Texture2D(width, height);
            texture.SetPixels32(pixels);
            texture.filterMode = FilterMode.Point;
            texture.Apply();
            texture.Compress(true);

            return texture;
        }

        public static Color32[] EncodePixels(string textForEncoding, int width = 512, int height = 512)
        {
            return EncodePixels(textForEncoding, Color.black, Color.white, width, height);
        }

        public static Color32[] EncodePixels(string textForEncoding, Color fgColor, Color bgColor, int width = 512, int height = 512)
        {
            var qrCodeEncodingOptions = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width,
                Margin = 4,
                QrVersion = 11
            };

            var writer = new BarcodeWriter<Color32[]>
            {
                Format = BarcodeFormat.QR_CODE,
                Options = qrCodeEncodingOptions,
                Renderer = new Color32Renderer
                {
                    Foreground = fgColor,
                    Background = bgColor
                }
            };

            return writer.Write(textForEncoding);
        }
    }
    
    /// <summary>
    ///     A barcode renderer which returns a Color32 array
    /// </summary>
    /// <remarks>
    ///     Based on https://github.com/micjahn/ZXing.Net/blob/master/Source/lib/unity/Color32Renderer.cs
    /// </remarks>
    public class Color32Renderer : IBarcodeRenderer<Color32[]>
    {
        /// <summary>
        ///     Gets or sets the foreground color.
        /// </summary>
        /// <value>
        ///     The foreground color.
        /// </value>
        [System.CLSCompliant(false)]
        public Color32 Foreground { get; set; }
        /// <summary>
        ///     Gets or sets the background color.
        /// </summary>
        /// <value>
        ///     The background color.
        /// </value>
        [System.CLSCompliant(false)]
        public Color32 Background { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Color32Renderer"/> class.
        /// </summary>
        public Color32Renderer()
        {
            Foreground = Color.black;
            Background = Color.white;
        }

        /// <summary>
        ///     Renders the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="format">The format.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        [System.CLSCompliant(false)]
        public Color32[] Render(BitMatrix matrix, BarcodeFormat format, string content)
        {
            return Render(matrix, format, content, null);
        }

        /// <summary>
        ///     Renders the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="format">The format.</param>
        /// <param name="content">The content.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        [System.CLSCompliant(false)]
        public Color32[] Render(BitMatrix matrix, BarcodeFormat format, string content, EncodingOptions options)
        {
            var result = new Color32[matrix.Width * matrix.Height];
            var offset = matrix.Height - 1;
            var foreground = Foreground;
            var background = Background;
            var divisionLeftover = matrix.Width % 32;
            var bitRowRemainder = divisionLeftover > 0 ? divisionLeftover : 32;

            for (int y = 0; y < matrix.Height; y++)
            {
                var ba = matrix.getRow(offset - y, null);
                int[] bits = ba.Array;

                for (int x = 0; x < bits.Length; x++)
                {
                    int finalIndex = 32;

                    if (x == bits.Length - 1)
                    {
                        finalIndex = bitRowRemainder;
                    }

                    for (int i = 0; i < finalIndex; i++)
                    {
                        int bit = (bits[x] >> i) & 1;

                        if (bit == 1)
                        {
                            result[matrix.Width * y + x * 32 + i] = new Color32(foreground.r, foreground.g, foreground.b, foreground.a);
                        }
                        else
                        {
                            result[matrix.Width * y + x * 32 + i] = new Color32(background.r, background.g, background.b, background.a);
                        }
                    }
                }
            }

            return result;
        }
    }
}