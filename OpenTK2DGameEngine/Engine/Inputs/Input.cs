using System;
using System.Collections.Generic;
using MarioGabeKasper.Engine.Core;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Vector2 = System.Numerics.Vector2;
using Window = MarioGabeKasper.Engine.Core.Window;


namespace MarioGabeKasper.Engine;

public class Input
{
    private static List<Keys> keysDown;
    private static List<Keys> keysDownLast;
    private static List<MouseButton> buttonsDown;
    private static List<MouseButton> buttonsDownLast;
    private static GameWindow glfwWindow;
        
    public static Vector2 MousePosition { get; private set; }
    public static float ScrollX {get; private set;}
    public static float ScrollY {get; private set;}

    public static float OrthoX()
    {
        float currentX = MousePosition.X;
        
        currentX = (currentX / (float)glfwWindow.Size.X) * 2f - 1f;
        
        OpenTK.Mathematics.Vector4 tmp = new OpenTK.Mathematics.Vector4(currentX, 0, 0, 1);
        
        tmp = (tmp * Window.CurrentScene.SCamera.GetInverseProjection()) * Window.CurrentScene.SCamera.GetInverseView();
        currentX = tmp.X;
            
        return currentX;
    }
    
    public static float OrthoY()
    {
        float currentY = Window.Get().Size.Y - MousePosition.Y;
        
        currentY = (currentY / Window.Get().Size.Y * 2f) - 1f;
        
        OpenTK.Mathematics.Vector4 tmp = new OpenTK.Mathematics.Vector4(0, currentY, 0, 1);
        
        tmp = (tmp * Window.CurrentScene.SCamera.GetInverseProjection()) * Window.CurrentScene.SCamera.GetInverseView();
        currentY = tmp.Y;
            
        return currentY;
    }
    
    public static void Initialize(GameWindow game)
    {
        keysDown = new List<Keys>();
        keysDownLast = new List<Keys>();
        buttonsDown = new List<MouseButton>();
        buttonsDownLast = new List<MouseButton>();

        CurrentKeyDown = -2;
        
        glfwWindow = game;

        game.MouseDown += game_MouseDown;
        game.MouseUp += game_MouseUp;
        game.KeyDown += game_KeyDown;
        game.KeyUp += game_KeyUp;

        game.MouseMove += args =>
        {
            MousePosition = new Vector2(args.X, args.Y);
        };

        game.MouseWheel += args =>
        {
            ScrollX = args.OffsetX;
            ScrollY = args.OffsetY;
        };

    }

    public static int CurrentKeyDown { get; private set; }

    static void game_KeyDown(KeyboardKeyEventArgs e)
    {
        CurrentKeyDown = (int)e.Key;
        if (!keysDown.Contains(e.Key))
            keysDown.Add(e.Key);
    }
    static void game_KeyUp(KeyboardKeyEventArgs e)
    {
        CurrentKeyDown = -2;
        while(keysDown.Contains(e.Key))
            keysDown.Remove(e.Key);
    }
    static void game_MouseDown(MouseButtonEventArgs e)
    {
        if (!buttonsDown.Contains(e.Button))
            buttonsDown.Add(e.Button);
    }
    static void game_MouseUp(MouseButtonEventArgs e)
    {
        while (buttonsDown.Contains(e.Button))
            buttonsDown.Remove(e.Button);
    }
    public static void Update()
    {
        keysDownLast = new List<Keys>(keysDown);
        buttonsDownLast = new List<MouseButton>(buttonsDown);
    }

    public static bool KeyPress(Keys key)
    {
        return (keysDown.Contains(key) && !keysDownLast.Contains(key));
    }
    public static bool KeyRelease(Keys key)
    {
        return (!keysDown.Contains(key) && keysDownLast.Contains(key));
    }
    public static bool KeyDown(Keys key)
    {
        return (keysDown.Contains(key));
    }

    public static bool MousePress(MouseButton button)
    {
        return (buttonsDown.Contains(button) && !buttonsDownLast.Contains(button));
    }
    public static bool MouseRelease(MouseButton button)
    {
        return (!buttonsDown.Contains(button) && buttonsDownLast.Contains(button));
    }
    public static bool MouseDown(MouseButton button)
    {
        return (buttonsDown.Contains(button));
    }
}   