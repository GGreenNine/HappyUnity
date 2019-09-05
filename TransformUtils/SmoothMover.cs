using HappyUnity.Math;
using UnityEngine;

namespace HappyUnity.TransformUtils
{
    public class SmoothMover : MonoBehaviour
    {
        public Vector3 TargetPosition { get; set; }
        public float SmoothTime;
        private Vector3 velocity = Vector3.zero;
        private bool stopWhenReach;
        public bool Moving { get; private set; }
        public float EndPrecision = 0.0001f;
        public bool UseFixedDeltaTime;

        public void BeginMoving(bool stopWhenReach = true)
        {
            this.stopWhenReach = stopWhenReach;
            Moving = true;
        }

        public void StopMoving()
        {
            Moving = false;
            velocity = Vector3.zero;
        }

        protected virtual void Update()
        {
            if (Moving)
            {
                if (TargetPosition.PrecisionEquals(transform.position, EndPrecision))
                {
                    transform.position = TargetPosition;
                    velocity = Vector3.zero;
                    if (stopWhenReach)
                    {
                        Moving = false;
                    }
                }
                else
                {
                    Vector3 newPosition = Vector3.SmoothDamp(transform.position, TargetPosition, ref velocity,
                        SmoothTime,
                        Mathf.Infinity, UseFixedDeltaTime ? Time.fixedDeltaTime : Time.deltaTime);
                    transform.position = newPosition;
                }
            }
        }
    }
}