using System;
using System.Collections.Generic;
using HappyUnity.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HappyUnity.TransformUtils
{
    public class GameField : MonoBehaviour
    {
        public static List<Vector2> Points = new List<Vector2>();
        public int radius = 3;

        public void Generate_GameField()
        {
            Viewport.GenerateWorld(Vector3.zero);
            Points = Generate_RandomPoints();
        }

        /// <summary>
        /// Getting random position in the game field
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetRandomWorldPositionXY(Transform transform)
        {
            if (Points.Count <= 0)
            {
                Points = Generate_RandomPoints();
                Debug.Log("Field is fulled");
            }

            bool IsValid = false;
            Vector2 randomPlace;

            LayerMask shipLayer = LayerMask.GetMask("Ship");

            do
            {
                randomPlace = Points[Random.Range(0, Points.Count - 1)];
                IsValid = !CheckValidPosition(transform, randomPlace, shipLayer);
            } while (!IsValid);

            return randomPlace;
        }

        static bool CheckValidPosition(Transform transform, Vector2 positionVariant, int layerMask = ~0)
        {
            float x = transform.localScale.x;
            float y = transform.localScale.y;
            float collisionSphereRadius = x > y ? x : y;

            var soverlap = Physics2D.OverlapCircle(positionVariant, collisionSphereRadius, layerMask);
            return soverlap && soverlap.CompareTag("Ship");
        }

        void OnDrawGizmos()
        {
            var regionSize = new Vector2(Camera.main.transform.position.x * 2, Camera.main.transform.position.y * 2);
            Gizmos.DrawWireCube(regionSize / 2, regionSize);
            if (Points != null)
            {
                foreach (Vector2 point in Points)
                {
                    Gizmos.DrawSphere(point, radius);
                }
            }
        }

        static List<Vector2> Generate_RandomPoints(float radius = 1f, int rejectionSamples = 30)
        {
            var points = RandomCirclePositionGeneration.GeneratePoints(radius,
                new Vector2(Camera.main.transform.position.x * 2, Camera.main.transform.position.y * 2),
                rejectionSamples);
            return points;
        }
    }

    public static class Viewport
    {
        public static Vector2 World;

        public static void GenerateWorld(Vector3 viewPoint)
        {
            Vector2 world = Camera.main.ViewportToWorldPoint(viewPoint);
            World = world;
        }
    }
}