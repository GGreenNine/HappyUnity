using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HappyUnity.Alghoritms;
using Random = UnityEngine.Random;

public enum GenerationType
{
    Guassian,
    Random,
    Circle
}

public class Delaunay 
{
    private double MaxX;
    private double MaxY;

    private Triangle tri1;
    private Triangle tri2;

    private IEnumerable<Triangle> border;

    public List<Point> GeneratePoints(int amount, double maxX, double maxY, GenerationType generationType)
    {
        MaxX = maxX;
        MaxY = maxY;

        var point0 = new Point(0, 0);
        var point1 = new Point(0, MaxY);
        var point2 = new Point(MaxX, MaxY);
        var point3 = new Point(MaxX, 0);

        var points = new List<Point>(amount + 1) {point0, point1, point2, point3};

        tri1 = new Triangle(point0, point1, point2);
        tri2 = new Triangle(point0, point2, point3);

        border = new List<Triangle>() {tri1, tri2};

        var random = new System.Random();

        switch (generationType)
        {
            case GenerationType.Guassian:
                for (int i = 0; i < amount - 4; i++)
                {
                    var pointXG = Math.Abs(random.NextGaussian(0, 0.3f) * MaxX);
                    var pointYG = Math.Abs(random.NextGaussian(0, 0.3f) * MaxY);

                    points.Add(new Point(pointXG, pointYG));
                }

                break;
            case GenerationType.Random:
                for (int i = 0; i < amount - 4; i++)
                {
                    var pointX = random.NextDouble() * MaxX;
                    var pointY = random.NextDouble() * MaxY;
                    points.Add(new Point(pointX, pointY));
                }

                break;
            case GenerationType.Circle:
                for (int i = 0; i < amount - 4; i++)
                {
                    var point = random.InCircleGetPoint(1500);
                    points.Add(point);
                }

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(generationType), generationType, null);
        }

        return points;
    }

    public HashSet<Triangle> BowyerWatson(in List<Point> points)
    {
        var triangulation = new HashSet<Triangle>(border);

        Stopwatch s = new Stopwatch();
        s.Start();

//            
        foreach (var point in points)
        {
            var badTriangles = new List<Triangle>();

            FindBadTriangles(in point, in triangulation, ref badTriangles);

            if (badTriangles.Count > 0)
            {
                var polygon = FindHoleBoundaries(badTriangles);
                foreach (var edge in polygon)
                {
                    var triangle = new Triangle(point, edge.Point1, edge.Point2);
                    triangulation.Add(triangle);
                }
            }

            foreach (var triangle in badTriangles)
            {
                foreach (var vertex in triangle.vertices)
                {
                    vertex.AdjacentTriangles.Remove(triangle);
                }
            }

            triangulation.ExceptWith(badTriangles);
        }

        s.Stop();

        return triangulation;
    }

    private List<Edge> FindHoleBoundaries(List<Triangle> badTriangles)
    {
        var edges = new List<Edge>(badTriangles.Count * 3);

        foreach (var triangle in badTriangles)
        {
            edges.Add(new Edge(triangle.vertices[0], triangle.vertices[1]));
            edges.Add(new Edge(triangle.vertices[1], triangle.vertices[2]));
            edges.Add(new Edge(triangle.vertices[2], triangle.vertices[0]));
        }

        var boundaryEdges = edges.GroupBy(o => o).Where(o => o.Count() == 1).Select(o => o.First()).ToList();
        return boundaryEdges;
    }

    private void FindBadTriangles(in  Point             point,
                                  in  HashSet<Triangle> triangles,
                                  ref List<Triangle>    badTriangles)
    {
        foreach (var triangle in triangles)
        {
            if (triangle.IsPointInsideCircumcircle(in point))
                badTriangles.Add(triangle);
        }
    }
}
