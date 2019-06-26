using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SeparatingAxis.Input;

namespace SeparatingAxis
{
    public class Camera
    {
        private const float TurnSpeed = 0.1f;
        private const float MoveSpeed = 5f;

        public Matrix ProjectionMatrix { get; }
        public Matrix ViewMatrix { get; private set; }

        private readonly GraphicsDevice _graphicsDevice;

        private Vector2 _mouseRotationBuffer;
        private Vector3 _cameraPosition;
        private Vector3 _cameraRotation;
        private Vector3 _target;

        public Camera(Vector3 position, GraphicsDevice graphicsDevice)
        {
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphicsDevice.Viewport.AspectRatio, 1f, 500f);
            ViewMatrix = Matrix.Identity;
            _graphicsDevice = graphicsDevice;
            _cameraPosition = position;
            _mouseRotationBuffer = Vector2.Zero;
            _target = Vector3.UnitZ;
        }

        public void Update(GameTime gameTime, InputDetector inputDetector)
        {
            var mousePosition = inputDetector.MousePosition();

            var elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var moveVector = Vector3.Zero;
            var turnAmount = TurnSpeed * elapsedSeconds;
            var moveAmount = MoveSpeed * elapsedSeconds;

            if (inputDetector.IsKeyDown(Keys.S))
                moveVector.Z = -moveAmount;

            if (inputDetector.IsKeyDown(Keys.W))
                moveVector.Z = moveAmount;

            if (inputDetector.IsKeyDown(Keys.A))
                moveVector.X = moveAmount;

            if (inputDetector.IsKeyDown(Keys.D))
                moveVector.X = -moveAmount;

            if (inputDetector.IsKeyDown(Keys.E))
                moveVector.Y = -moveAmount;

            if (inputDetector.IsKeyDown(Keys.Q))
                moveVector.Y = moveAmount;

            if (moveVector != Vector3.Zero)
                _cameraPosition += Vector3.Transform(moveVector, Matrix.CreateRotationY(_cameraRotation.Y));

            _mouseRotationBuffer.X -= turnAmount * (mousePosition.X - _graphicsDevice.Viewport.Width / 2f);
            _mouseRotationBuffer.Y -= turnAmount * (mousePosition.Y - _graphicsDevice.Viewport.Height / 2f);

            _mouseRotationBuffer.Y = MathHelper.Clamp(_mouseRotationBuffer.Y, MathHelper.ToRadians(-89), MathHelper.ToRadians(89));

            _cameraRotation = new Vector3(-_mouseRotationBuffer.Y, MathHelper.WrapAngle(_mouseRotationBuffer.X), 0);
            _target = Vector3.Transform(Vector3.UnitZ, Matrix.CreateRotationX(_cameraRotation.X) * Matrix.CreateRotationY(_cameraRotation.Y));

            ViewMatrix = Matrix.CreateLookAt(_cameraPosition, _cameraPosition + _target, Vector3.Up);

            inputDetector.SetMousePosition(_graphicsDevice.Viewport.Width / 2, _graphicsDevice.Viewport.Height / 2);
        }
    }
}
