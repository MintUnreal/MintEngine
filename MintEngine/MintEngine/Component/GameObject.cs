using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MintEngine.Component.BuiltInComponents;

namespace MintEngine.Component
{
    public class GameObject
    {
        public GameObject()
        {
            components = new List<MonoBehavior>();
            AttachComponent(new Transform());
            transform = GetComponent<Transform>();
        }
        
        private List<MonoBehavior> components;
        public Transform transform;

        /// <summary>
        /// Добавить компонент на обьект
        /// </summary>
        /// <param name="behaviour"></param>
        public void AttachComponent(MonoBehavior behaviour)
        {
            behaviour.SetGameObject(this);
            components.Add(behaviour);
        }

        /// <summary>
        /// Получить определенный компонент (возращает null если его нет)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : MonoBehavior
        {
            foreach(MonoBehavior i in components)
            {
                if (i is T) return i as T;
            }
            return null;
        }

        public void Update()
        {
            foreach(MonoBehavior i in components)
            {
                i.Update();
            }
        }
    }

    /// <summary>
    /// Ошибка если GameObject = null
    /// </summary>
    public class nullGameObjectException : Exception { }
}
