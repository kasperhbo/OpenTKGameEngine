using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using MarioGabeKasper.Engine.Components;

namespace MarioGabeKasper.Engine.Core
{
    public class GameObject
    {
        private static int _idCounter = 0;
        
        public int Uid                      ;
        public string Name                  ;
        public int ZIndex                   ;
        public Transform Transform          ;
        public List<Component> Components   ;

        private int objType = 5;
        public GameObject(string name, Transform transform, int zIndex)
        {
            Components = new List<Component>();
            this.Name = name;
            this.ZIndex = zIndex;
            this.Transform = transform;
            Uid = _idCounter++;
        }

        public virtual Component GetComponent<T>(Type componentClass) where T : Component
        {
            foreach (var c in Components)
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
            return Transform;
        }

        public virtual Component RemoveComponent<T>(Type componentClass) where T : Component
        {
            for (var i = 0; i < Components.Count; i++)
            {
                var c = Components[i];
                if (componentClass.IsAssignableFrom(c.GetType()))
                {
                    Components.RemoveAt(i);
                    return default;
                }
            }

            return null;
        }

        public Component AddComponent(Component c)
        {
            c.GenerateId();
            c.Parent = this;
            Components.Add(c);
            return c;
        }
        
        /// <summary>
        /// Initialize method, happens before anything else
        /// </summary>
        /// <param name="maxId"></param>
        public static void Init(int maxId)
        {
            _idCounter = maxId;
        }

        /// <summary>
        /// Runs when game object is started
        /// </summary>
        public void Start()
        {
            foreach (var component in Components) component.Start(this);
        }

        /// <summary>
        /// Runs every frame
        /// </summary>
        /// <param name="dt"></param>
        public void Update(float dt)
        {
            foreach (var component in Components)
            {
                component.Update(dt);
            }
        }

        public void ImGui_()
        {
            foreach (var component in Components) component.ImGui_();
            GetTransform().ImGui_();
        }
    }
}