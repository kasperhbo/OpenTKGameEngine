using System.Numerics;

using MarioGabeKasper.Engine.Core;
using MarioGabeKasper.Engine.Renderer;
using OpenTK.Audio.OpenAL.Extensions.SOFT.DeviceClock;


namespace MarioGabeKasper.Engine.Components
{
    public class SpriteRenderer : Component
    {
        public Vector4 Color = new Vector4(1,1,1,1);
        
        public Sprite Sprite = new Sprite();

        private Transform lastTransform;
        public bool IsDirty = true;
        
        public Texture Texture => Sprite.Texture;
        public Vector2[] TextureCoords => Sprite.TexCoords;
        
        public SpriteRenderer()
        {
            SetObjectType();
        }
        
        public override void Start(GameObject gameObject)
        {
            lastTransform = gameObject.GetTransform().Copy();
            IsDirty = true;
            base.Start(gameObject);
        }
        
        public override void Update(float dt)
        {
            // Console.WriteLine(this._sprite.texture);
            if (!lastTransform.Equals(Parent.GetTransform()))
            {
                // Console.WriteLine("Transform changed");
                Parent.GetTransform().Copy(lastTransform);
                IsDirty = true;
            }
        }
        
        public override void ImGui_()
        {
            System.Numerics.Vector4 col = new System.Numerics.Vector4(Color.X, Color.Y, Color.Z, Color.W);
            
            if (ImGuiNET.ImGui.ColorPicker4("test", ref col))
            {
                this.Color = new Vector4(col.X, col.Y, col.Z, col.W);
                IsDirty = true;
            }

            base.ImGui_();
        }
       
        public sealed override void SetObjectType()
        {
            ObjType = 2;
        }

    }
}