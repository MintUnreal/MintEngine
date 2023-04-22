using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MintEngine.Component;
using OpenTK;
using OpenTK.Input;

namespace MintEngine.Editor
{
    public class Camera : MonoBehavior
    {

        public Camera()
        {
            if (MainCamera == null) MainCamera = this;
        }
        /// <summary>
        /// Включенная камера
        /// </summary>
        public static Camera MainCamera;

        Vector3 Position = new Vector3(0.0f, 0.0f, 3.0f);
        Vector3 cameraTarget = Vector3.Zero;
        Vector3 cameraDirection;
        Vector3 up = Vector3.UnitY;
        Vector3 cameraRight;
        Vector3 cameraUp;

        float speed = 2f;
        float sensitivity = 0.1f;

        Vector3 vfront = new Vector3(0.0f, 0.0f, -1.0f);
        Vector3 vup = new Vector3(0.0f, 1.0f, 0.0f);

        public override void Update()
        {
            CameraInput();

        }
        //mouse
        Vector2 lastPos;
        float yaw = 0;
        float pitch = 0;
        bool firstMove = true;

        private void CameraInput()
        {
            cameraDirection = Vector3.Normalize(Position - cameraTarget);
            cameraRight = Vector3.Normalize(Vector3.Cross(up, cameraDirection));
            cameraUp = Vector3.Cross(cameraDirection, cameraRight);
            ViewMatrix = Matrix4.LookAt(Position, Position + vfront, vup);

            MouseState mouse = Mouse.GetState();
            if (firstMove) // this bool variable is initially set to true
            {
                lastPos = new Vector2(mouse.X, mouse.Y);
                firstMove = false;
            }
            if (!Game.game.Focused) return;


            float deltaX = mouse.X - lastPos.X;
            float deltaY = mouse.Y - lastPos.Y;
            lastPos = new Vector2(mouse.X, mouse.Y);

            yaw += deltaX * sensitivity;

            if (pitch > 89.0f)
            {
                pitch = 89.0f;
            }
            else if (pitch < -89.0f)
            {
                pitch = -89.0f;
            }
            else
            {
                pitch -= deltaY * sensitivity;
            }
            vfront.X = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(yaw));
            vfront.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
            vfront.Z = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(yaw));
            vfront = Vector3.Normalize(vfront);

            KeyboardState input = Keyboard.GetState();

            //...

            if (input.IsKeyDown(Key.W))
            {
                Position += vfront * speed * Game.deltaTime; //Forward 
            }

            if (input.IsKeyDown(Key.S))
            {
                Position -= vfront * speed * Game.deltaTime; //Backwards
            }

            if (input.IsKeyDown(Key.A))
            {
                Position -= Vector3.Normalize(Vector3.Cross(vfront, vup)) * speed * Game.deltaTime; //Left
            }

            if (input.IsKeyDown(Key.D))
            {
                Position += Vector3.Normalize(Vector3.Cross(vfront, vup)) * speed * Game.deltaTime; //Right
            }

            if (input.IsKeyDown(Key.Space))
            {
                Position += up * speed * Game.deltaTime; //Up 
            }

            if (input.IsKeyDown(Key.LShift))
            {
                Position -= up * speed * Game.deltaTime; //Down
            }
        }
        public Matrix4 ViewMatrix { get; private set; }
        public void GenProjection(float fov, float minDepth, float maxDepth)
        {
            float _fov = 60;
            if (fov < 180) _fov = fov;
            else Console.WriteLine("Вы не можете выставить угол обзора выше 180 градусов!");
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(_fov), (float)Game.game.Width / Game.game.Height, minDepth, maxDepth);
        }
        public Matrix4 ProjectionMatrix { get; private set; }
    }
}
