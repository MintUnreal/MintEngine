using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace MintEngine.Rendering.Bridges
{
    public class VAO <T> where T : struct
    {
        public VAO()
        {
            array = GL.GenVertexArray();
            Bind();
            buffers = new List<int>();
        }

        private int array; //сам VAO
        private List<int> buffers; //список VBO
        private int indices; //индексы (EBO)
        private int indicesCount; //количество индексов

        private int attributeIndex; //считает атрибут чтобы использовать несколько атрибутов

        /// <summary>
        /// биндим VAO
        /// </summary>
        public void Bind()
        {
            GL.BindVertexArray(array);
        }

        /// <summary>
        /// добавляем VBO в наш VAO
        /// </summary>
        /// <param name="data"> cписок из вершин </param> 
        public void AddVertexBufferObject(T[] data)
        {
            int buffer;
            buffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(data.Length * Marshal.SizeOf<T>()), data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attributeIndex, 3, VertexAttribPointerType.Float, false, 5 * Marshal.SizeOf<T>(), 0);
            GL.VertexAttribPointer(attributeIndex + 1, 2, VertexAttribPointerType.Float, false, 5 * Marshal.SizeOf<T>(), 3 * Marshal.SizeOf<T>());
            GL.EnableVertexAttribArray(attributeIndex);
            GL.EnableVertexAttribArray(attributeIndex + 1);
            attributeIndex += 2;
            buffers.Add(buffer);
        }

        /// <summary>
        /// Включаем указатель атрибутов чтобы шейдер видел данные
        /// </summary>
        /// <param name="size">количество вершин которое отрисуем</param>
        public void Draw(Shader shader)
        {
            if (indices == 0) throw new nullIndicesVAO();;
            Bind();
            for(int i = 0; i < attributeIndex; i+=2)
            {
                GL.EnableVertexAttribArray(i);
                GL.EnableVertexAttribArray(i+1);
            }
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indices);
            shader.Use();
            //GL.DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }

        /// <summary>
        /// добавить индексы
        /// </summary>
        public void AddIndices(int[] _indices)
        {
            Bind();
            indices = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indices);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(int), _indices, BufferUsageHint.StaticDraw);
            indicesCount = _indices.Length;
        }
    }

    public class nullIndicesVAO : Exception { }
}
