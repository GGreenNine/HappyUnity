using UnityEngine;

namespace HappyUnity.Extensions
{
    public static class QuaternionExtensions
    {
        
        /// <summary>
        /// Checks the approximate position of quaternions
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static bool ApproximatelyEqual(this Quaternion q1, Quaternion q2, float precision)
        {
            return Mathf.Abs(Quaternion.Dot(q1, q2)) >= 1 - precision;
        }

        public static void SetX(this Quaternion q, float x)
        {
            q.x = x;
        }
        public static void SetY(this Quaternion q, float y)
        {
            q.y = y;
        }
        public static void SetZ(this Quaternion q, float z)
        {
            q.z = z;
        }
        public static void SetW(this Quaternion q, float w)
        {
            q.w = w;
        }
    
        public static Quaternion ScalarMultiply(this Quaternion input, float scalar)
        {
            return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
        }
    
    
    }
}