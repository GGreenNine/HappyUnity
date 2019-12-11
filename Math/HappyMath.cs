using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HappyUnity.Math
{
    public class HappyMath : MonoBehaviour
    {
        /// <summary>
        /// Remaps a value x in interval [A,B], to the proportional value in interval [C,D]
        /// </summary>
        /// <param name="x">The value to remap.</param>
        /// <param name="A">the minimum bound of interval [A,B] that contains the x value</param>
        /// <param name="B">the maximum bound of interval [A,B] that contains the x value</param>
        /// <param name="C">the minimum bound of target interval [C,D]</param>
        /// <param name="D">the maximum bound of target interval [C,D]</param>
        public static float Remap(float x, float A, float B, float C, float D)
        {
            float remappedValue = C + (x-A)/(B-A) * (D - C);
            return remappedValue;
        }
        
        /// <summary>
        /// Returns a random success based on X% of chance.
        /// </summary>
        /// <param name="percent">Percent of chance.</param>
        public static bool Chance(int percent)
        {
            return (UnityEngine.Random.Range(0,100) <= percent);
        }
        
        /// <summary>
        /// Compares 2 floating point numbers to an approximate value
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static bool ApproximatelyEqual(float first, float second, float precision)
        {
            return Mathf.Abs(first - second ) <= precision;
        }
        
        /// <summary>
        /// Calls OverlapSphereNonAlloc* and checks if an object exists in this area
        /// </summary>
        /// <param name="place"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool FindColliders(Vector3 place, Collider target, float area)
        {
            Collider[] colliders = { };
            Physics.OverlapSphereNonAlloc(place, area,colliders );
            return colliders.Any(colOut => colOut == target);
        }
        
    }


    
    
}

