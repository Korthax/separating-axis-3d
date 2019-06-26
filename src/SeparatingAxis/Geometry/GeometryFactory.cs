using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SeparatingAxis.Geometry
{
    public class GeometryFactory
    {
        public static Polyhedron CreateCube(Vector3 position, Vector3 size, Color colour)
        {
            // Calculate the position of the vertices on the top face.
            var topLeftFront = new Vector3(0, 1, 0) * size;
            var topLeftBack = new Vector3(0, 1, 1) * size;
            var topRightFront = new Vector3(1, 1, 0) * size;
            var topRightBack = new Vector3(1, 1, 1) * size;

            // Calculate the position of the vertices on the bottom face.
            var btmLeftFront = new Vector3(0, 0, 0) * size;
            var btmLeftBack = new Vector3(0, 0, 1) * size;
            var btmRightFront = new Vector3(1, 0, 0) * size;
            var btmRightBack = new Vector3(1, 0, 1) * size;

            var vertices = new[]
            {
                // Add the vertices for the FRONT face.
                new VertexPositionColor(topLeftFront, colour),
                new VertexPositionColor(btmLeftFront, colour),
                new VertexPositionColor(topRightFront, colour),
                new VertexPositionColor(btmLeftFront, colour),
                new VertexPositionColor(btmRightFront, colour),
                new VertexPositionColor(topRightFront, colour),

                // Add the vertices for the BACK face.
                new VertexPositionColor(topLeftBack, colour),
                new VertexPositionColor(topRightBack, colour),
                new VertexPositionColor(btmLeftBack, colour),
                new VertexPositionColor(btmLeftBack, colour),
                new VertexPositionColor(topRightBack, colour),
                new VertexPositionColor(btmRightBack, colour),

                // Add the vertices for the TOP face.
                new VertexPositionColor(topLeftFront, colour),
                new VertexPositionColor(topRightBack, colour),
                new VertexPositionColor(topLeftBack, colour),
                new VertexPositionColor(topLeftFront, colour),
                new VertexPositionColor(topRightFront, colour),
                new VertexPositionColor(topRightBack, colour),

                // Add the vertices for the BOTTOM face.
                new VertexPositionColor(btmLeftFront, colour),
                new VertexPositionColor(btmLeftBack, colour),
                new VertexPositionColor(btmRightBack, colour),
                new VertexPositionColor(btmLeftFront, colour),
                new VertexPositionColor(btmRightBack, colour),
                new VertexPositionColor(btmRightFront, colour),

                // Add the vertices for the LEFT face.
                new VertexPositionColor(topLeftFront, colour),
                new VertexPositionColor(btmLeftBack, colour),
                new VertexPositionColor(btmLeftFront, colour),
                new VertexPositionColor(topLeftBack, colour),
                new VertexPositionColor(btmLeftBack, colour),
                new VertexPositionColor(topLeftFront, colour),

                // Add the vertices for the RIGHT face.
                new VertexPositionColor(topRightFront, colour),
                new VertexPositionColor(btmRightFront, colour),
                new VertexPositionColor(btmRightBack, colour),
                new VertexPositionColor(topRightBack, colour),
                new VertexPositionColor(topRightFront, colour),
                new VertexPositionColor(btmRightBack, colour)
            };

            return new Polyhedron(position, vertices);
        }
    }
}
