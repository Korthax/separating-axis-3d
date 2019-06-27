namespace SeparatingAxis.Collisions
{
    public class CollisionResponse
    {
        public static CollisionResponse None => new CollisionResponse();
        public bool HasOccurred { get; set; }
        public float Depth { get; set; }

        public CollisionResponse()
        {

        }
    }
}
