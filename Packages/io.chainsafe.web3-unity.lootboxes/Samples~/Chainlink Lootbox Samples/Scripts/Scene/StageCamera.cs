using UnityEngine;

namespace LootBoxes.Chainlink.Scene
{
    public class StageCamera : MonoBehaviour
    {
        public float distanceFromItem = 10f;
        public float lerpSpeed = 10f;

        private Vector3 targetPosition;
        private Stage stage;
        private Quaternion targetRotation;
        private new Transform transform;

        public void Configure(Stage stage)
        {
            this.stage = stage;
        }

        private void Awake()
        {
            transform = GetComponent<Transform>();
            targetPosition = transform.position;
            targetRotation = transform.rotation;
        }

        private void Update()
        {
            var positionSynced = (transform.position - targetPosition).magnitude < 0.001f;
            var rotationSynced = Quaternion.Angle(transform.rotation, targetRotation) < 0.01f;
            if (positionSynced && rotationSynced)
            {
                return;
            }

            var lerpT = Time.deltaTime * lerpSpeed;
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpT);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpT);
        }

        public void LookAt(int stageItemIndex)
        {
            var itemPosition = stage.GetStagePosition(stageItemIndex);
            var vector = stage.GetStageVector(stageItemIndex);

            targetPosition = itemPosition + vector * distanceFromItem;
            targetPosition.y = transform.position.y;
            targetRotation = Quaternion.LookRotation(-vector);
        }

        public void LookAtImmediately(int stageItemIndex)
        {
            LookAt(stageItemIndex);

            transform.position = targetPosition;
            transform.rotation = targetRotation;
        }
    }
}