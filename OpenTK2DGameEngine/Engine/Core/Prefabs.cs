using System.Numerics;
using MarioGabeKasper.Engine.Components;

namespace MarioGabeKasper.Engine.Core
{
    public class Prefabs
    {
        public static GameObject GenerateSpriteObject(Sprite sprite, float sizeX, float sizeY)
        {
            GameObject block = new GameObject("Sprite_Object_Gen", new Transform(new Vector2(), new Vector2(sizeX, sizeY)), 0);
            SpriteRenderer spriteRenderer = new SpriteRenderer();
            spriteRenderer.Sprite = sprite;
            block.AddComponent(spriteRenderer);

            return block;
        }
    }
}