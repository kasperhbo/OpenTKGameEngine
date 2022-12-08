using ImGuiNET;
using MarioGabeKasper.Engine.Core;
using System;
using System.Numerics;

namespace MarioGabeKasper.Editor
{
    public static class GameViewWindow
    {
        public static void Imgui()
        {
            ImGui.Begin("Game Viewport", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse);
            
            Vector2 windowSize = GetLargestSizeForViewport();
            Vector2 windowPos = GetCenteredPosForViewport(windowSize);
            ImGui.SetCursorPos(new Vector2(windowPos.X, windowPos.Y));
            IntPtr textureId = new IntPtr(Window.Get().GetFrameBuffer().GetTextureId());
            ImGui.Image(textureId, new Vector2(windowSize.X, windowSize.Y), new Vector2(0,1), new Vector2(1,0));
            
            ImGui.End();
        }

        private static Vector2 GetLargestSizeForViewport()
        {
            Vector2 windowSize = new Vector2();
            windowSize = ImGui.GetContentRegionAvail();
            
            windowSize.X -= ImGui.GetScrollX();
            windowSize.Y -= ImGui.GetScrollY();

            float aspectWidth = windowSize.X;
            float aspectHeigth = aspectWidth / Window.Get().GetTargetAspectRatio();
            
            if (aspectHeigth > windowSize.X)
            {
                aspectHeigth = windowSize.Y;
                aspectWidth = aspectHeigth * Window.Get().GetTargetAspectRatio();
            }

            return new Vector2(aspectWidth, aspectHeigth);
        }
        
        private static Vector2 GetCenteredPosForViewport(Vector2 aspectSize)
        {
            Vector2 windowSize = new Vector2();
            windowSize = ImGui.GetContentRegionAvail();
            
            windowSize.X -= ImGui.GetScrollX();
            windowSize.Y -= ImGui.GetScrollY();

            float viewPortX = (windowSize.X / 2.0F) - (aspectSize.X / 2.0F);
            float viewPortY = (windowSize.Y / 2.0F) - (aspectSize.Y / 2.0F);

            return new Vector2(viewPortX + ImGui.GetCursorPosX(), viewPortY+ ImGui.GetCursorPosY());
        }
    }
}
