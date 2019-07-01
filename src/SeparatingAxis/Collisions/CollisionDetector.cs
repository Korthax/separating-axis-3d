using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SeparatingAxis.Geometry;

namespace SeparatingAxis.Collisions
{
    public static class CollisionDetector
    {
        public static CollisionResponse CheckForCollisions(Polyhedron polyhedronA, Polyhedron polyhedronB)
        {
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
            var minIntervalDistance = float.PositiveInfinity;
            var translationAxis = Vector3.Zero;

            var collisionAxes = new List<Vector3>();
            collisionAxes.AddRange(polygonA.Axes);
            collisionAxes.AddRange(polygonB.Axes);

            foreach (var polyAAxes in polygonA.Axes)
            {
                foreach (var polyBAxes in polygonB.Axes)
                    collisionAxes.Add(Vector3.Cross(polyAAxes, polyBAxes));
            }

            foreach (var c in collisionAxes)
            {
                float minA = 0;
                float minB = 0;
                float maxA = 0;
                float maxB = 0;

                var axis = Vector3.Negate(c);

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
            var dotProduct = Vector3.Dot(polygon.Vertices[0].Position + polygon.Position, axis);
            min = dotProduct;
            max = dotProduct;
            foreach (var point in polygon.Vertices)
            {
                dotProduct = Vector3.Dot(point.Position + polygon.Position, axis);
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
