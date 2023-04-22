using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MintEngine.Component.BuiltInComponents;
using MintEngine.Editor;
using OpenTK.Graphics.OpenGL;

namespace MintEngine.Rendering
{
    public class Material
    {
        public Texture Texture0;
        public Texture Texture1;
        public Material(Shader shader)
        {
            if (shader == null) throw new nullShaderExeption();
            Shader = shader;
        }
        public Shader Shader;

        public void PreDraw(Transform transform)
        {
            Texture0.Use(TextureUnit.Texture0);
            Shader.SetInt("texture1", 0);
            Texture1.Use(TextureUnit.Texture1);
            Shader.SetInt("texture2", 1);
            Shader.SetMat4("model", transform.GetTransformMatrix());
            Shader.SetMat4("view", Camera.MainCamera.ViewMatrix);
            Shader.SetMat4("projection", Camera.MainCamera.ProjectionMatrix);
        }

    }


}
