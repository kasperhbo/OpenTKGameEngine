using System;
using ImGuiNET;
using MarioGabeKasper.Engine.Components;
using MarioGabeKasper.Engine.Components.Colliders;
using MarioGabeKasper.Engine.Core;
using MarioGabeKasper.Engine.GUI;
using MarioGabeKasper.Engine.Sound;
using MarioGabeKasper.Engine.Utils;
using Window = MarioGabeKasper.Engine.Core.Window;
using OpenTK.Windowing.GraphicsLibraryFramework;

using Vector2 = System.Numerics.Vector2;
using Vector3 = OpenTK.Mathematics.Vector3;

namespace MarioGabeKasper.Engine.Scenes
{
    public class LevelEditorScene : Scene
    {
        private SpriteSheet sprites;
        
        GameObject levelEditorStuff = new GameObject("level editor", new Transform(), 0);
        
        public override void Init(Window window)
        {
            base.Init(window);
            LoadResources();

            SCamera = new Camera(new Vector3(-250, 0, 0));
            
            sprites = AssetPool.GetSpriteSheet("decorationsandblocks.png");
            
            levelEditorStuff.AddComponent(new MouseControls());
            levelEditorStuff.AddComponent(new GridLines());
            
            levelEditorStuff.AddComponent(new GameEngineSound());
            GameEngineSound s = (GameEngineSound)levelEditorStuff.GetComponent<GameEngineSound>(typeof(GameEngineSound));
            
            s.SetAudioFile("../../../Resources/drums.wav");
            s.Play();

            Window.Get().KeyDown += args =>
            {
                if(args.Key == Keys.B)
                    s.PlayPause();
            };
            
            PActiveGameObject = GameObjects[0];
        }

        /// <summary>
        /// Loading All The Textures/Shaders/Sounds into the asset pool before the scene is started for speed
        /// </summary>
        private void LoadResources()
        {
            AssetPool.GetShader(new ShaderSource("../../../default.vert", "../../../default.frag"));
            //SpriteSheet file, sprite width, sprite height, num of sprites, spacing
            AssetPool.AddSpriteSheet("decorationsandblocks.png",
            new SpriteSheet(AssetPool.GetTexture("decorationsandblocks.png"),
                16, 16, 81, 0
            ));
        
            AssetPool.GetTexture("blendImage1.png");
            AssetPool.GetTexture("mario.png");
        }
        
        /// <summary>
        /// Update that runs every frame
        /// </summary>
        /// <param name="dt">Delta time</param>
        /// <param name="mouseState">State of the mouse</param>
        /// <param name="input">State of the keyboard</param>
        public override void Update(float dt)
        {
            //Update the level editor gameobject(TODO:REPLACE THIS WITH SOMETHING BETTER :)!)
            levelEditorStuff.Update(dt);
            
            //Update all the game objects in the scene
            foreach (var go in GameObjects) go.Update(dt);

            //Update camera controls
            CameraControls(dt);
            
            //Check if clicked an gameobject is clicked, if it is clicked start an imgui inspector window for the gameobject
            GameObject clicked = null;
            if (ClickedGameObject(out clicked))
            {
                PActiveGameObject = clicked;
            }

        }

        /// <summary>
        /// Check if the mouse clicks an gameobject
        /// </summary>
        /// <param name="go"> the (if)clicked gameobject</param>
        /// <returns></returns>
        private bool ClickedGameObject(out GameObject go)
        {
            if (Input.MouseDown(MouseButton.Button1))
            {
                foreach (GameObject gameObject in GameObjects)
                {
                    Transform trans = gameObject.Transform;

                    Vector2 pos = trans.Position;
                    Vector2 scale = trans.Scale;

                    if (CollisionCheck.PointInBox(pos.X, pos.Y, pos.X + scale.X, pos.Y + scale.Y,
                            Input.OrthoX(), Input.OrthoY()
                        ))
                    {                    
                        go = gameObject;
                        return true;
                    }
                }
            }

            go = null;
            return false;
        }

        /// <summary>
        /// Editor Camera Controls
        /// </summary>
        /// <param name="dt"></param>
        private float minTimeBetweenStepMoves = 2f;
        private float reamingTimeBetweenStepMoves = 2f;
        
        private void CameraControls(float dt)
        {
            reamingTimeBetweenStepMoves -= 10 * dt;
            
            if (reamingTimeBetweenStepMoves <= 0)
            {
                reamingTimeBetweenStepMoves = minTimeBetweenStepMoves;
                if (!Input.KeyDown(Keys.LeftControl))
                {
                    if (Input.KeyDown(Keys.Left))
                    {
                        Window.CurrentScene.SCamera.position.X -= 32;
                    }

                    if (Input.KeyDown(Keys.Right))
                    {
                        Window.CurrentScene.SCamera.position.X += 32;
                    }
                    if (Input.KeyDown(Keys.Down))
                    {
                        Window.CurrentScene.SCamera.position.Y -= 32;
                    }
                    if (Input.KeyDown(Keys.Up))
                    {
                        Window.CurrentScene.SCamera.position.Y += 32;
                    }
                }
            }
            
            if (Input.KeyDown(Keys.LeftControl))
            {
                if (Input.KeyDown(Keys.Left))
                {
                    Window.CurrentScene.SCamera.position.X -=
                        (Window.Get().Settings.SceneCameraSpeedMultiplier * 100) * dt;
                }

                if (Input.KeyDown(Keys.Right))
                {
                    Window.CurrentScene.SCamera.position.X +=
                        (Window.Get().Settings.SceneCameraSpeedMultiplier * 100) * dt;
                }

                if (Input.KeyDown(Keys.Up))
                {
                    Window.CurrentScene.SCamera.position.Y +=
                        (Window.Get().Settings.SceneCameraSpeedMultiplier * 100) * dt;
                }

                if (Input.KeyDown(Keys.Down))
                {
                    Window.CurrentScene.SCamera.position.Y -=
                        (Window.Get().Settings.SceneCameraSpeedMultiplier * 100) * dt;
                }
            }
            
            
        }

