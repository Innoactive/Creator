using System;
using UnityEngine;

namespace Innoactive.Hub.Training.SceneObjects.Properties
{
    public class TransformInRangeDetectorProperty : TrainingSceneObjectProperty
    {
        private bool isTransformInRange = false;
        private Transform trackedTransform;

        public float DetectionRange { get; set; }

        public class RangeEventArgs : EventArgs
        {
            public readonly Transform TrackedTransform;
            public RangeEventArgs(Transform trackedTransform)
            {
                TrackedTransform = trackedTransform;
            }
        }

        public event EventHandler<RangeEventArgs> EnteredRange;
        public event EventHandler<RangeEventArgs> ExitedRange;

        private void Update()
        {
            Refresh();
        }

        /// <summary>
        /// Check if there are transforms in range and fire the appropriate events.
        /// </summary>
        public void Refresh()
        {
            if (trackedTransform == null)
            {
                return;
            }

            bool isInsideArea = IsTargetInsideRange();

            if (isInsideArea && isTransformInRange == false)
            {
                EmitEnteredArea();
                isTransformInRange = true;
            }
            else if (isInsideArea == false && isTransformInRange)
            {
                EmitExitedArea();
                isTransformInRange = false;
            }
        }

        protected virtual bool IsTargetInsideRange()
        {
            // Compare squared distance for better performance
            float distanceSqr = Vector3.SqrMagnitude(transform.position - trackedTransform.position);
            bool isInsideRange = (distanceSqr <= DetectionRange * DetectionRange);

            return isInsideRange;
        }

        public void SetTrackedTransform(Transform transformToBeTracked)
        {
            trackedTransform = transformToBeTracked;
        }

        public void DestroySelf()
        {
            Destroy(this);
        }

        protected void EmitEnteredArea()
        {
            if (EnteredRange != null)
            {
                EnteredRange.Invoke(this, new RangeEventArgs(trackedTransform));
            }
        }

        protected void EmitExitedArea()
        {
            if (ExitedRange != null)
            {
                ExitedRange.Invoke(this, new RangeEventArgs(trackedTransform));
            }
        }
    }
}
