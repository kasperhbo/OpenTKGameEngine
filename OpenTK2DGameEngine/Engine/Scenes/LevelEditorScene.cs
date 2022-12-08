using System;
using System.Reflection;
using MarioGabeKasper.Engine.Components;
using MarioGabeKasper.Engine.Components.Colliders;
using MarioGabeKasper.Engine.Core;
using MarioGabeKasper.Engine.GUI;
using MarioGabeKasper.Engine.Utils;
using OpenTK.Windowing.Desktop;
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

            SCamera = new SceneCamera("Scene Camera", new Transform(
                new Vector2(-250,0),new Vector2(0,0)));
            
            sprites = AssetPool.GetSpriteSheet("decorationsandblocks.png");
            
            levelEditorStuff.AddComponent(new MouseControls());
            levelEditorStuff.AddComponent(new GridLines());
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

        private float t = 0;
        
        /// <summary>
        /// Update that runs every frame
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="mouseState"></param>
        /// <param name="input"></param>
        public override void Update(float dt, MouseState mouseState, KeyboardState input)
        {
            levelEditorStuff.Update(dt);
            
            foreach (var go in GameObjects) go.Update(dt);

            MouseState = mouseState;

            UpdateMouseCallbacks(dt);

            if (input.IsKeyDown(Keys.Left))
            {
                Window.Get().CurrentScene.GetCamera().Transform.Position.X -= 1 * dt;
            }

            if (mouseState.IsButtonDown(MouseButton.Right))
            {
                if(MouseListener.GetIsDragging())
                {
                    if (MouseListener.GetDx() > 3)
                    {
                        Window.Get().CurrentScene.GetCamera().Transform.Position.X += 100 * dt;
                    }
                    if (MouseListener.GetDx() < -3)
                    {
                        Window.Get().CurrentScene.GetCamera().Transform.Position.X -= 100 * dt;
                    }
                    if (MouseListener.GetDy() > 3)
                    {
                        Window.Get().CurrentScene.GetCamera().Transform.Position.Y += 100 * dt;
                    }
                    if (MouseListener.GetDy() < -3)
                    {
                        Window.Get().CurrentScene.GetCamera().Transform.Position.Y -= 100 * dt;
                    }
                    
                    //Make sure mouse pops at other side of the screen when reached the screen border
                    if (MouseListener.GetX() >= Window.Get().ClientSize.X - 10)
                    {
                        Window.Get().MousePosition = new OpenTK.Mathematics.Vector2(0 + 11, Window.Get().MousePosition.Y);
                    }
                    if (MouseListener.GetX() <= 10)
                    {
                        Window.Get().MousePosition = new OpenTK.Mathematics.Vector2(Window.Get().ClientSize.X - 11, Window.Get().MousePosition.Y);
                    }
                }
            }

            OpenTK.Mathematics.Vector2 mousePos = (MouseListener.GetX(), MouseListener.GetY());
            foreach (GameObject gameObject in GameObjects)
            {
                Transform trans = gameObject.Transform;

                Vector2 pos = trans.Position;
                Vector2 scale = trans.Scale;
                
                if (CollisionCheck.PointInBox(pos.X, pos.Y, pos.X + scale.X, pos.Y + scale.Y, MouseListener.GetOrthoX(), MouseListener.GetOrthoY()))
                {
                    if (mouseState.IsButtonPressed(MouseButton.Left))
                        PActiveGameObject = gameObject;
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
            SceneImGui();
            AssetBrowser();
        }

        private void SceneImGui()
        {
            ImGuiNET.ImGui.Begin("Scene Settings");
            ImGuiNET.ImGui.DragFloat("Camera multiplier ", ref Window.Get().Settings.SceneCameraSpeedMultiplier);
            ImGuiNET.ImGui.DragFloat2("Camera multiplier ", ref Window.Get().Settings.GridSize, 16, 16);
            // ImGuiNET.ImGui.Dr("Camera multiplier ", ref Window.Get().Settings.SceneCameraSpeedMultiplier);
            ImGuiNET.ImGui.End();

        }

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
                    // levelEditorStuff._mouseControls.PickupObject(obj);

                    var mouseGo = (MouseControls)levelEditorStuff.GetComponent<MouseControls>(typeof(MouseControls));
                    mouseGo.PickupObject(obj);
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
            
            ImGuiEnd();
        }
        
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

        private void ImGuiEnd()
        {
            ImGuiNET.ImGui.End();
        }

        private void UpdateMouseCallbacks(float dt)
        {
            if(GetCurrentMouseDown() != -1)
            {
                MouseListener.MouseButtonCallback(GetCurrentMouseDown(), MouseState);
            }
        
            MouseListener.MouseScrollCallback(MouseState.Scroll.X, MouseState.Scroll.Y);
            MouseListener.MousePosCallback(MouseState.X, MouseState.Y);
        }

    }
}