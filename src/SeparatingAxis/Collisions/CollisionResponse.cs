using Microsoft.Xna.Framework;

namespace SeparatingAxis.Collisions
{
    public class CollisionResponse
    {
        public static CollisionResponse None => new CollisionResponse(Vector3.Zero, 0);
        public bool HasOccurred { get; set; }
        public float Depth { get; set; }

        public CollisionResponse(Vector3 translationAxis, float minIntervalDistance)
        {

        }
    }
}
