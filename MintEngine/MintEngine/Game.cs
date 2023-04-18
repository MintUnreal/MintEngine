using System;
using System.Collections.Generic;
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
            game.Run();
        }

        VAO<float> vao;
        Shader shader;
        Texture texture1;
        Texture texture2;
        public void Load(object sender, EventArgs e)
        {
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

        public void Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, game.Width, game.Height);
        }

        public void Render(object sender, EventArgs e)
        {
            GL.ClearColor(Color4.CornflowerBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            texture1.Use(TextureUnit.Texture0);
            shader.SetInt("texture1", 0);
            texture2.Use(TextureUnit.Texture1);
            shader.SetInt("texture2", 1);
            vao.Draw();
            

            game.SwapBuffers();
        }
    }
}
