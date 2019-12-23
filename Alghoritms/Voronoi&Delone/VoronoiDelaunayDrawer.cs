using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using HappyUnity.Alghoritms;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class VoronoiDelaunayDrawer : MonoBehaviour
{
    private readonly Delaunay delaunay = new Delaunay();
    private readonly Voronoi  voronoi  = new Voronoi();

    public GenerationType getn_type;
    public int            pointsCount = 200;
    public int maxX = 200;
    public int maxY = 200;
    public LineRenderer   lineRenderer;

    private List<Point>       points        = new List<Point>();
    private HashSet<Triangle> triangulation = new HashSet<Triangle>();
    private HashSet<Edge>     vornoiEdges   = new HashSet<Edge>();

    [ContextMenu("Draw delaunay voronoi")]
    public void Draw()
    {
        points = delaunay.GeneratePoints(pointsCount, maxX, maxY, getn_type);

        //_______
        triangulation = delaunay.BowyerWatson(in points);

        //_______
        vornoiEdges = voronoi.GenerateEdgesFromDelaunay(in triangulation);

        DrawPoints(in points);
        DrawVorornoi(in vornoiEdges);
        DrawTriangulationDeloune(in triangulation);
    }

    private void DrawTriangulationDeloune(in HashSet<Triangle> triangulation)
    {
    }

    private void DrawVorornoi(in HashSet<Edge> voronoiEdges)
    {
        var i = 0;
        lineRenderer.positionCount = voronoiEdges.Count * 2;
        foreach (var edge in voronoiEdges)
        {
            lineRenderer.SetPosition(i, new Vector3((float) edge.Point1.X, (float) edge.Point2.X));
            i++;
            lineRenderer.SetPosition(i, new Vector3((float) edge.Point1.Y, (float) edge.Point2.Y));
            i++;
            
//            lines.Lines.Add(new SegmentLine()
//            {
//                    AX = edge.Point1.X,
//                    BX = edge.Point2.X,
//                    AY = edge.Point1.Y,
//                    BY = edge.Point2.Y
//            });
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var edgeVoronoi in vornoiEdges)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3((float) edgeVoronoi.Point1.X, (float) edgeVoronoi.Point1.Y),
                    new Vector3((float) edgeVoronoi.Point2.X, (float) edgeVoronoi.Point2.Y));
        }

        foreach (var point in points)
        {
            Gizmos.DrawSphere(new Vector3((float) point.X,(float) point.Y), 1f);
        }

        var edges = new List<Edge>(triangulation.Count * 3) { };
        
        foreach (var triangle in triangulation)
        {
            edges.Add(new Edge(triangle.vertices[0], triangle.vertices[1]));
            edges.Add(new Edge(triangle.vertices[1], triangle.vertices[2]));
            edges.Add(new Edge(triangle.vertices[2], triangle.vertices[0]));
        }

        foreach (var edge in edges)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(new Vector3((float) edge.Point1.X, (float) edge.Point1.Y),
                    new Vector3((float) edge.Point2.X, (float) edge.Point2.Y));
        }
    }

    private void DrawPoints(in List<Point> points)
    {
    }
}