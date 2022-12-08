using System;
using MarioGabeKasper.Engine.Core;

namespace MarioGabeKasper.Engine.Components
{
    public class FontRenderer : Component
    {
        public FontRenderer()
        {
            SetObjectType();
        }
        
        public override void Start(GameObject gameobject)
        {
            if (Parent.GetComponent<SpriteRenderer>(typeof(SpriteRenderer)) != null)
                Console.WriteLine("Found Font Renderer");

            base.Start(gameobject);
        }

        public override void Update(float dt)
        {
        }

        public sealed override void SetObjectType()
        {
            ObjType = 1;
        }
    }
}