using System.Collections.Generic;
using UnityEngine;

namespace HappyUnity.Alghoritms
{
    public class Voronoi
    {
        public HashSet<Edge> GenerateEdgesFromDelaunay(in HashSet<Triangle> triangulation)
        {
            var voronoiEdges = new HashSet<Edge>();
            foreach (var triangle in triangulation)
            {
                foreach (var neighbor in triangle.TrianglesWithSharedEdge)
                {
                    var edge = new Edge(triangle.circumcenter, neighbor.circumcenter);
                    voronoiEdges.Add(edge);
                }
            }

            return voronoiEdges;
        }
    }
}
