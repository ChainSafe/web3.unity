using System.Collections.Generic;
using UnityEngine;
#if UNITY_2022_3_OR_NEWER
using Unity.VectorGraphics;
#endif

namespace Reown.AppKit.Unity.Utils
{
    public class UiUtils
    {
#if UNITY_2022_3_OR_NEWER
        private static GradientStop[] GenerateGradientStops(Color baseColor)
        {
            var stops = new GradientStop[5];
            for (var i = 0; i < 5; i++)
            {
                var tint = 0.25f * i;
                var tintedColor = Color.Lerp(Color.white, baseColor, tint);
                stops[i] = new GradientStop
                {
                    Color = tintedColor,
                    StopPercentage = i * 0.25f
                };
            }

            return stops;
        }
#endif

        public static Texture2D GenerateAvatarTexture(string address)
        {
            var baseColorHex = address.Substring(2, 6);
            if (!ColorUtility.TryParseHtmlString($"#{baseColorHex}", out var baseColor))
                return null;

#if UNITY_2022_3_OR_NEWER
            var circleShape = new Shape
            {
                Fill = new GradientFill
                {
                    Type = GradientFillType.Radial,
                    Stops = GenerateGradientStops(baseColor),
                    RadialFocus = new Vector2(0.3f, -0.2f)
                }
            };
            VectorUtils.MakeCircleShape(circleShape, Vector2.zero, 4.0f);

            var scene = new Scene
            {
                Root = new SceneNode
                {
                    Shapes = new List<Shape>
                    {
                        circleShape
                    }
                }
            };

            var tessOptions = new VectorUtils.TessellationOptions
            {
                StepDistance = 100.0f,
                MaxCordDeviation = 0.5f,
                MaxTanAngleDeviation = 0.1f,
                SamplingStepSize = 0.01f
            };

            var geoms = VectorUtils.TessellateScene(scene, tessOptions);
            var sprite = VectorUtils.BuildSprite(geoms, 10.0f, VectorUtils.Alignment.Center, Vector2.zero, 16, true);

            var mat = Resources.Load<Material>("Fonts & Materials/AvatarGradientMaterial");
            var texture = VectorUtils.RenderSpriteToTexture2D(sprite, 128, 128, mat);

            return texture;
#else
            const int size = 128;
            var texture = new Texture2D(size, size);

            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                    texture.SetPixel(x, y, baseColor);
            }

            texture.Apply();
            return texture;
#endif
        }
    }
}