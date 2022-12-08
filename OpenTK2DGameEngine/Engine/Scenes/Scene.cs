using System;
using System.Collections.Generic;
using System.IO;
using MarioGabeKasper.Engine.Components;
using MarioGabeKasper.Engine.Core;
using MarioGabeKasper.Engine.GUI;
using MarioGabeKasper.Engine.Utils;
using Newtonsoft.Json;
using OpenTK.Platform.Windows;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Window = MarioGabeKasper.Engine.Core.Window;

namespace MarioGabeKasper.Engine
{
    public abstract class Scene
    {
        protected readonly List<GameObject> GameObjects = new();
        private bool _isRunning;
        protected Renderer.Renderer Renderer = new();
        protected Camera SCamera;
        protected GameObject PActiveGameObject = null;

        public virtual void Init(Window window)
        { 

            
            // p_mouseListener = mouseListener;
        }

        public void Start()
        {
            foreach (var go in GameObjects)
            {
                go.Start();
                Renderer.Add(go, SCamera);
            }
            
            _isRunning = true;
        }

        public void AddGameObjectToScene(GameObject go)
        {
            if (!_isRunning)
            {
                GameObjects.Add(go);
                return;
            }

            GameObjects.Add(go);
            go.Start();
            Renderer.Add(go, SCamera);
        }

        public abstract void Update(float dt, MouseState mouseState, KeyboardState keyboardState);
        public abstract void Render();

        public virtual void SceneImGui(ImGuiController imGuiController)
        {
            //Create Inspector for the currently gameobject
            if(PActiveGameObject != null)
            {
                ImGuiNET.ImGui.Begin("Inspector");

                PActiveGameObject.ImGui_();

                ImGuiNET.ImGui.End();
            }

            GetCamera().ImGui(imGuiController);

            ImGui(imGuiController);
        }

        public virtual void ImGui(ImGuiController imGuiController)
        {
            
        }

        public void Load()
        {
            if(File.Exists("../../../data.json"))
            {
                List<GameObject> objs = JsonConvert.DeserializeObject<List<GameObject>>(File.ReadAllText("../../../data.json"));

                int maxGoid = -1;
                int maxCompId = -1;
                
                for (int i = 0; i < objs.Count; i++)
                {
                    AddGameObjectToScene(objs[i]);

                    foreach (Component c in objs[i].GetAllComponent())
                    {
                        if (c.Uid > maxCompId)
                        {
                            maxCompId = c.Uid;
                        }
                    }

                    if (objs[i].GetUid() > maxGoid)
                    {
                        maxGoid = objs[i].GetUid();
                    }
                }

                maxGoid++;
                maxCompId++;
                
                GameObject.Init(maxGoid);
                Component.Init(maxCompId);
            }
        }

        public void SaveScene()
        {
            string sceneData = JsonConvert.SerializeObject(GameObjects.ToArray());
            File.WriteAllText("../../../data.json", sceneData);
        }
        public Camera GetCamera()
        {
            return SCamera;
        }

        protected MouseState MouseState;
        
        public int GetCurrentMouseDown()
        {
            int buttonNum = -1;

            if (MouseState.IsButtonDown(MouseButton.Button1))
            {
                buttonNum = 0;
            }
        
            if (MouseState.IsButtonDown(MouseButton.Button2))
            {
                buttonNum = 1;
            }
        
            if (MouseState.IsButtonDown(MouseButton.Button3))
            {
                buttonNum = 2;
            }
        
            if (MouseState.IsButtonDown(MouseButton.Button4))
            {
                buttonNum = 3;
            }
        
            if (MouseState.IsButtonDown(MouseButton.Button5))
            {
                buttonNum = 4;
            }
        
            if (MouseState.IsButtonDown(MouseButton.Button6))
            {
                buttonNum = 5;
            }
        
            if (MouseState.IsButtonDown(MouseButton.Button7))
            {
                buttonNum = 6;
            }
        
            if (MouseState.IsButtonDown(MouseButton.Button8))
            {
                buttonNum = 7;
            }

            return buttonNum;
        }
    }
}