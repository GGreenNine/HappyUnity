using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions 
{
    public static bool ApproximatelyEqual(this Vector2 current, Vector2 other, float precision)
    {
        return Vector2.SqrMagnitude(current - other) < precision;
    }

    public static bool ApproximatelyEqual(this Vector3 lhs, Vector3 rhs, float precision)
    {
        return Vector3.SqrMagnitude(lhs - rhs) < precision;
    }

    public static bool ApproximatelyEqual(this Vector4 lhs, Vector4 rhs, float precision)
    {
        return Vector4.SqrMagnitude(lhs - rhs) < precision;
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
}
}
