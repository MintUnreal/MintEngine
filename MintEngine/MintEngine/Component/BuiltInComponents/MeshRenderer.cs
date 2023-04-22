using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MintEngine.Rendering;
using MintEngine.Rendering.Bridges;

namespace MintEngine.Component.BuiltInComponents
{
    public class MeshRenderer : MonoBehavior
    {
        public MeshRenderer(VAO<float> vao, Material material)
        {
            MainMaterial = material;
            vertexArrayObject = vao;
        }

        public void LoadVAO(VAO<float> newVAO)
        {
            vertexArrayObject = newVAO;
        }
        private VAO<float> vertexArrayObject;
        public Material MainMaterial { get; private set; }

        public override void Update()
        {
            MainMaterial.PreDraw(gameObject.transform);
            vertexArrayObject.Draw(MainMaterial.Shader);
        }



    }
}
