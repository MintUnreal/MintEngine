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
        Texture texture;
        public void Load(object sender, EventArgs e)
        {
            game.VSync = VSyncMode.On;

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
            texture = new Texture(@"textures/texture.png");
            vao = new VAO<float>(shader);
            vao.AddVertexBufferObject(vertices);
            vao.AddIndices(indices);

            texture.Bind();
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            float[] borderColor = { 1.0f, 1.0f, 0.0f, 1.0f };
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, borderColor);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);

        }

        public void Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, game.Width, game.Height);
        }

        public void Render(object sender, EventArgs e)
        {
            GL.ClearColor(Color4.CornflowerBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            texture.Use();
            texture.Bind();
            vao.Draw();
            

            game.SwapBuffers();
        }
    }
}
