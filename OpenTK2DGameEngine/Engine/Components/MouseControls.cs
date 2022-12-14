using MarioGabeKasper.Engine.Core;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Window = MarioGabeKasper.Engine.Core.Window;

namespace MarioGabeKasper.Engine.Components;

public class MouseControls : Component
{
    private GameObject holdingObject = null;

    public void PickupObject(GameObject go)
    {
        this.holdingObject = go;
        Window.CurrentScene.AddGameObjectToScene(go);
    }

    public override void Update(float dt)
    {
        if (holdingObject != null)
        {
            holdingObject.Transform.Position.X = Input.OrthoX();
            holdingObject.Transform.Position.Y = Input.OrthoY();

            holdingObject.Transform.Position.X =
                (int)(holdingObject.Transform.Position.X / Window.Get().Settings.GridSize.X) * Window.Get().Settings.GridSize.Y;
                
            holdingObject.Transform.Position.Y =
                (int)(holdingObject.Transform.Position.Y / Window.Get().Settings.GridSize.Y) * Window.Get().Settings.GridSize.Y;

            if (Input.MouseDown(MouseButton.Button1)) {
                Place();
            }
        }
    }
    
    public void Place()
    {
        this.holdingObject = null;
    }

    public override void SetObjectType()
    {
        ObjType = 5;
    }
}