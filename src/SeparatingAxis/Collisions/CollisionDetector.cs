using System;
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
            var edgeCountA = polygonA.Edges.Count;
            var edgeCountB = polygonB.Edges.Count;
            var minIntervalDistance = float.PositiveInfinity;
            var translationAxis = new Vector2();

            for (var edgeIndex = 0; edgeIndex < edgeCountA + edgeCountB; edgeIndex++)
            {
                var edge = edgeIndex < edgeCountA ? polygonA.Edges[edgeIndex] : polygonB.Edges[edgeIndex - edgeCountA];

                var axis = Vector2.Normalize(new Vector2(-edge.Y, edge.X));

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
                if (distance.DotProduct(translationAxis) < 0)
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

        private static void ProjectPolygon(Vector2 axis, Polyhedron polygon, ref float min, ref float max)
        {
            var dotProduct = axis.DotProduct(polygon.Vertices[0].Position);
            min = dotProduct;
            max = dotProduct;
            foreach (var point in polygon.Vertices)
            {
                dotProduct = point.Position.DotProduct(axis);
                if (dotProduct < min)
                    min = dotProduct;
                else if (dotProduct > max)
                    max = dotProduct;
            }
        }
    }
}
