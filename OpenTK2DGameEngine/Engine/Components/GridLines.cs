
using System.Numerics;
using MarioGabeKasper.Engine.Core;
using MarioGabeKasper.Engine.Renderer;
using Vector2 = OpenTK.Mathematics.Vector2;

namespace MarioGabeKasper.Engine.Components
{
    public class GridLines : Component
    {
        public override void update(float dt)
        {
            Vector2 cameraPos = Window.GetScene().GetCamera().position;
            Vector2 projectionSize = new Vector2(Window.GetScene().GetCamera().GetProjectSize().X,Window.GetScene().GetCamera().GetProjectSize().Y);

            int firstX = ((int)(cameraPos.X / Settings.GridWidth)- 1) * Settings.GridWidth;
            int firstY = ((int)(cameraPos.Y / Settings.GridHeight) -1) * Settings.GridHeight;

            int numVerticalLines = (int)(projectionSize.X / Settings.GridWidth) + 2;
            int numHorizontalLines = (int)(projectionSize.Y / Settings.GridHeight) + 2;

            int height = (int)projectionSize.Y + Settings.GridHeight * 2;
            int width = (int)projectionSize.X + Settings.GridWidth * 2;

            int maxLines = numVerticalLines > numHorizontalLines ? numVerticalLines : numHorizontalLines;
            Vector3 color = new Vector3(1, 0, 0);
            
            for (int i = 0; i < maxLines; i++)
            {
                int x = firstX + (Settings.GridWidth * i);
                int y = firstY + (Settings.GridHeight * i);

                if (i < numVerticalLines)
                {
                    DebugDraw.AddLine2D(new System.Numerics.Vector2(x, firstY), new System.Numerics.Vector2(x, firstY + height), color);
                }

                if (i < numHorizontalLines)
                {
                    DebugDraw.AddLine2D(new System.Numerics.Vector2(firstX, y), new System.Numerics.Vector2(firstX + width, y), color);
                }
            }
            
            base.update(dt);
        }

        public override void SetObjectType()
        {
            objType = -1;
        }
    }
}