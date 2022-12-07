using OpenTK.Mathematics;

namespace MarioGabeKasper.Engine.Components.Colliders
{
    internal class BoxCollider : Collider
    {
        public Vector2 offset { get; private set; }
        public Vector2 scale { get; private set; }
        
        public override void SetObjectType()
        {
            objType = 6;
        }
    }
}
