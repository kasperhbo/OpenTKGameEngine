using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MarioGabeKasper.Engine.Components;
using MarioGabeKasper.Engine.Core;
using MarioGabeKasper.Engine.GUI;
using Newtonsoft.Json;
using Window = MarioGabeKasper.Engine.Core.Window;

namespace MarioGabeKasper.Engine
{
    public abstract class Scene
    {
        protected readonly List<GameObject> GameObjects = new();
        private bool _isRunning;
        protected Renderer.Renderer Renderer = new();
        public Camera SCamera { get; protected set; }
        
        protected GameObject PActiveGameObject = null;

        public string CurrentSceneName = "test01.scene";

        public virtual void Init(Window window, string currentSceneName)
        {
            this.CurrentSceneName = currentSceneName;
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

        public abstract void Update(float dt);
        public abstract void Render();

        public virtual void SceneImGui(ImGuiController imGuiController)
        {
            //Update level editor scene imgui
            ImGui_(imGuiController);
        }

        
        public virtual void ImGui_(ImGuiController imGuiController)
        {
            
        }
        
        /// <summary>
        /// Load the data of the scene
        /// </summary>
        public void LoadScene(string SceneToOpen)
        {
            SceneToOpen = "Resources/Scenes/" + SceneToOpen;
            
            if(File.Exists(SceneToOpen)) 
            {
                List<GameObject> objs = JsonConvert.DeserializeObject<List<GameObject>>(File.ReadAllText(SceneToOpen));

                int maxGoid = -1;
                int maxCompId = -1;
                
                for (int i = 0; i < objs.Count; i++)
                {
                    AddGameObjectToScene(objs[i]);

                    foreach (Component c in objs[i].Components)
                    {
                        if (c.Uid > maxCompId)
                        {
                            maxCompId = c.Uid;
                        }
                    }

                    if (objs[i].Uid > maxGoid)
                    {
                        maxGoid = objs[i].Uid;
                    }
                }

                maxGoid++;
                maxCompId++;
                
                GameObject.Init(maxGoid);
                Component.Init(maxCompId);
            }
        }

        /// <summary>
        /// Save data of scene on exit
        /// </summary>
        public void SaveScene()
        {
            string sceneData = JsonConvert.SerializeObject(GameObjects.ToArray());
            string path = "Resources/Scenes/" + CurrentSceneName;
            
            if (File.Exists(path))
            {
                File.WriteAllText( path, sceneData);
            }
            else
            {
                using (FileStream fs = File.Create(path))
                {
                    fs.Close();
                }
                File.WriteAllText(path, sceneData);
            }
        } 
    }
}