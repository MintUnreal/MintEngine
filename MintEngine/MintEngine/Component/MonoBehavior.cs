using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MintEngine.Component.BuiltInComponents;

namespace MintEngine.Component
{
    public class MonoBehavior
    {
        public GameObject gameObject { get; private set; }
        public void SetGameObject(GameObject obj)
        {
            gameObject = obj;
        }
        public virtual void Update() { }
        public virtual void Start() { }
    }

}
