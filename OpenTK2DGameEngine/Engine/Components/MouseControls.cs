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

        public void Place()
        {
            this.holdingObject = null;
        }

        public override void update(float dt)
        {
            if (holdingObject != null)
            {
                //Transform transform = (Transform)holdingObject.GetComponent<Transform>(typeof(Transform));
                holdingObject.GetTransform().Position.X = MouseListener.GetOrthoX();
                holdingObject.GetTransform().Position.Y = MouseListener.GetOrthoY();

                holdingObject.GetTransform().Position.X =
                    (int)(holdingObject.GetTransform().Position.X / Settings.GridWidth) * Settings.GridWidth;

                holdingObject.GetTransform().Position.Y =
                    (int)(holdingObject.GetTransform().Position.Y / Settings.GridHeight) * Settings.GridHeight;

                
                if (MouseListener.mouseButtonDown(Window.GetScene().GetCurrentMouseDown())) {
                    Place();
                }
            }
        }

        public override void SetObjectType()
        {
            objType = 5;
        }
    }
}