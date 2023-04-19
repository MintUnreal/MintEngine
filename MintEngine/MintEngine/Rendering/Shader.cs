using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MintEngine.Rendering
{
    public class Shader
    {
        public int Handle;

        private int VertexShader;
        private int FragmentShader;

        public Shader(string vertex, string fragment)
        {
            //создаем шейдеры
            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, vertex);

            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, fragment);

            //компилируем шейдеры
            GL.CompileShader(VertexShader);

            //выводим ошибки компиляции
            GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int successVertex);
            if (successVertex == 0)
            {
                string infoLog = GL.GetShaderInfoLog(VertexShader);
                Console.WriteLine(infoLog);
            }

            GL.CompileShader(FragmentShader);

            GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out int successFragment);
            if (successFragment == 0)
            {
                string infoLog = GL.GetShaderInfoLog(FragmentShader);
                Console.WriteLine(infoLog);
            }

            //объединяем шейдервы в программу
            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            GL.LinkProgram(Handle);

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                Console.WriteLine(infoLog);
            }

            //очистка шейдеров
            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
            
        }

        /// <summary>
        /// Добавляет униформу в шейдер
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetInt(string name, int value)
        {
            int location = GL.GetUniformLocation(Handle, name);

            GL.Uniform1(location, value);
        }
        public void SetMat4(string name, Matrix4 matrix)
        {
            int location = GL.GetUniformLocation(Handle, name);

            GL.UniformMatrix4(location,true,ref matrix);
        }

        /// <summary>
        /// Применить этот шейдер
        /// </summary>
        public void Use()
        {
            GL.UseProgram(Handle);
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }
        ~Shader()
        {
            if (disposedValue == false)
            {
                Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            }
        }

        /// <summary>
        /// утилизировать шейдер
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public const string defaultVertexShader = @"
            #version 330 core
            layout (location = 0) in vec3 aPos;
            layout (location = 1) in vec2 aTexCoord;

            out vec2 texCoord;
  
            uniform mat4 transform;

            void main()
            {
                gl_Position = vec4(aPos, 1.0f) * transform;
                texCoord = vec2(aTexCoord.x, aTexCoord.y);
            }
            ";
        public const string defaultFragmentShader = @"
            #version 330

            out vec4 outputColor;

            in vec2 texCoord;

            uniform sampler2D texture1;
            uniform sampler2D texture2;

            void main()
            {
                outputColor = mix(texture(texture1, texCoord), texture(texture2, texCoord), 0.2);
            }
            ";
    }
}
