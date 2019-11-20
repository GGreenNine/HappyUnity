using HappyUnity.Math;
using UnityEngine;

namespace HappyUnity.Transforms
{
    public class CaptureObjectContainer : MonoBehaviour
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
            Debug.Log(HappyMath.ApproximatelyEqual(shadowCaptureObject.eulerAngles.y % devideY,
                objectToCapture.eulerAngles.y % devideY, 0.1f));

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
                    if (HappyMath.ApproximatelyEqual(shadowCaptureObject.eulerAngles.x % devideX,
                            objectToCapture.eulerAngles.x % devideX, 3f) &&
                        HappyMath.ApproximatelyEqual(shadowCaptureObject.eulerAngles.y % devideY,
                            objectToCapture.eulerAngles.y % devideY, 3f))
                    {
                        captured = true;
                        return captured;
                    }
                }

                if (HasCaptureFlag(CaptureRotation.X) && HasCaptureFlag(CaptureRotation.Z))
                {
                    if (HappyMath.ApproximatelyEqual(shadowCaptureObject.eulerAngles.x % devideX,
                            objectToCapture.eulerAngles.x % devideX, 3f) &&
                        HappyMath.ApproximatelyEqual(shadowCaptureObject.eulerAngles.z % devideZ,
                            objectToCapture.eulerAngles.z % devideZ, 3f))
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