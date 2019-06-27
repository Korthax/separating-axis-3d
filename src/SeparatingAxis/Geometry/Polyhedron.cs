using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SeparatingAxis.Input;

namespace SeparatingAxis.Geometry
{
    public class Polyhedron
    {
        private const float MoveSpeed = 5f;

        public VertexPositionColor[] Vertices { get; }
        public Vector3 Position { get; private set; }
        public List<Vector3> Edges { get; }
        public bool Active { get; set; }

        public static Polyhedron From(Vector3 position, params VertexPositionColor[] vertices)
        {
            var edges = new List<Vector3>();
            for (var i = 0; i < vertices.Length; i++)
            {
                var p1 = vertices[i];
                var p2 = i + 1 >= vertices.Length ? vertices[0] : vertices[i + 1];
                edges.Add(new Vector3(p2.Position.X - p1.Position.X, p2.Position.Y - p1.Position.Y, p2.Position.Z - p1.Position.Z));
            }

            return new Polyhedron(position, vertices, edges);
        }

        private Polyhedron(Vector3 position, VertexPositionColor[] vertices, List<Vector3> edges)
        {
            Position = position;
            Vertices = vertices;
            Edges = edges;
        }

        public void Render(GraphicsDevice graphicsDevice, BasicEffect effect)
        {
            effect.World = Matrix.CreateScale(1.0f)
                  * Matrix.CreateRotationX(MathHelper.ToRadians(0))
                  * Matrix.CreateRotationY(MathHelper.ToRadians(0))
                  * Matrix.CreateTranslation(Position);

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply ();
                graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vertices, 0, Vertices.Length / 3);
            }
        }

        public void Update(GameTime gameTime, InputDetector inputDetector)
        {
            if (Active)
            {
                var speed = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                var moveVector = Vector3.Zero;

                if (inputDetector.IsKeyDown(Keys.Down))
                    moveVector.Z = -speed;

                if (inputDetector.IsKeyDown(Keys.Up))
                    moveVector.Z = speed;

                if (inputDetector.IsKeyDown(Keys.Left))
                    moveVector.X = speed;

                if (inputDetector.IsKeyDown(Keys.Right))
                    moveVector.X = -speed;

                Position += moveVector;
            }
        }
    }
}
