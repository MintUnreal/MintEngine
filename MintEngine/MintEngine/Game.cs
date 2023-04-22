using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MintEngine.Component;
using MintEngine.Component.BuiltInComponents;
using MintEngine.Editor;
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
        public static GameWindow game;
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

        private List<GameObject> gameObjects = new List<GameObject>();
        private Camera camera = new Camera();

        VAO<float> vao;
        Shader shader;
        Texture texture1;
        Texture texture2;

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
            vao = new VAO<float>();
            vao.AddVertexBufferObject(vertices);
            vao.AddIndices(indices);


            Material mat = new Material(shader);
            mat.Texture0 = texture1;
            mat.Texture1 = texture2;

            MeshRenderer renderer = new MeshRenderer(vao, mat);
            MeshRenderer renderer2 = new MeshRenderer(vao, mat);
            MeshRenderer renderer3 = new MeshRenderer(vao, mat);

            GameObject obj1 = new GameObject();
            obj1.AttachComponent(renderer);
            obj1.transform.position = new Vector3(2, 0, 0);
            gameObjects.Add(obj1);

            GameObject obj2 = new GameObject();
            obj2.AttachComponent(renderer2);
            obj2.transform.position = new Vector3(0, 0, 0);
            gameObjects.Add(obj2);

            GameObject obj3 = new GameObject();
            obj3.AttachComponent(renderer3);
            obj3.transform.position = new Vector3(-2, 0, 0);
            gameObjects.Add(obj3);

            GameObject player = new GameObject();
            camera.GenProjection(70, 0.01f, 100f);
            player.AttachComponent(camera);
            gameObjects.Add(player);

            //спиздил с юнити




        }

        public void Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, game.Width, game.Height);
            camera.GenProjection(70, 0.01f, 100f);
        }

        private Stopwatch stopwatch;
        public void Render(object sender, FrameEventArgs e)
        {
            deltaTime = (float)e.Time;

            GL.ClearColor(Color4.CornflowerBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            foreach(GameObject i in gameObjects)
            {
                i.Update();
            }

            gameObjects[0].transform.localRotation = new Vector3(stopwatch.ElapsedMilliseconds * 0.02f, stopwatch.ElapsedMilliseconds*0.02f, stopwatch.ElapsedMilliseconds * 0.02f);
            gameObjects[1].transform.localRotation = new Vector3(stopwatch.ElapsedMilliseconds * 0.02f, stopwatch.ElapsedMilliseconds*0.02f, stopwatch.ElapsedMilliseconds * 0.02f);
            gameObjects[2].transform.localRotation = new Vector3(stopwatch.ElapsedMilliseconds * 0.02f, stopwatch.ElapsedMilliseconds*0.02f, stopwatch.ElapsedMilliseconds * 0.02f);

            game.SwapBuffers();
        }
        public static float deltaTime { get; private set; }
    }
}
