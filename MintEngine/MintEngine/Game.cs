using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MintEngine.Rendering;
using MintEngine.Rendering.Bridges;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace MintEngine
{
    public class Game
    {
        private GameWindow game;
        public Game(string title)
        {
            game = new GameWindow();
            game.Title = title;

            game.Load += Load;
            game.Resize += Resize;
            game.RenderFrame += Render;
        }

        public void Run()
        {
            game.Run(60);
        }

        VAO<float> vao;
        Shader shader;
        Texture texture1;
        Texture texture2;

        //camera
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
        public void Load(object sender, EventArgs e)
        {
            game.CursorVisible = false;
            game.CursorGrabbed = true;
            game.VSync = VSyncMode.On;
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            stopwatch = new Stopwatch();
            stopwatch.Start();
            float[] vertices = {
                -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
                 0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
                 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
                -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

                -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                 0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
                 0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
                 0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
                -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
                -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

                -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
                -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

                 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
                 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                 0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                 0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                 0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

                -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                 0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
                 0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
                 0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
                -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

                -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
                 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
                 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
                -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
                -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
            };
            int[] indices =
            {
                0, 1, 2,   // first triangle
                2, 0, 3   // second triangle
            };
            shader = new Shader(Shader.defaultVertexShader, Shader.defaultFragmentShader);
            texture1 = new Texture(@"textures/texture.png");
            texture2 = new Texture(@"textures/texture2.png");
            shader.SetInt("texture1", 0);
            shader.SetInt("texture2", 1);
            vao = new VAO<float>(shader);
            vao.AddVertexBufferObject(vertices);
            vao.AddIndices(indices);



        }

        Matrix4 trans = new Matrix4();

        public void Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, game.Width, game.Height);
        }

        private Stopwatch stopwatch;
        public void Render(object sender, FrameEventArgs e)
        {
            cameraDirection = Vector3.Normalize(Position - cameraTarget);
            cameraRight = Vector3.Normalize(Vector3.Cross(up, cameraDirection));
            cameraUp = Vector3.Cross(cameraDirection, cameraRight);
            CameraInput(e);

            Matrix4 rotX = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(stopwatch.ElapsedMilliseconds*0.01f));
            Matrix4 rotY = Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(stopwatch.ElapsedMilliseconds*0.01f));
            Matrix4 rotZ = Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(stopwatch.ElapsedMilliseconds*0.01f));
            Matrix4 model = rotX * rotY * rotZ;
            Matrix4 view = Matrix4.LookAt(Position, Position + vfront, vup);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60.0f),(float) game.Width/game.Height, 0.1f, 100.0f);

            GL.ClearColor(Color4.CornflowerBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            texture1.Use(TextureUnit.Texture0);
            shader.SetInt("texture1", 0);
            texture2.Use(TextureUnit.Texture1);
            shader.SetInt("texture2", 1);
            shader.SetMat4("model", model);
            shader.SetMat4("view", view);
            shader.SetMat4("projection", projection);
            vao.Draw();
            

            game.SwapBuffers();
        }

        //mouse
        Vector2 lastPos;
        float yaw = 0;
        float pitch = 0;
        bool firstMove = true;
        private void CameraInput(FrameEventArgs e)
        {
            MouseState mouse = Mouse.GetState();
            if (firstMove) // this bool variable is initially set to true
            {
                lastPos = new Vector2(mouse.X, mouse.Y);
                firstMove = false;
            }
            if (!game.Focused) return;


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
                Position += vfront * speed * (float)e.Time; //Forward 
            }

            if (input.IsKeyDown(Key.S))
            {
                Position -= vfront * speed * (float)e.Time; //Backwards
            }

            if (input.IsKeyDown(Key.A))
            {
                Position -= Vector3.Normalize(Vector3.Cross(vfront, vup)) * speed * (float)e.Time; //Left
            }

            if (input.IsKeyDown(Key.D))
            {
                Position += Vector3.Normalize(Vector3.Cross(vfront, vup)) * speed * (float)e.Time; //Right
            }

            if (input.IsKeyDown(Key.Space))
            {
                Position += up * speed * (float)e.Time; //Up 
            }

            if (input.IsKeyDown(Key.LShift))
            {
                Position -= up * speed * (float)e.Time; //Down
            }
        }
    }
}
