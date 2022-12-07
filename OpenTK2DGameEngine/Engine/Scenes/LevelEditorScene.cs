using System;

using MarioGabeKasper.Engine.Components;
using MarioGabeKasper.Engine.Components.Colliders;
using MarioGabeKasper.Engine.Core;
using MarioGabeKasper.Engine.GUI;
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

            LoadResources();

            s_camera = new Camera(new Vector3(-250, 0, 0));

            sprites = AssetPool.GetSpriteSheet("decorationsandblocks.png");
            
            levelEditorStuff.AddComponent(new MouseControls());
            levelEditorStuff.AddComponent(new GridLines());


            //p_activeGameObject = GameObjects[0];
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

            _mouseState = mouseState;

            UpdateMouseCallbacks(dt);

            if (input.IsKeyDown(Keys.Left))
            {
                Window._currentScene.GetCamera().position.X -= 1 * dt;
            }

            if (mouseState.IsButtonDown(MouseButton.Right))
            {
                if(MouseListener.GetIsDragging())
                {
                    if (MouseListener.getDx() > 3)
                    {
                        Window._currentScene.GetCamera().position.X += 100 * dt;
                    }
                    if (MouseListener.getDx() < -3)
                    {
                        Window._currentScene.GetCamera().position.X -= 100 * dt;
                    }
                    if (MouseListener.getDy() > 3)
                    {
                        Window._currentScene.GetCamera().position.Y += 100 * dt;
                    }
                    if (MouseListener.getDy() < -3)
                    {
                        Window._currentScene.GetCamera().position.Y -= 100 * dt;
                    }
                }
            }

            OpenTK.Mathematics.Vector2 mousePos = (MouseListener.getX(), MouseListener.getY());
            foreach (GameObject gameObject in GameObjects)
            {
                Transform trans = gameObject.transform;

                Vector2 pos = trans.Position;
                Vector2 scale = trans.Scale;
                
                if (CollisionCheck.PointInBox(pos.X, pos.Y, pos.X + scale.X, pos.Y + scale.Y, MouseListener.GetOrthoX(), MouseListener.GetOrthoY()))
                {
                    if (mouseState.IsButtonPressed(MouseButton.Left))
                        p_activeGameObject = gameObject;
                }
            }

        }

        /// <summary>
        /// Render the renderer
        /// </summary>
        public override void Render()
        {
            _renderer.Render();
        }

        /// <summary>
        /// ImGui Control
        /// </summary>
        /// <param name="imGuiController"></param>
        public override void ImGui(ImGuiController imGuiController)
        {
            base.ImGui(imGuiController);

            Vector2 itemSpacing;
            float windowX2;

            ImGuiWindowStart("Object Window", out itemSpacing, out windowX2);
            for (int i = 0; i < sprites.GetSize(); i++)
            {
                Sprite sprite = sprites.GetSprite(i);
                float spriteWidth = sprite.GetWidth() * 4;
                float spriteHeigth = sprite.GetHeight() * 4;
                
                int id = sprite.GetTexID();
                
                Vector2[] texCoords = sprite.GetTexCoords();

                ImGuiNET.ImGui.PushID(i);
            
                if (ImGuiNET.ImGui.ImageButton(
                        (IntPtr)id,
                        new Vector2(spriteWidth, spriteHeigth),
                        new Vector2(texCoords[2].X, texCoords[0].Y),
                        new Vector2(texCoords[0].X, texCoords[2].Y)))
                {
                    GameObject obj = Prefabs.GenerateSpriteObject(sprite, 32, 32);
                    // levelEditorStuff._mouseControls.PickupObject(obj);

                    var mouseGO = (MouseControls)levelEditorStuff.GetComponent<MouseControls>(typeof(MouseControls));
                    mouseGO.PickupObject(obj);
                }
            
                ImGuiNET.ImGui.PopID();
            
            
                Vector2 lastButtonPos = new Vector2();
                lastButtonPos = ImGuiNET.ImGui.GetItemRectMax();
                float lastButtonX2 = lastButtonPos.X;
                float nextButtonX2 = lastButtonX2 + itemSpacing.X + spriteWidth;
            
                if (i + 1 < sprites.GetSize() && nextButtonX2 < windowX2)
                {
                    ImGuiNET.ImGui.SameLine();
                }
            
            }
            ImGuiEnd();

        }
        
        private void ImGuiWindowStart(string Name, out Vector2 itemSpacing, out float windowX2)
        {
            ImGuiNET.ImGui.Begin(Name);

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
                MouseListener.mouseButtonCallback(GetCurrentMouseDown(), _mouseState);
            }
        
            MouseListener.MouseScrollCallback(_mouseState.Scroll.X, _mouseState.Scroll.Y);
            MouseListener.mousePosCallback(_mouseState.X, _mouseState.Y);
        }

    }
}