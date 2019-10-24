using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuaternionExtensions
{
    public static bool ApproximatelyEqual(this Quaternion q1, Quaternion q2, float precision)
    {
        return Mathf.Abs(Quaternion.Dot(q1, q2)) >= 1 - precision;
    }
}