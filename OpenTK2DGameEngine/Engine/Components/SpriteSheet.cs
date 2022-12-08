using System.Collections.Generic;
using MarioGabeKasper.Engine.Renderer;
using OpenTK.Mathematics;

namespace MarioGabeKasper.Engine.Components
{
    public class SpriteSheet
    {
        private Texture texture;
        public List<Sprite> sprites;

        public SpriteSheet(Texture texture, int spriteWidth, int spriteHeight, int numSprites, int spacing)
        {
            this.sprites = new List<Sprite>();

            this.texture = texture;
            
            int currentX = 0;
            
            int currentY = texture.GetHeight() - spriteHeight;
            
            for (int i=0; i < numSprites; i++) {
                
                float topY = (currentY + spriteHeight) / (float)texture.GetHeight();
                float rightX = (currentX + spriteWidth) / (float)texture.GetWidth();
                float leftX = currentX / (float)texture.GetWidth();
                float bottomY = currentY / (float)texture.GetHeight();

                System.Numerics.Vector2[] texCoords = {
                    new System.Numerics.Vector2(rightX, topY),
                    new System.Numerics.Vector2(rightX, bottomY),
                    new System.Numerics.Vector2(leftX, bottomY),
                    new System.Numerics.Vector2(leftX, topY)
                };
                Sprite sprite = new Sprite();
                
                sprite.Texture = this.texture;
                sprite.TexCoords = texCoords;
                sprite.Width = spriteWidth;
                sprite.Height = spriteHeight;
                
                this.sprites.Add(sprite);

                currentX += spriteWidth + spacing;
                if (currentX >= texture.GetWidth()) {
                    currentX = 0;
                    currentY -= spriteHeight + spacing;
                }
            }
        }
        
        public Sprite GetSprite(int index)
        {
            return this.sprites[index];
        }
    }
}