using System.Text.Json.Serialization;
using MarioGabeKasper.Engine.Renderer;
using OpenTK.Mathematics;

namespace MarioGabeKasper.Engine.Components
{
    public class Sprite
    {
        public float Width, Height;
        public Texture Texture = null;
        public int TextureId
        {
            get
            {
                return Texture == null ? -1 : Texture.TexId;

            }
        }
        
        public System.Numerics.Vector2[] TexCoords = new[]
        {
            new System.Numerics.Vector2(1, 1),
            new System.Numerics.Vector2(1, 0),
            new System.Numerics.Vector2(0, 0),
            new System.Numerics.Vector2(0, 1)
        };
    }
}