using System.Text.Json.Serialization;
using MarioGabeKasper.Engine.Renderer;
using OpenTK.Mathematics;

namespace MarioGabeKasper.Engine.Components
{
    public class Sprite
    {
        private float width, height;
        
        public Texture texture = null;
        
        public System.Numerics.Vector2[] texCoords = new[]
        {
            new System.Numerics.Vector2(1, 1),
            new System.Numerics.Vector2(1, 0),
            new System.Numerics.Vector2(0, 0),
            new System.Numerics.Vector2(0, 1)
        };

        public Texture GetTexture() {
            return this.texture;
        }

        public System.Numerics.Vector2[] GetTexCoords() {
            return this.texCoords;
        }

        public void SetTexture(Texture tex) {
            this.texture = tex;
        }

        public void SetTexCoords(System.Numerics.Vector2[] texCoords) {
            this.texCoords = texCoords;
        }

        public void SetHeight(float height)
        {
            this.height = height;
        }

        public void SetWidth(float width)
        {
            this.width = width;
        }
        
        public float GetHeight()
        {
            return this.height;
        }

        public float GetWidth()
        {
            return this.width;
        }


        public int GetTexID()
        {
            return texture == null ? -1 : texture.texID;
        }
    }
}