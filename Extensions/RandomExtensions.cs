using System;
using System.Collections.Generic;
using HappyUnity.Alghoritms;
using UnityEngine;
using Random = System.Random;

    public static class RandomExtensions
    {
        /// <summary>
        /// Get random guassian double
        /// </summary>
        /// <param name="r"></param>
        /// <param name="mu"></param>
        /// <param name="sigma"></param>
        /// <returns></returns>
        public static double NextGaussian(this Random r, double mu = 0, double sigma = 1)
        {
            var u1 = r.NextDouble();
            var u2 = r.NextDouble();

            var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                  Math.Sin(2.0 * Math.PI * u2);

            var rand_normal = mu + sigma * rand_std_normal;

            return rand_normal;
        }
        
        /// <summary>
        /// Get Random Float Value
        /// </summary>
        /// <param name="random"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float NextFloat(this Random random, float min, float max)
        {
            return Lerp(min, max, (float) random.NextDouble());
        }
        
        public static float Lerp(float from, float to, float amount)
        {
            return (float) ((1.0 - (double) amount) * (double) from + (double) amount * (double) to);
        }
        
        /// <summary>Generates a random angle [0,2*pi)</summary>
        public static float RandomAngleRadians(this Random r)
        {
            return r.NextFloat(0,1) * 6.283185f;
        }
        /// <summary>
        /// Generates a random point inside the circle with specified radius.
        /// </summary>
        public static Vector2 InCircle(this Random r, float radius = 1f)
        {
            var num = radius * (float)Math.Sqrt(r.NextFloat(0, 1));
            float f   = RandomAngleRadians(r);
            var point = new Vector2((num * (float)(Math.Cos(f))) + 1500, (num * (float)(Math.Sin(f))+1500));
            return point;
        }
        
        /// <summary>
        /// Generates a random point inside the circle with specified radius.
        /// </summary>
        public static Point InCircleGetPoint(this Random r, float radius = 1f)
        {
            var   num   = radius * (float)Math.Sqrt(r.NextFloat(0, 1));
            float f     = RandomAngleRadians(r);
            var   point = new Point((num * (float)(Math.Cos(f))) + 1500, (num * (float)(Math.Sin(f))+1500));
            return point;
        }

    }
