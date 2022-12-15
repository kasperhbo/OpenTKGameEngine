using System.Collections.Generic;
using MarioGabeKasper.Engine.Renderer;
using OpenTK.Mathematics;

namespace MarioGabeKasper.Engine.Components
{
    public class SpriteSheet
    {
        private Texture texture;
        
        public List<Sprite> sprites = new List<Sprite>();
        public Sprite GetSprite(int index)
        {
            return this.sprites[index];
        }

        public SpriteSheet(Texture texture, int spriteWidth, int spriteHeight, int numSprites, int spacing)
        {
            this.sprites = new List<Sprite>();

            this.texture = texture;
            
            int currentX = 0;
            
            int currentY = texture.Height - spriteHeight;
            
            for (int i=0; i < numSprites; i++) {
                
                float topY = (currentY + spriteHeight) / (float)texture.Height;
                float rightX = (currentX + spriteWidth) / (float)texture.Width;
                float leftX = currentX / (float)texture.Width;
                float bottomY = currentY / (float)texture.Height;

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
                if (currentX >= texture.Width) {
                    currentX = 0;
                    currentY -= spriteHeight + spacing;
                }
            }
        }
        
    }
}