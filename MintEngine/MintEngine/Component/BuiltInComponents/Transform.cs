using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace MintEngine.Component.BuiltInComponents
{
    public class Transform : MonoBehavior
    {
        private Vector3 _position;
        private Vector3 _localPosition;
        private Vector3 _localRotation;

        public Vector3 position 
        {
            get { return _position; }
            set { _position = value; }
        }
        public Vector3 localPosition
        {
            get { return _localPosition; }
            set { _localPosition = value; }
        }
        public Vector3 localRotation
        {
            get { return _localRotation; }
            set { _localRotation = value; }
        }
        public Vector3 right
        {
            get { return _localRotation; }
            private set { _localRotation = value; }
        }

        public Matrix4 GetTransformMatrix()
        {
            Matrix4 rotX = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_localRotation.X));
            Matrix4 rotY = Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(_localRotation.Y));
            Matrix4 rotZ = Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(_localRotation.Z));
            Matrix4 pos = Matrix4.CreateTranslation(position);
            return rotX * rotY * rotZ * pos;
        }


    }
}
