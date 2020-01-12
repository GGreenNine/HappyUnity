using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace HappyUnity.Alghoritms
{
    public class TilesGrid : MonoBehaviour
    {
        public Grid tilemapGrid;

        [Header("Tilemap with obstacles")] public Tilemap obstacles;

        private int xMax = 0;
        private int xMin = 0;

        private int yMax = 0;
        private int yMin = 0;

        public Node[,] grid;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            grid.Initialize();
            GetFieldBounds();
            GetMap();
        }

        /// <summary>
        /// Getting field boundaries
        /// </summary>
        private void GetFieldBounds()
        {
            var tilemaps = tilemapGrid.transform.GetComponentsInChildren<Tilemap>();
            foreach (var tilemap in tilemaps)
            {
                //Get max bounds
                if (tilemap.cellBounds.xMax > xMax)
                    xMax = tilemap.cellBounds.xMax;
                if (tilemap.cellBounds.yMax > yMax)
                    yMax = tilemap.cellBounds.yMax;

                //Get min bounds
                if (tilemap.cellBounds.yMin < yMin)
                    yMin = tilemap.cellBounds.yMin;
                if (tilemap.cellBounds.xMin < xMin)
                    xMin = tilemap.cellBounds.xMin;
            }
        }

        /// <summary>
        /// Generating the grid map
        /// </summary>
        private void GetMap()
        {
            grid = new Node[Mathf.Abs(xMax - xMin), Mathf.Abs(yMax - yMin)];

            for (int x = xMin; x < xMax; x++)
            {
                for (int y = yMin; y < yMax; y++)
                {
                    Vector3Int localPlace = (new Vector3Int(x, y, (int) obstacles.transform.position.y));
                    Vector3    place      = obstacles.CellToWorld(localPlace);

                    if (obstacles.HasTile(localPlace))
                    {
                        grid[x + Mathf.Abs(xMin), y + Mathf.Abs(yMin)] = new Node(place, localPlace,
                                x + Mathf.Abs(xMin), y + Mathf.Abs(yMin), true);
                    }
                    else
                    {
                        grid[x + Mathf.Abs(xMin), y + Mathf.Abs(yMin)] = new Node(place, localPlace,
                                x + Mathf.Abs(xMin), y + Mathf.Abs(yMin), false);
                    }
                }
            }
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < xMax + +Mathf.Abs(xMin) && checkY >= 0 &&
                        checkY < yMax + +Mathf.Abs(yMin))
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }

        public Node GetNodeByWorldPos(Vector3Int pos)
        {
            return grid[pos.x + Mathf.Abs(xMin), pos.y + Mathf.Abs(yMin)];
        }

//    private void OnDrawGizmos()
//    {
//        if (grid.Length <= 0) return;
//        foreach (var TB in grid)
//        {
//            if (TB.IsObstacle)
//            {
//                Gizmos.color = Color.red;
//                Gizmos.DrawCube(TB.WorldTilePos, Vector3.one);
//            }
//            else
//            {
//                Gizmos.color = Color.white;
//                Gizmos.DrawCube(TB.WorldTilePos, Vector3.one);
//            }
//        }
//    }
    }
}