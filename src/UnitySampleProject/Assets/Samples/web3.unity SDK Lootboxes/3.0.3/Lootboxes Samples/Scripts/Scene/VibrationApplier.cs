using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace LootBoxes.Chainlink.Scene
{
    public class VibrationApplier : MonoBehaviour
    {
        public float Speed = 10f;
        [FormerlySerializedAs("Range")]
        public float PositionRange = 0.07f;
        public float RotationRange = 5f;
        public AnimationCurve ScaleByValue = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        private new Transform transform;
        private Vector2[] posPerlinSeeds;
        private Vector2[] rotPerlinSeeds;

        private void Awake()
        {
            transform = GetComponent<Transform>();
            posPerlinSeeds = new Vector2[]
            {
                new(Random.Range(-100f, 100f), Random.Range(-100f, 100f)),
                new(Random.Range(-100f, 100f), Random.Range(-100f, 100f)),
                new(Random.Range(-100f, 100f), Random.Range(-100f, 100f)),
            };

            rotPerlinSeeds = new Vector2[]
            {
                new(Random.Range(-100f, 100f), Random.Range(-100f, 100f)),
                new(Random.Range(-100f, 100f), Random.Range(-100f, 100f)),
                new(Random.Range(-100f, 100f), Random.Range(-100f, 100f)),
            };
        }

        private void Update()
        {
            var scaledTime = Time.time * Speed;

            // position
            if (PositionRange != 0)
            {
                var posOffset = new Vector3(
                    FetchValue(scaledTime, posPerlinSeeds[0]),
                    FetchValue(scaledTime, posPerlinSeeds[1]),
                    FetchValue(scaledTime, posPerlinSeeds[2])
                ) * PositionRange;

                transform.localPosition = posOffset;
            }

            // rotation
            if (RotationRange != 0)
            {
                var rotationOffset = new Vector3(
                    FetchValue(scaledTime, rotPerlinSeeds[0]),
                    FetchValue(scaledTime, rotPerlinSeeds[1]),
                    FetchValue(scaledTime, rotPerlinSeeds[2])
                ) * RotationRange;

                transform.localRotation = Quaternion.Euler(rotationOffset);
            }
        }

        private float FetchValue(float scaledTime, Vector2 seed)
        {
            var signedValue = Mathf.PerlinNoise(seed.x + scaledTime, seed.y + scaledTime);
            var absValue = Mathf.Abs(signedValue);
            var curveScaledValue = ScaleByValue.Evaluate(absValue);
            return Mathf.Sign(signedValue) * curveScaledValue;
        }
    }
}