using OpenTK.Mathematics;

namespace MarioGabeKasper.Engine.Components.Colliders
{
    internal class BoxCollider : Collider
    {
        public Vector2 Offset { get; private set; }
        public Vector2 Scale { get; private set; }
        
        public override void SetObjectType()
        {
            ObjType = 6;
        }
    }
}
