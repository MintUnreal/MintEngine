using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;

namespace MintEngine.Rendering
{
    public class Texture
    {
        public int Handle { get; private set; }
        private string path;
        public Texture(string _path)
        {
            if (!File.Exists(_path)) Console.WriteLine(_path + " не существует!");
            Handle = GL.GenTexture();
            path = _path;
        }
        /// <summary>
        /// Выбрать эту текстуру для рендера
        /// </summary>
        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
        public void Use()
        {
            GL.BindTexture(TextureTarget.Texture2D, Handle);

            //Load the image
            Image<Rgba32> image = Image.Load<Rgba32>(path);

            //ImageSharp loads from the top-left pixel, whereas OpenGL loads from the bottom-left, causing the texture to be flipped vertically.
            //This will correct that, making the texture display properly.
            image.Mutate(x => x.Flip(FlipMode.Vertical));

            //Use the CopyPixelDataTo function from ImageSharp to copy all of the bytes from the image into an array that we can give to OpenGL.
            var pixels = new byte[4 * image.Width * image.Height];
            image.CopyPixelDataTo(pixels);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
            
        }
    }
}
