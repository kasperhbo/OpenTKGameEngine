using System;
using ImGuiNET;
using MarioGabeKasper.Editor;
using MarioGabeKasper.Engine.GUI;
using MarioGabeKasper.Engine.Renderer;
using MarioGabeKasper.Engine.Scenes;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MarioGabeKasper.Engine.Core
{
    // This is where all OpenGL code will be written.
    // OpenToolkit allows for several functions to be overriden to extend functionality; this is how we'll be writing code.
    public class Window : GameWindow
    {
        public static Scene _currentScene;

        private float beginTime = (float)GLFW.GetTime();
        private float dt = -1.0f;
        private float endTime;

        private static int width, height;
        private Vector4 clearColor;

        public Scene CurrentScene => _currentScene;
        private ImGuiController _guiController;

        private FrameBuffer _frameBuffer;
        
        private static Window s_window = null;

        public static Window Get()
        {
            if (s_window == null)
            {
                Console.WriteLine("No Window, Initialize");
                var nativeWindowSettings = new NativeWindowSettings
                {
                    Size = new Vector2i(Settings.Width, Settings.Height),
                    Title = Settings.Title,

                    // This is needed to run on macos
                    Flags = ContextFlags.ForwardCompatible
                };

                // To create a new window, create a class that extends GameWindow, then call Run() on it.
                using (s_window = new Window(GameWindowSettings.Default, nativeWindowSettings))
                {
                    s_window.Run();
                }
            }
            
            return s_window;
        }

        /// <summary>
        /// A simple constructor to let us set properties like window size, title, FPS, etc. on the window.
        /// </summary>
        /// <param name="gameWindowSettings"></param>
        /// <param name="nativeWindowSettings"></param>
        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {

        }

        /// <summary>
        /// Load all the assets and settings
        /// </summary>
        protected override void OnLoad()
        {
            base.OnLoad();

            width = ClientSize.X;
            height = ClientSize.Y;

            clearColor = new Vector4(1, 1, 1, 1);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);

            //Loading ImGui
            _guiController = new ImGuiController(Size.X, Size.Y);
            ImGui.LoadIniSettingsFromDisk("../../../imgui/imgui.ini");

            this._frameBuffer = new FrameBuffer(1920, 1080);
            
            
            //Loading imgui font
            //ImGui.GetIO().Fonts.AddFontFromFileTTF("../../../Resources/Fonts/segoeui.ttf", 13);
            //_guiController.RecreateFontDeviceTexture();

            //Open the Level Editor Scene
            ChangeScene(new LevelEditorScene());
        }

        /// <summary>
        /// Render current scene and openGL
        /// </summary>
        /// <param name="args"></param>
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            
            _guiController.Update(this, (float)args.Time);

            this._frameBuffer.Bind();
            
            DebugDraw.BeginFrame();
            GL.ClearColor(clearColor.X, clearColor.Y, clearColor.Z, clearColor.W);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);

            
            //Render scenes
            if (args.Time > 0)
            {
                DebugDraw.Draw();
                _currentScene.Render();
            }
            
            this._frameBuffer.UnBind();
            
            ////Update ImGui
            
            _currentScene.SceneImGui(_guiController);
            
            //Render the gameviewport
            GameViewWindow.Imgui();
            
            _guiController.Render();
            
            ImGuiController.CheckGLError("End of frame");

            //Replacing front with back buffer
            SwapBuffers();
        }


        /// <summary>
        /// Run Update For The Window And For The Current Scene 
        /// </summary>
        /// <param name="FrameEventArgs">Event Arguments</param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // Check if the Escape button is currently being pressed.
            if (KeyboardState.IsKeyDown(Keys.Escape))
                Close();

            endTime = (float)GLFW.GetTime();
            dt = endTime - beginTime;
            beginTime = endTime;

            //Update scenes
            if (dt >= 0) _currentScene.Update(dt, MouseState, KeyboardState);

            base.OnUpdateFrame(e);
        }


        /// <summary>
        /// If Window is Resized Update Open Gl ViewPort
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            // Update the opengl viewport
            GL.Viewport(0, 0, 1920,1080);

            width = ClientSize.X;
            height = ClientSize.Y;

            // Tell ImGui of the new size
            _guiController.WindowResized(ClientSize.X, ClientSize.Y);
        }

        /// <summary>
        /// If text is typed also send it to imgui controller
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextInput(TextInputEventArgs e)
        {
            base.OnTextInput(e);
            _guiController.PressChar((char)e.Unicode);
        }


        /// <summary>
        /// If mouse is scrolled also send it to imgui controller
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            _guiController.MouseScroll(e.Offset);
        }

        /// <summary>
        /// Changing scenes
        /// </summary>
        /// <param name="scene">new scene</param>
        public void ChangeScene(Scene scene)
        {
            _currentScene = scene;
            _currentScene.Load();
            _currentScene.Init(this);
            _currentScene.Start();
        }

        /// <summary>
        /// Get the current scene
        /// </summary>
        /// <returns>m_CurrentScene</returns>
        public static Scene GetScene()
        {
            return _currentScene;
        }

        /// <summary>
        /// Get the width of the viewport
        /// </summary>
        /// <returns>width</returns>
        public static int GetSizeX()
        {
            return width;
        }

        /// <summary>
        /// Get Height of viewport
        /// </summary>
        /// <returns>height</returns>
        public static int GetSizeY()
        {
            return height;
        }

        /// <summary>
        /// If Window is closed from glfw
        /// </summary>
        public override void Close()
        {
            base.Close();
        }

        public FrameBuffer GetFrameBuffer()
        {
            return _frameBuffer;
        }

        public float GetTargetAspectRatio()
        {
            return 16.0f / 9.0f;
        }
    }
}