using System;
using System.Collections.Generic;
using SharpDX.Mathematics;
using Point = DelaunayVoronoi.Point;

namespace Delaunay
{
    public static class RandomExtensions
    {
        public static double NextGaussian(this Random r, double mu = 0, double sigma = 1)
        {
            var u1 = r.NextDouble();
            var u2 = r.NextDouble();

            var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                  Math.Sin(2.0 * Math.PI * u2);

            var rand_normal = mu + sigma * rand_std_normal;

            return rand_normal;
        }
        
        /// <summary>Generates a random angle [0,2*pi)</summary>
        public static float RandomAngleRadians(this Random r)
        {
            return r.NextFloat(0,1) * 6.283185f;
        }
        /// <summary>
        /// Generates a random point inside the circle with specified radius.
        /// </summary>
        public static Point InCircle(this Random r, float radius = 1f)
        {
            var num = radius * (float)Math.Sqrt(r.NextFloat(0, 1));
            float f   = RandomAngleRadians(r);
            var point = new Point((num * (Math.Cos(f))) + 1500, (num * (Math.Sin(f))+1500));
            return point;
        }

    }
    
}