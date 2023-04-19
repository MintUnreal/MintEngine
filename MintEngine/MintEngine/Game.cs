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

        private Stopwatch stopWatch;
        public void Load(object sender, EventArgs e)
        {
            stopWatch = new Stopwatch();
            stopWatch.Start();
            game.VSync = VSyncMode.On;
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            float[] vertices = {
                //Position          Texture coordinates
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
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

        private int fps = 0;
        private float angle = 0;
        public void Render(object sender, EventArgs e)
        {
            if(stopWatch.ElapsedMilliseconds > 10)
            {
                angle++;
                Matrix4 rotation = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(angle)) * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(angle)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(angle));
                Matrix4 scale = Matrix4.CreateScale(0.5f, 0.5f, 0.5f);
                trans = rotation * scale;
            }
            if (stopWatch.ElapsedMilliseconds > 1000)
            {
                Console.WriteLine("FPS: "+fps);
                stopWatch.Restart();
                fps = 0;
            }
            else fps++;

            GL.ClearColor(Color4.CornflowerBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            texture1.Use(TextureUnit.Texture0);
            shader.SetInt("texture1", 0);
            texture2.Use(TextureUnit.Texture1);
            shader.SetInt("texture2", 1);
            shader.SetMat4("transform", trans);
            vao.Draw();
            

            game.SwapBuffers();
        }
    }
}
