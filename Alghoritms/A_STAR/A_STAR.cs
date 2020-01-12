using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using KouXiaGu.BinaryHeap;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HappyUnity.Alghoritms
{
    public class A_STAR : MonoBehaviour
    {
        public TilesGrid grid;

        public IEnumerable<Node> Path;

        public IEnumerable<Node> GetPath(Vector3Int targetPos)
        {
            var startPos = new Vector3Int((int) transform.position.x, (int) transform.position.y,
                    (int) transform.position.z);

            return AStar(startPos, targetPos);
        }

        public IEnumerable<Node> AStar(Vector3Int start, Vector3Int end)
        {
            var startNode = grid.GetNodeByWorldPos(start);
            var endNode   = grid.GetNodeByWorldPos(end);

            var openStack  = new MinHeap<Node>(grid.grid.Length) {startNode};
            var closeStack = new HashSet<Node>();

            while (openStack.Count > 0)
            {
                var lowestFNode = openStack.Extract();

                if (lowestFNode.Equals(endNode))
                {
                    Path = ReconstructThePath(startNode, lowestFNode);
                    return Path;
                }

                closeStack.Add(lowestFNode);

                foreach (Node neighbour in grid.GetNeighbours(lowestFNode))
                {
                    if (neighbour.IsObstacle || closeStack.Contains(neighbour))
                    {
                        continue;
                    }

                    int newCostToNeighbour = lowestFNode.G + GetDistance(lowestFNode, neighbour);
                    if (newCostToNeighbour < neighbour.G || !openStack.Contains(neighbour))
                    {
                        neighbour.G        = newCostToNeighbour;
                        neighbour.H        = GetDistance(neighbour, endNode);
                        neighbour.CameFrom = lowestFNode;

                        if (!openStack.Contains(neighbour))
                        {
                            openStack.Add(neighbour);
                        }
                    }
                }
            }

            return null;
        }


        private void OnDrawGizmos()
        {
            if (!Path.Any()) return;
            foreach (var node in Path)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(node.WorldTilePos, 0.5f);
            }
        }

        int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }

        public IEnumerable<Node> ReconstructThePath(Node start, Node end)
        {
            var currentNode = end;

            while (currentNode != start)
            {
                yield return currentNode;
                currentNode = currentNode.CameFrom;
            }
        }
    }
}