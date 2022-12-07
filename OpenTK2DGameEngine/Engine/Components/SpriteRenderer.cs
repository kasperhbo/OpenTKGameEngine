using System.Numerics;

using MarioGabeKasper.Engine.Core;
using MarioGabeKasper.Engine.Renderer;

// using OpenTK.Mathematics;

namespace MarioGabeKasper.Engine.Components
{
    public class SpriteRenderer : Component
    {
        public Vector4 _color = new Vector4(1, 1, 1, 1);
        public Sprite _sprite = new Sprite();

        private Transform lastTransform;
        private bool isDirty = true;

        public SpriteRenderer()
        {
            SetObjectType();
        }
        
        public override void Start(GameObject gameObject)
        {
            lastTransform = gameObject.GetTransform().Copy();
            isDirty = true;
            base.Start(gameObject);
        }
        
        public override void update(float dt)
        {
            // Console.WriteLine(this._sprite.texture);
            if (!lastTransform.Equals(parent.GetTransform()))
            {
                // Console.WriteLine("Transform changed");
                parent.GetTransform().Copy(lastTransform);
                isDirty = true;
            }
        }
        
        public override void ImGui()
        {
            System.Numerics.Vector4 _col = new System.Numerics.Vector4(_color.X, _color.Y, _color.Z, _color.W);
            
            if (ImGuiNET.ImGui.ColorPicker4("test", ref _col))
            {
                this._color = new Vector4(_col.X, _col.Y, _col.Z, _col.W);
                isDirty = true;
            }

            base.ImGui();
        }
        
        public Vector4 GetColor() {
            return this._color;
        }

        public Texture GetTexture() {
            return _sprite.GetTexture();
        }
        
        public System.Numerics.Vector2[] GetTexCoords() {
            return _sprite.GetTexCoords();
        }
        
        public void SetSprite(Sprite sprite)
        {
            _sprite = sprite;
            isDirty = true;
        }

        public void SetColor(Vector4 color)
        {
            if (!_color.Equals(color))
            {
                _color = color;
                isDirty = true;
            }
        }

        public bool IsDirty()
        {
            return isDirty;
        }

        public void SetClean()
        {
            isDirty = false;
        }
        
        public override void SetObjectType()
        {
            objType = 2;
        }

    }
}