using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Xna.Framework;
using SeparatingAxis.Geometry;

namespace SeparatingAxis.Collisions
{
    public static class CollisionDetector
    {
        public static CollisionResponse CheckForCollisions(Polyhedron polyhedronA, Polyhedron polyhedronB)
        {
            foreach (var VARIABLE in polyhedronA.FaceNormals)
            {

            }

            if (polyhedronA.Equals(polyhedronB))
                return CollisionResponse.None;

            var collision = CollisionResponse.None;
            var result = CheckForCollision(polyhedronA, polyhedronB);
            if (result.HasOccurred && result.Depth > collision.Depth)
                collision = result;

            return collision;
        }

        private static CollisionResponse CheckForCollision(Polyhedron polygonA, Polyhedron polygonB)
        {
            var edgeCountA = polygonA.Edges.Count;
            var edgeCountB = polygonB.Edges.Count;
            var minIntervalDistance = float.PositiveInfinity;
            var translationAxis = Vector3.Zero;

            var cAxis = new List<Vector3>();
            cAxis.AddRange(polygonA.FaceNormals);
            cAxis.AddRange(polygonB.FaceNormals);
            foreach (var aEdge in polygonA.Edges)
            {
                foreach (var bEdge in polygonB.Edges)
                    cAxis.Add(Vector3.Cross(aEdge.Direction, bEdge.Direction));
            }

            foreach (var edge in polygonA.Edges)
            {
                var axis = Vector3.Cross(edge.Direction,  edge.Direction);

                float minA = 0;
                float minB = 0;
                float maxA = 0;
                float maxB = 0;

                ProjectPolygon(axis, polygonA, ref minA, ref maxA);
                ProjectPolygon(axis, polygonB, ref minB, ref maxB);

                var intervalDistance = IntervalDistance(minA, maxA, minB, maxB);
                if (intervalDistance >= 0)
                    return CollisionResponse.None;

                intervalDistance = Math.Abs(intervalDistance);
                if (intervalDistance >= minIntervalDistance)
                    continue;

                minIntervalDistance = intervalDistance;
                translationAxis = axis;

                var distance = polygonA.Center - polygonB.Center;
                if (Vector3.Dot(distance, translationAxis) < 0)
                    translationAxis = -translationAxis;
            }

            return new CollisionResponse(translationAxis, minIntervalDistance);
        }

        private static float IntervalDistance(float minA, float maxA, float minB, float maxB)
        {
            if (minA < minB)
                return minB - maxA;
            return minA - maxB;
        }

        private static void ProjectPolygon(Vector3 axis, Polyhedron polygon, ref float min, ref float max)
        {
            var dotProduct = Vector3.Dot(axis, polygon.Vertices[0].Position);
            min = dotProduct;
            max = dotProduct;
            foreach (var point in polygon.Vertices)
            {
                dotProduct = Vector3.Dot(point.Position, axis);
                if (dotProduct < min)
                    min = dotProduct;
                else if (dotProduct > max)
                    max = dotProduct;
            }
        }

        public static void T()
        {

        }
    }
}
