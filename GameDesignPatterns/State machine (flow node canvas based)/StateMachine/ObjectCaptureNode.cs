using System;
using System.Collections;
using System.Linq;
using HappyUnity.Math;
using UnityEngine;

namespace UnityMultiplayer.Scripts.StateMachine.Nodes
{
    public class ObjectCaptureNode : StateNode
    {
        public ObjectCaptureContainer[] captureContainers = new ObjectCaptureContainer[1];
        public CaptureCheckSettings captureCheckSettings;

        private WaitForSeconds s = new WaitForSeconds(.3f);

        Coroutine captureChekingWorkflow_ = null;

        IEnumerator CaptureChekingWorkflow()
        {
            while (true)
            {
                yield return s;
                switch (captureCheckSettings)
                {
                    case CaptureCheckSettings.ForAny:
                        var d = captureContainers.FirstOrDefault(x => x.CheckForCapture());
                        if (d != null)
                        {
                            d.objectToCapture.gameObject.AddComponent<FollowTarget>().SetTarget(d.shadowCaptureObject);
                            MoveNext();
                            yield break;
                        }

                        break;
                    case CaptureCheckSettings.ForEach:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        [ShowInGraphNodeInspector("Move next")]
        public override void MoveNext()
        {
            //StopCoroutine(captureChekingWorkflow_);
            base.MoveNext();
        }

        public override void Enter(StateNode node)
        {
            captureChekingWorkflow_ = StartCoroutine(CaptureChekingWorkflow());
            base.Enter(node);
        }
    }

    [Serializable]
    public class ObjectCaptureContainer
    {
        public Transform shadowCaptureObject;
        public Transform objectToCapture;

        public CaptureRotation captureRotation;

        public bool captured = false;

        public float devideX = 360;
        public float devideY = 360;
        public float devideZ = 360;
        
        public bool HasCaptureFlag(CaptureRotation flag)
        {
            return (captureRotation & flag) == flag;
        }

        public bool CheckForCapture()
        {
            Debug.Log(HappyMath.ApproximatelyEqual(shadowCaptureObject.eulerAngles.y % devideY, objectToCapture.eulerAngles.y % devideY, 0.1f));

            if (objectToCapture.position.ApproximatelyEqual(shadowCaptureObject.position, .003f))
            {
                if (HasCaptureFlag(CaptureRotation.X) && HasCaptureFlag(CaptureRotation.Y) &&
                    HasCaptureFlag(CaptureRotation.Z))
                {
                    if (objectToCapture.rotation.ApproximatelyEqual(shadowCaptureObject.rotation, .003f))
                    {
                        captured = true;
                        return captured;
                    }
                }

                if (HasCaptureFlag(CaptureRotation.X) && HasCaptureFlag(CaptureRotation.Y))
                {
                    if (HappyMath.ApproximatelyEqual(shadowCaptureObject.eulerAngles.x % devideX, objectToCapture.eulerAngles.x % devideX, 3f) &&
                        HappyMath.ApproximatelyEqual(shadowCaptureObject.eulerAngles.y % devideY, objectToCapture.eulerAngles.y % devideY, 3f))
                    {
                        captured = true;
                        return captured;
                    }
                }
                if (HasCaptureFlag(CaptureRotation.X) && HasCaptureFlag(CaptureRotation.Z))
                {
                    if (HappyMath.ApproximatelyEqual(shadowCaptureObject.eulerAngles.x % devideX, objectToCapture.eulerAngles.x % devideX, 3f) &&
                        HappyMath.ApproximatelyEqual(shadowCaptureObject.eulerAngles.z % devideZ, objectToCapture.eulerAngles.z % devideZ, 3f))
                    {
                        captured = true;
                        return captured;
                    }
                }

            }

            return false;
        }
    }

    [System.Flags]
    public enum CaptureRotation
    {
        None,
        X = 1 << 0,
        Y = 1 << 1,
        Z = 1 << 2,
    }

    public enum CaptureCheckSettings
    {
        ForAny,
        ForEach
    }
}