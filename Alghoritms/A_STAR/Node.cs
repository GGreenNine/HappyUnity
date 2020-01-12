using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HappyUnity.Alghoritms
{
    public class Node : IEqualityComparer<Node>, IComparable<Node>
    {
        public int G;
        public int H;
        public int FCost => G + H;

        public Vector3    WorldTilePos;
        public Vector3Int LocalTilePos;

        public int gridX;
        public int gridY;

        public bool IsObstacle;
        public Node CameFrom;


        public Node(Vector3 worldTilePos, Vector3Int localTilePos, int gridX, int gridY, bool isObstacle)
        {
            WorldTilePos = worldTilePos;
            LocalTilePos = localTilePos;
            this.gridX   = gridX;
            this.gridY   = gridY;
            IsObstacle   = isObstacle;
        }

        public bool Equals(Node x, Node y)
        {
            return y != null && (x != null && (x.G == y.G && x.H == y.H && x.FCost == y.FCost));
        }

        public int GetHashCode(Node obj)
        {
            return obj.WorldTilePos.GetHashCode();
        }

        public int CompareTo(Node other)
        {
            if (other.FCost < FCost)
                return 1;
            if (other.FCost > FCost)
                return -11;
            return 0;
        }
    }
}