using Microsoft.Xna.Framework;

namespace SeparatingAxis.Collisions
{
    public class CollisionResponse
    {
        public static CollisionResponse None => new CollisionResponse(Vector3.Zero, 0, false);

        public bool HasOccurred { get; }
        public Vector3 Inverse { get; }
        public Vector3 Result { get; }
        public float Depth { get; }

        public CollisionResponse(Vector3 translationAxis, float minIntervalDistance) : this(translationAxis, minIntervalDistance, true)
        {
        }

        private CollisionResponse(Vector3 translationAxis, float minIntervalDistance, bool hasOccured)
        {
            Result = Vector3.Normalize(translationAxis) * minIntervalDistance;
            Inverse = Vector3.Normalize(translationAxis) * -minIntervalDistance;
            Depth = minIntervalDistance;
            HasOccurred = hasOccured;
        }

        public override string ToString()
        {
            return $"{HasOccurred} - {Result} ({Depth})";
        }
    }
}
