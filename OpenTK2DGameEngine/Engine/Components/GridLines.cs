using System.Numerics;
using MarioGabeKasper.Engine.Core;
using MarioGabeKasper.Engine.Renderer;
using Vector2 = OpenTK.Mathematics.Vector2;

namespace MarioGabeKasper.Engine.Components
{
    public class GridLines : Component
    {
        public override void Update(float dt)
        {
            Vector2 cameraPos = Window.CurrentScene.SCamera.position;
            Vector2 projectionSize = new Vector2(Window.CurrentScene.SCamera.GetProjectSize().X,Window.CurrentScene.SCamera.GetProjectSize().Y);

            int firstX = ((int)(cameraPos.X / Window.Get().Settings.GridSize.X)- 1) * Window.Get().Settings.GridSize.X;
            int firstY = ((int)(cameraPos.Y / Window.Get().Settings.GridSize.Y) -1) * Window.Get().Settings.GridSize.Y;

            int numVerticalLines = (int)(projectionSize.X / Window.Get().Settings.GridSize.X) + 2;
            int numHorizontalLines = (int)(projectionSize.Y / Window.Get().Settings.GridSize.Y) + 2;

            int height = (int)projectionSize.Y + Window.Get().Settings.GridSize.Y * 2;
            int width = (int)projectionSize.X + Window.Get().Settings.GridSize.X * 2;

            int maxLines = numVerticalLines > numHorizontalLines ? numVerticalLines : numHorizontalLines;
            Vector3 color = new Vector3(1, 0, 0);
            
            for (int i = 0; i < maxLines; i++)
            {
                int x = firstX + (Window.Get().Settings.GridSize.X * i);
                int y = firstY + (Window.Get().Settings.GridSize.Y * i);

                if (i < numVerticalLines)
                {
                    DebugDraw.AddLine2D(new System.Numerics.Vector2(x, firstY), new System.Numerics.Vector2(x, firstY + height), color);
                }

                if (i < numHorizontalLines)
                {
                    DebugDraw.AddLine2D(new System.Numerics.Vector2(firstX, y), new System.Numerics.Vector2(firstX + width, y), color);
                }
            }
            
            base.Update(dt);
        }

        public override void SetObjectType()
        {
            ObjType = -1;
        }
    }
}