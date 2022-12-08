using System;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MarioGabeKasper.Engine.Core
{
    public class MouseListener
    {
        private static MouseListener _instance;
        private double scrollX, scrollY;
        private double xPos, yPos, lastY, lastX;
        private bool[] mouseButtonPressed = new bool[9];
        private bool isDragging;

        private MouseListener()
        {
            scrollX = 0.0;
            scrollY = 0.0;
            xPos = 0.0;
            yPos = 0.0;
            lastX = 0.0;
            lastY = 0.0;
        }

        public static MouseListener Get()
        {
            if (_instance == null)
            {
                _instance = new MouseListener();
            }

            return _instance;
        }

        public static void MousePosCallback(double xpos, double ypos)
        {
            Get().lastX = Get().xPos;
            Get().lastY = Get().yPos;
            Get().xPos = xpos;
            Get().yPos = ypos;
            Get().isDragging = Get().mouseButtonPressed[0] || Get().mouseButtonPressed[1] || Get().mouseButtonPressed[2];
        }


        public static void MouseButtonCallback(int button, MouseState mouseState)
        {
            // MouseState mouseState = Window.GetMouseState();

            if (mouseState.IsAnyButtonDown)
            {
                if (button < Get().mouseButtonPressed.Length)
                {
                    Get().mouseButtonPressed[button] = true;
                }
            }
            else if (
                mouseState.IsButtonReleased(MouseButton.Button1) || mouseState.IsButtonReleased(MouseButton.Button2) ||
                mouseState.IsButtonReleased(MouseButton.Button3) || mouseState.IsButtonReleased(MouseButton.Button4) ||
                mouseState.IsButtonReleased(MouseButton.Button5) || mouseState.IsButtonReleased(MouseButton.Button6) ||
                mouseState.IsButtonReleased(MouseButton.Button7) || mouseState.IsButtonReleased(MouseButton.Button8))
            {
                if (button < Get().mouseButtonPressed.Length && button != -1)
                {
                    Get().mouseButtonPressed[button] = false;
                    Get().isDragging = false;
                }
            }
        }

        public static void MouseScrollCallback(double xOffset, double yOffset)
        {
            Get().scrollX = xOffset;
            Get().scrollY = yOffset;
        }

        public static float GetX()
        {
            return (float)Get().xPos;
        }

        public static float GetY()
        {
            return (float)Get().yPos;
        }

        public static float GetOrthoX()
        {
            // MouseState mouseState = Window.GetMouseState();

            float currentX = GetX();

            currentX = currentX / Window.GetSizeX() * 2f - 1f;

            OpenTK.Mathematics.Vector4 tmp = new OpenTK.Mathematics.Vector4(currentX, 0, 0, 1);

            tmp = tmp * Window.GetScene().GetCamera().GetInverseProjection() * Window.GetScene().GetCamera().GetInverseView();
            currentX = tmp.X;

            return currentX;
        }

        public static float GetOrthoY()
        {
            float currentY = Window.GetSizeY() - GetY();

            currentY = currentY / Window.GetSizeY() * 2f - 1f;

            OpenTK.Mathematics.Vector4 tmp = new OpenTK.Mathematics.Vector4(0, currentY, 0, 1);

            tmp = tmp * Window.GetScene().GetCamera().GetInverseProjection() * Window.GetScene().GetCamera().GetInverseView();
            currentY = tmp.Y;

            return currentY;
        }

        public static float GetDx()
        {
            return (float)(Get().lastX - Get().xPos);
        }

        public static float GetDy()
        {
            return (float)(Get().lastY - Get().yPos);
        }

        public static float GetScrollX()
        {
            return (float)Get().scrollX;
        }

        public static float GetScrollY()
        {
            return (float)Get().scrollY;
        }

        public static bool GetIsDragging()
        {
            return Get().isDragging;
        }

        public static bool MouseButtonDown(int button)
        {
            if (button < Get().mouseButtonPressed.Length && button != -1)
            {
                return Get().mouseButtonPressed[button];
            }
            else
            {
                return false;
            }
        }

    }
}