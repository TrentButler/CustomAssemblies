using System;
namespace RoslynCompiler
{
    public class Globals
    {
        public class Vector3
        {
            public static Vector3 right { get { return new Vector3(1, 0, 0); } }
            public static Vector3 left { get { return new Vector3(-1, 0, 0); } }
            public static Vector3 up { get { return new Vector3(0, 1, 0); } }
            public static Vector3 down { get { return new Vector3(0, -1, 0); } }
            public static Vector3 forward { get { return new Vector3(0, 0, 1); } }
            public static Vector3 back { get { return new Vector3(0, 0, -1); } }
            public static Vector3 one { get { return new Vector3(1, 1, 1); } }
            public static Vector3 zero { get { return new Vector3(0, 0, 0); } }
            public float magnitude { get { return (float)System.Math.Sqrt(_x * _x + _y * _y + _z * _z); } }
            public Vector3 normalized { get { return new Vector3(_x / magnitude, _y / magnitude, _z / magnitude); } }

            private float _x, _y, _z;
            public float x { get { return _x; } set { _x = value; } }
            public float y { get { return _y; } set { _y = value; } }
            public float z { get { return _z; } set { _z = value; } }

            public Vector3(float X, float Y, float Z)
            {
                _x = X;
                _y = Y;
                _z = Z;
            }

            public static Vector3 Cross(Vector3 left, Vector3 right)
            {
                var x = (left.y * right.z) - (left.z * right.y);
                var y = (left.x * right.z) - (left.z * right.x);
                var z = (left.x * right.y) - (left.y * right.x);
                return new Vector3(x, y, z);
            }

            public static Vector3 Dot(Vector3 left, Vector3 right)
            {
                return new Vector3(left.x * right.x, left.y * right.y, left.z * right.z);
            }

            public override string ToString()
            {
                return string.Format("Vector3({0}, {1}, {2})", _x, _y, _z);
            }
        }
    }
}
