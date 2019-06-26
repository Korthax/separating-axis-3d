using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SeparatingAxis.Geometry;
using SeparatingAxis.Input;

namespace SeparatingAxis
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private InputDetector _inputDetector;
        private BasicEffect _basicEffect;
        private Polyhedron _cubeOne;
        private Polyhedron _cubeTwo;
        private Camera _camera;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Dispose(bool disposing)
        {
            _basicEffect.Dispose();
            base.Dispose(disposing);
        }

        protected override void Initialize()
        {
            _camera = new Camera(new Vector3(0, 0, -4), _graphics.GraphicsDevice);
            _inputDetector = new InputDetector();
            _basicEffect = new BasicEffect(_graphics.GraphicsDevice);

            _cubeOne = GeometryFactory.CreateCube(new Vector3(0, 0, 0), new Vector3(1, 1, 1), Color.Red);
            _cubeTwo = GeometryFactory.CreateCube(new Vector3(2, 0, 0), new Vector3(1, 1, 1), Color.Blue);

            _graphics.GraphicsDevice.RasterizerState = new RasterizerState
            {
                FillMode = FillMode.WireFrame,
                CullMode = CullMode.None
            };

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (_inputDetector.IsKeyDown(Keys.Escape))
                Exit();

            if (_inputDetector.IsKeyDown(Keys.D1))
            {
                _cubeOne.Active = true;
                _cubeTwo.Active = false;
            }
            else if (_inputDetector.IsKeyDown(Keys.D2))
            {
                _cubeOne.Active = false;
                _cubeTwo.Active = true;
            }

            _inputDetector.Update(gameTime);
            _camera.Update(gameTime, _inputDetector);
            _cubeOne.Update(gameTime, _inputDetector);
            _cubeTwo.Update(gameTime, _inputDetector);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _basicEffect.VertexColorEnabled = true;
            _basicEffect.LightingEnabled = false;

            _basicEffect.View = _camera.ViewMatrix;
            _basicEffect.Projection = _camera.ProjectionMatrix;

            _cubeOne.Render(GraphicsDevice, _basicEffect);
            _cubeTwo.Render(GraphicsDevice, _basicEffect);

            base.Draw(gameTime);
        }
    }

}
