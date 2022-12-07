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
        protected Renderer.Renderer _renderer = new();
        protected Camera s_camera;
        protected GameObject p_activeGameObject = null;
        protected bool p_loadedScene = false;
        protected Window p_window;

        public virtual void Init(Window window)
        {
            p_window = window;
            // p_mouseListener = mouseListener;
        }

        public void Start()
        {
            foreach (var go in GameObjects)
            {
                go.Start();
                _renderer.Add(go, s_camera);
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
            _renderer.Add(go, s_camera);
        }

        public abstract void Update(float dt, MouseState mouseState, KeyboardState keyboardState);
        public abstract void Render();

        public virtual void SceneImGui(ImGuiController imGuiController)
        {
            //Create Inspector for the currently gameobject
            if(p_activeGameObject != null)
            {
                ImGuiNET.ImGui.Begin("Inspector");

                p_activeGameObject.ImGui();

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

                int maxGOID = -1;
                int maxCompID = -1;
                
                for (int i = 0; i < objs.Count; i++)
                {
                    AddGameObjectToScene(objs[i]);

                    foreach (Component c in objs[i].GetAllComponent())
                    {
                        if (c.GetUID() > maxCompID)
                        {
                            maxCompID = c.GetUID();
                        }
                    }

                    if (objs[i].GetUID() > maxGOID)
                    {
                        maxGOID = objs[i].GetUID();
                    }
                }

                maxGOID++;
                maxCompID++;
                
                GameObject.Init(maxGOID);
                Component.Init(maxCompID);

                p_loadedScene = true;
            }
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(GameObjects.ToArray());
            File.WriteAllText("../../../data.json", json);
        }

        public Camera GetCamera()
        {
            return s_camera;
        }

        protected MouseState _mouseState;
        
        public int GetCurrentMouseDown()
        {
            int buttonNum = -1;

            if (_mouseState.IsButtonDown(MouseButton.Button1))
            {
                buttonNum = 0;
            }
        
            if (_mouseState.IsButtonDown(MouseButton.Button2))
            {
                buttonNum = 1;
            }
        
            if (_mouseState.IsButtonDown(MouseButton.Button3))
            {
                buttonNum = 2;
            }
        
            if (_mouseState.IsButtonDown(MouseButton.Button4))
            {
                buttonNum = 3;
            }
        
            if (_mouseState.IsButtonDown(MouseButton.Button5))
            {
                buttonNum = 4;
            }
        
            if (_mouseState.IsButtonDown(MouseButton.Button6))
            {
                buttonNum = 5;
            }
        
            if (_mouseState.IsButtonDown(MouseButton.Button7))
            {
                buttonNum = 6;
            }
        
            if (_mouseState.IsButtonDown(MouseButton.Button8))
            {
                buttonNum = 7;
            }

            return buttonNum;
        }
    }
}