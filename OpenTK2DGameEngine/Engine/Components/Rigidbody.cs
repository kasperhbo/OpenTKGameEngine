using System.Numerics;
using Newtonsoft.Json;

namespace MarioGabeKasper.Engine.Components
{
    public class Rigidbody : Component
    {
        public int colliderType = 0;
        public float friction = 0.9f;
        public Vector3 vel = Vector3.One;
        [JsonIgnore] private Vector4 tmp = new Vector4(1, 1, 1, 0);
        
        public override void SetObjectType()
        {
            objType = 3;
        }
    }
}