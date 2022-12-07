using System;
using System.Collections.Generic;
using System.Diagnostics;
using MarioGabeKasper.Engine.Components;

namespace MarioGabeKasper.Engine.Core
{
    public class GameObject
    {
        private static int idCounter = 0;
        public int uid = -1;
        public string name;
        public int zIndex;
        public Transform transform;

        public List<Component> components = new List<Component>();

        private int objType = 5;

        public GameObject(string name, Transform transform, int zIndex)
        {
            this.name = name;
            this.zIndex = zIndex;
            this.transform = transform;
            uid = idCounter++;
        }

        public virtual Component GetComponent<T>(Type componentClass) where T : Component
        {
            foreach (var c in components)
                if (componentClass.IsAssignableFrom(c.GetType()))
                    try
                    {
                        return c;
                    }
                    catch (InvalidCastException e)
                    {
                        Console.WriteLine(e.ToString());
                        Console.Write(e.StackTrace);
                        Debug.Assert(false, "Error: Casting component.");
                    }

            return default(T);
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public virtual Component RemoveComponent<T>(Type componentClass) where T : Component
        {
            for (var i = 0; i < components.Count; i++)
            {
                var c = components[i];
                if (componentClass.IsAssignableFrom(c.GetType()))
                {
                    components.RemoveAt(i);
                    return default;
                }
            }

            return null;
        }

        public void AddComponent(Component c)
        {
            c.GenerateID();
            components.Add(c);
            c.SetGameObject(this);
        }

        public void Start()
        {
            foreach (var component in components) component.Start(this);
        }

        public void Update(float dt)
        {
            ;
            foreach (var component in components)
            {
                component.update(dt);
            }
        }

        public int GetZIndex()
        {
            return zIndex;
        }

        public void ImGui()
        {
            foreach (var component in components) component.ImGui();
            GetTransform().ImGui();
        }

        public static void Init(int maxID)
        {
            idCounter = maxID;
        }

        public List<Component> GetAllComponent()
        {
            return components;
        }

        public int GetUID()
        {
            return uid;
        }
    }
}