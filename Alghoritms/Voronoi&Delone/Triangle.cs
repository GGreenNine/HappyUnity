﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace HappyUnity.Alghoritms
{
    public class Triangle
    {
        public  Point[] vertices = new Point[3];
        public  Point   circumcenter;
        private double  _radiusSquared;
        private double  _radius;

        public HashSet<Triangle> TrianglesWithSharedEdge
        {
            get
            {
                var neighbors = new HashSet<Triangle>();
                foreach (var vertex in vertices)
                {
                    foreach (var triangle in vertex.AdjacentTriangles)
                    {
                        if (triangle != this && this.SharesEdgeWith(triangle))
                        {
                            neighbors.Add(triangle);
                        }
                    }
                }

                return neighbors;
            }
        }

        public Triangle(in Point point1, in Point point2, in Point point3)
        {
            if (!IsCounterClockwise(point1, point2, point3))
            {
                vertices[0] = point1;
                vertices[1] = point3;
                vertices[2] = point2;
            }
            else
            {
                vertices[0] = point1;
                vertices[1] = point2;
                vertices[2] = point3;
            }

            vertices[0].AdjacentTriangles.Add(this);
            vertices[1].AdjacentTriangles.Add(this);
            vertices[2].AdjacentTriangles.Add(this);
            UpdateCircumcircle();
        }

        private void UpdateCircumcircle()
        {
            var p0 = vertices[0];
            var p1 = vertices[1];
            var p2 = vertices[2];
            var dA = p0.X * p0.X + p0.Y * p0.Y;
            var dB = p1.X * p1.X + p1.Y * p1.Y;
            var dC = p2.X * p2.X + p2.Y * p2.Y;

            var aux1 = (dA * (p2.Y - p1.Y) + dB * (p0.Y - p2.Y) + dC * (p1.Y - p0.Y));
            var aux2 = -(dA * (p2.X - p1.X) + dB * (p0.X - p2.X) + dC * (p1.X - p0.X));
            var div  = (2 * (p0.X * (p2.Y - p1.Y) + p1.X * (p0.Y - p2.Y) + p2.X * (p1.Y - p0.Y)));

            if (div == 0)
            {
                throw new System.Exception();
            }

            var center = new Point(aux1 / div, aux2 / div);
            circumcenter   = center;
            _radiusSquared = (center.X - p0.X) * (center.X - p0.X) + (center.Y - p0.Y) * (center.Y - p0.Y);
        }

        private bool IsCounterClockwise(in Point point1, in Point point2, in Point point3)
        {
            var result = (point2.X - point1.X) * (point3.Y - point1.Y) -
                         (point3.X - point1.X) * (point2.Y - point1.Y);
            return result > 0;
        }

        public bool SharesEdgeWith(in Triangle triangle)
        {
            int shaderVerticesCount = 0;

            foreach (var vertex in vertices)
            {
                if (triangle.vertices.Contains(vertex))
                {
                    shaderVerticesCount++;
                }
            }

            return shaderVerticesCount == 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsPointInsideCircumcircle(in Point point)
        {
            var d_squared = (point.X - circumcenter.X) * (point.X - circumcenter.X) +
                            (point.Y - circumcenter.Y) * (point.Y - circumcenter.Y);

            return d_squared < _radiusSquared;
        }
    }
}