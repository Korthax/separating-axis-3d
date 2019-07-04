using System.Collections.Generic;
using System.Linq;
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
        public HashSet<Vector3> Axes { get; }
        public List<Edge> Edges { get; }
        public bool Active { get; set; }
        public Vector3 Center { get; }
        public Vector3 HalfSize => Size / 2;
        public Vector3 Size { get; }

        public static Polyhedron From(Vector3 position, params VertexPositionColor[] vertices)
        {
            var potentialEdges = new Dictionary<Edge, HashSet<Vector3>>();

            var centre = Vector3.Zero;

            var min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            var faceNormals = new HashSet<Vector3>();
            for (var i = 0; i < vertices.Length; i += 3)
            {
                var a = vertices[i];
                var b = vertices[i + 1];
                var c = vertices[i + 2];

                SetMinMax(a.Position, ref min, ref max);
                SetMinMax(b.Position, ref min, ref max);
                SetMinMax(c.Position, ref min, ref max);

                var u = b.Position - a.Position;
                var v = c.Position - a.Position;

                centre += a.Position;
                centre += b.Position;
                centre += c.Position;

                var normal = Vector3.Normalize(Vector3.Cross(v, u));

                AddFaceNormal(faceNormals, normal);

                var edge1 = Edge.From(a.Position, b.Position);
                var edge2 = Edge.From(b.Position, c.Position);
                var edge3 = Edge.From(c.Position, a.Position);

                AddFaceNormalToEdges(potentialEdges, edge1, normal);
                AddFaceNormalToEdges(potentialEdges, edge2, normal);
                AddFaceNormalToEdges(potentialEdges, edge3, normal);
            }

            var edges = potentialEdges
                .Where(x => x.Value.Count > 1)
                .Select(x => x.Key)
                .ToList();

            return new Polyhedron(position, vertices, edges, faceNormals, centre / vertices.Length, max - min);
        }

        private static void SetMinMax(Vector3 a, ref Vector3 min, ref Vector3 max)
        {
            if (a.X < min.X)
                min.X = a.X;

            if (a.Y < min.Y)
                min.Y = a.Y;

            if (a.Z < min.Z)
                min.Z = a.Z;

            if (a.X > max.X)
                max.X = a.X;

            if (a.Y > max.Y)
                max.Y = a.Y;

            if (a.Z > max.Z)
                max.Z = a.Z;
        }

        private static void AddFaceNormalToEdges(IDictionary<Edge, HashSet<Vector3>> edges, Edge edge, Vector3 faceNormal)
        {
            if (edges.ContainsKey(edge))
                edges[edge].Add(faceNormal);
            else if (edges.ContainsKey(edge.Negate()))
                edges[edge.Negate()].Add(faceNormal);
            else
                edges.Add(edge, new HashSet<Vector3> { faceNormal });
        }


        private static void AddFaceNormal(HashSet<Vector3> edges, Vector3 faceNormal)
        {
            if (edges.Contains(faceNormal))
                return;

            if (edges.Contains(Vector3.Negate(faceNormal)))
                return;

            edges.Add(faceNormal);
        }

        private Polyhedron(Vector3 position, VertexPositionColor[] vertices, List<Edge> edges, HashSet<Vector3> axes,
            Vector3 center, Vector3 size)
        {
            Axes = axes;
            Center = center;
            Size = size;
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
                pass.Apply();
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

    public class Edge
    {
        public Vector3 Direction { get; }
        private Vector3 Start { get; }
        private Vector3 End { get; }

        public static Edge From(Vector3 start, Vector3 end)
        {
            var direction = end - start;
            return new Edge(start, end, direction);
        }

        private Edge(Vector3 start, Vector3 end, Vector3 direction)
        {
            Start = start;
            End = end;
            Direction = direction;
        }

        public Edge Negate()
        {
            return new Edge(End, Start, Vector3.Negate(Direction));
        }

        private bool Equals(Edge other)
        {
            return Direction == other.Direction && Start == other.Start && End == other.End;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return obj.GetType() == GetType() && Equals((Edge)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Start.GetHashCode();
                hashCode = (hashCode * 397) ^ End.GetHashCode();
                hashCode = (hashCode * 397) ^ Direction.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{{ start: {Start}, end: {End}, direction: {Direction} }}";
        }
    }
}
