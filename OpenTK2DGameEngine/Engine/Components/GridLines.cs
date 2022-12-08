
using System.Numerics;
using MarioGabeKasper.Engine.Core;
using MarioGabeKasper.Engine.Renderer;
using MarioGabeKasper.Engine.Utils;
using Vector2 = OpenTK.Mathematics.Vector2;

namespace MarioGabeKasper.Engine.Components
{
    public class GridLines : Component
    {
        public override void Update(float dt)
        {
            Vector2 cameraPos = Math.DefaultToOpenTk(Window.GetScene().GetCamera().Transform.Position);
            Vector2 projectionSize = new Vector2(Window.GetScene().GetCamera().GetProjectSize().X,Window.GetScene().GetCamera().GetProjectSize().Y);

            float firstX = (((cameraPos.X / Window.Get().Settings.GridSize.X) - 1) * Window.Get().Settings.GridSize.X);
            
            float firstY = (((cameraPos.Y / Window.Get().Settings.GridSize.Y) -1) * Window.Get().Settings.GridSize.Y);

            int numVerticalLines = (int)(projectionSize.X / Window.Get().Settings.GridSize.X) + 2;
            int numHorizontalLines = (int)(projectionSize.Y / Window.Get().Settings.GridSize.Y) + 2;

            float width = projectionSize.X + Window.Get().Settings.GridSize.X * 2;
            float height = projectionSize.Y + Window.Get().Settings.GridSize.Y * 2;

            int maxLines = numVerticalLines > numHorizontalLines ? numVerticalLines : numHorizontalLines;
            Vector3 color = new Vector3(1, 0, 0);
            
            for (int i = 0; i < maxLines; i++)
            {
                float x = firstX + (Window.Get().Settings.GridSize.X * i);
                float y = firstY + (Window.Get().Settings.GridSize.Y * i);

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