using UnityEngine;

namespace HappyUnity.Extensions
{
    public static class VectorExtensions 
    {
        
        /// <summary>
        /// Checks the approximate position of vectors
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static bool ApproximatelyEqual(this Vector2 current, Vector2 other, float precision)
        {
            return Vector2.SqrMagnitude(current - other) < precision;
        }
        
        /// <summary>
        /// Projects a point on a line (perpendicularly) and returns the projected point.
        /// </summary>
        /// <returns>The point on line.</returns>
        /// <param name="point">Point.</param>
        /// <param name="lineStart">Line start.</param>
        /// <param name="lineEnd">Line end.</param>
        public static Vector3 ProjectPointOnLine(this Vector3 point, Vector3 lineStart, Vector3 lineEnd)
        {
            Vector3 rhs = point - lineStart;
            Vector3 vector2 = lineEnd - lineStart;
            float magnitude = vector2.magnitude;
            Vector3 lhs = vector2;
            if (magnitude > 1E-06f)
            {
                lhs = (Vector3)(lhs / magnitude);
            }
            float num2 = Mathf.Clamp(Vector3.Dot(lhs, rhs), 0f, magnitude);
            return (lineStart + ((Vector3)(lhs * num2)));
        }
        
        /// <summary>
        /// Changes X position
        /// </summary>
        /// <param name="current"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector3 SetX(this Vector3 current, float value)
        {
            return new Vector3(value, current.y,current.z);
        }
        
        /// <summary>
        /// Changes Y position
        /// </summary>
        /// <param name="current"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector3 SetY(this Vector3 current, float value)
        {
            return new Vector3(current.x, value,current.z);
        }
        
        /// <summary>
        /// Sets given object on the direction by distance X
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="start"></param>
        /// <param name="rayDot"></param>
        /// <param name="distance"></param>
        public static void SetPointOnTheLine(this Vector3 direction,Vector3 start, GameObject rayDot, float distance)
        {
            rayDot.transform.position = direction * distance + start;
        }
        
        /// <summary>
        /// Changes Z position
        /// </summary>
        /// <param name="current"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector3 SetZ(this Vector3 current, float value)
        {
            return new Vector3(current.x, current.y,value);
        }
        
}
}
