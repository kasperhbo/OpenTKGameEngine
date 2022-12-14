using System;
using System.IO;
using ImGuiNET;
using MarioGabeKasper.Editor;
using MarioGabeKasper.Engine.GUI;
using MarioGabeKasper.Engine.Renderer;
using MarioGabeKasper.Engine.Scenes;
using Newtonsoft.Json;
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
        public static Scene CurrentScene { get; private set; }

        private float beginTime = (float)GLFW.GetTime();
        private float dt = -1.0f;
        private float endTime;
        
        public float TargetAspectRatio =>  16.0f / 9.0f;

        private Vector4 clearColor;

        
        private ImGuiController _guiController;
        public Settings Settings;
        
        public FrameBuffer FrameBuffer { get; private set; }

        private static Window _sWindow = null;

        public static int Width { get; private set; }
        public static int Height { get; private set; }

        public static Window Get()
        {
            if (_sWindow == null)
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
                using (_sWindow = new Window(GameWindowSettings.Default, nativeWindowSettings))
                {
                    _sWindow.Run();
                }
            }
            
            return _sWindow;
        }

        /// <summary>
        /// A simple constructor to let us set properties like window size, title, FPS, etc. on the window.
        /// </summary>
        /// <param name="gameWindowSettings"></param>
        /// <param name="nativeWindowSettings"></param>
        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            LoadSettings();
            
            if(!File.Exists("../../../settings.json"))
                Settings = new Settings();
        }

        /// <summary>
        /// Load all the assets and settings
        /// </summary>
        protected override void OnLoad()
        {
            base.OnLoad();

            Width = ClientSize.X;
            Height = ClientSize.Y;

            clearColor = new Vector4(1, 1, 1, 1);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);

            //Loading ImGui
            _guiController = new ImGuiController(Size.X, Size.Y);
            ImGui.LoadIniSettingsFromDisk("../../../imgui/imgui.ini");

            this.FrameBuffer = new FrameBuffer(1920, 1080);
            
            Input.Initialize(this);
            
            //Open the Level Editor Scene
            ChangeScene(new LevelEditorScene());
        }

        /// <summary>
        /// Render current scene and openGL
        /// </summary>
        /// <param name="args"></param>
        ///
        private GameViewWindow _gameViewWindow = new GameViewWindow();
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            
            _guiController.Update(this, (float)args.Time);

            #region Frame Buffer
            this.FrameBuffer.Bind();
            
            DebugDraw.BeginFrame();
            
            GL.ClearColor(clearColor.X, clearColor.Y, clearColor.Z, clearColor.W);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
            
            //Render scenes
            if (args.Time > 0)
            {
                DebugDraw.Draw();
                CurrentScene.Render();
            }
            
            this.FrameBuffer.UnBind();
            #endregion
            
            ImGui.ShowDemoWindow();

            CurrentScene.SceneImGui(_guiController);
            _gameViewWindow.Imgui();
            _guiController.Render();
            
            ImGuiController.CheckGlError("End of frame");   
            
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

            Input.Update();
            
            endTime = (float)GLFW.GetTime();
            dt = endTime - beginTime;
            beginTime = endTime;

            //Update scenes
            if (dt >= 0) CurrentScene.Update(dt);

            base.OnUpdateFrame(e);
        }


        /// <summary>
        /// If Window is Resized Update Open Gl ViewPort
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            Width = ClientSize.X;
            Height = ClientSize.Y;

            // Update the opengl viewport
            GL.Viewport(0, 0, Width,Height);


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
            CurrentScene = scene;
            CurrentScene.Load();
            CurrentScene.Init(this);
            CurrentScene.Start();
        }

        public void LoadSettings()
        {
            if (File.Exists("../../../settings.json"))
            {
                Settings settings = JsonConvert.DeserializeObject <Settings>(File.ReadAllText("../../../settings.json"));
                Settings = settings;
            }
        }

        public void SaveSettings()
        {
            string settingsData = JsonConvert.SerializeObject(Settings);
            File.WriteAllText("../../../settings.json", settingsData);

        }

    }
}