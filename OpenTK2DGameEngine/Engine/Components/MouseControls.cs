using System;
using MarioGabeKasper.Engine.Core;

namespace MarioGabeKasper.Engine.Components
{
    public class MouseControls : Component
    {
        private GameObject holdingObject = null;

        public void PickupObject(GameObject go)
        {
            Console.WriteLine("Pickup");
            this.holdingObject = go;
            Window.GetScene().AddGameObjectToScene(go);
        }

        private void Place()
        {
            this.holdingObject = null;
        }

        public override void Update(float dt)
        {
            if (holdingObject != null)
            {
                //Transform transform = (Transform)holdingObject.GetComponent<Transform>(typeof(Transform));
                holdingObject.GetTransform().Position.X = MouseListener.GetOrthoX();
                holdingObject.GetTransform().Position.Y = MouseListener.GetOrthoY();

                holdingObject.GetTransform().Position.X =
                    (int)(holdingObject.GetTransform().Position.X / Window.Get().Settings.GridSize.X) * Window.Get().Settings.GridSize.X;

                holdingObject.GetTransform().Position.Y =
                    (int)(holdingObject.GetTransform().Position.Y / Window.Get().Settings.GridSize.Y) * Window.Get().Settings.GridSize.Y;

                
                if (MouseListener.MouseButtonDown(Window.GetScene().GetCurrentMouseDown())) {
                    Place();
                }
            }
        }

        public override void SetObjectType()
        {
            ObjType = 5;
        }
    }
}