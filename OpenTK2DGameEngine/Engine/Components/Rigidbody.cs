using System.Numerics;
using Newtonsoft.Json;

namespace MarioGabeKasper.Engine.Components
{
    public class Rigidbody : Component
    {
        public int ColliderType = 0;
        public float Friction = 0.9f;
        public Vector3 Vel = Vector3.One;
        [JsonIgnore] private Vector4 tmp = new Vector4(1, 1, 1, 0);
        
        public override void SetObjectType()
        {
            ObjType = 3;
        }
    }
}