        /// <summary>
        /// Render the renderer
        /// </summary>
        public override void Render()
        {
            Renderer.Render();
        }

        /// <summary>
        /// ImGui Control
        /// </summary>
        /// <param name="imGuiController"></param>
        public override void ImGui(ImGuiController imGuiController)
        {
            base.ImGui(imGuiController);
            
            //Asset browser imgui
            AssetBrowser();
            
            //All gameobjects in scene imgui
            GameobjectHierachyImGui();
            
            //Create Inspector for the currently actively clicked Gameobject
            ActiveGameobjectImGui();
            
            //Uppdate the imgui window for the camera
            SCamera.ImGui(imGuiController);
        }

        private void ActiveGameobjectImGui()
        {
            if(PActiveGameObject != null)
            {
                ImGuiNET.ImGui.Begin("Inspector");

                PActiveGameObject.ImGui_();

                ImGuiNET.ImGui.End();
            }
            
        }

        private void GameobjectHierachyImGui()
        {
            Vector2 itemSpacing;
            float windowX2;

            ImGuiWindowStart("Hierachy", out itemSpacing, out windowX2);
            int id = 0;
            
            for (int i = 0; i < GameObjects.Count; i++)
            {
                ImGuiNET.ImGui.PushID(id);
                bool treeOpen = ImGuiNET.ImGui.TreeNodeEx(
                    GameObjects[i].Name,
                    
                    ImGuiTreeNodeFlags.DefaultOpen| 
                        ImGuiTreeNodeFlags.FramePadding|
                        ImGuiTreeNodeFlags.OpenOnArrow | 
                        ImGuiTreeNodeFlags.SpanAvailWidth,
                    
                    GameObjects[i].Name
                );
                ImGuiNET.ImGui.PopID();
                if (treeOpen)
                {
                    ImGuiNET.ImGui.TreePop();
                }
                id++;
            }
            
            ImGuiNET.ImGui.End();
        }
        
        /// <summary>
        /// Asset Browser ImGui Window
        /// </summary>
        private void AssetBrowser()
        {
            Vector2 itemSpacing;
            float windowX2;

            ImGuiWindowStart("Object Window", out itemSpacing, out windowX2);
            
            ImGuiNET.ImGui.SliderFloat("Asset Size", ref Window.Get().Settings.AssetZoomSize, 1f, 4f);
            
            for (int i = 0; i < sprites.sprites.Count; i++)
            {
                Sprite sprite = sprites.GetSprite(i);
                float spriteWidth = sprite.Width * Window.Get().Settings.AssetZoomSize;
                float spriteHeigth = sprite.Height * Window.Get().Settings.AssetZoomSize;

                int id = sprite.TextureId;
                
                Vector2[] texCoords = sprite.TexCoords;

                ImGuiNET.ImGui.PushID(i);
            
                if (ImGuiNET.ImGui.ImageButton(
                        (IntPtr)id,
                        new Vector2(spriteWidth, spriteHeigth),
                        new Vector2(texCoords[2].X, texCoords[0].Y),
                        new Vector2(texCoords[0].X, texCoords[2].Y)))
                {
                    GameObject obj = Prefabs.GenerateSpriteObject(sprite, 32, 32);
                    var mouseGO = (MouseControls)levelEditorStuff.GetComponent<MouseControls>(typeof(MouseControls));
                    mouseGO.PickupObject(obj);
                }
            
                ImGuiNET.ImGui.PopID();

                Vector2 lastButtonPos = new Vector2();
                lastButtonPos = ImGuiNET.ImGui.GetItemRectMax();
                float lastButtonX2 = lastButtonPos.X;
                float nextButtonX2 = lastButtonX2 + itemSpacing.X + spriteWidth;
            
                if (i + 1 < sprites.sprites.Count && nextButtonX2 < windowX2)
                {
                    ImGuiNET.ImGui.SameLine();
                }
            
            }
            
            ImGuiNET.ImGui.End();
        }
        
        /// <summary>
        /// Default ImGui Window With items
        /// </summary>
        /// <param name="name"></param>
        /// <param name="itemSpacing"></param>
        /// <param name="windowX2"></param>
        private void ImGuiWindowStart(string name, out Vector2 itemSpacing, out float windowX2)
        {
            ImGuiNET.ImGui.Begin(name);

            Vector2 windowPos = new System.Numerics.Vector2();
            windowPos = ImGuiNET.ImGui.GetWindowPos();

            Vector2 windowSize = new Vector2();
            windowSize = ImGuiNET.ImGui.GetWindowSize();

            itemSpacing = new Vector2();
            itemSpacing = ImGuiNET.ImGui.GetStyle().ItemSpacing;

            windowX2 = windowPos.X + windowSize.X;
        }
    }
}