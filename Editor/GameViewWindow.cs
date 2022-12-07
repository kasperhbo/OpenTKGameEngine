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
            Vector2 windowPos = GetCenteredPosForViewport();

            ImGui.SetCursorPos(new Vector2(windowPos.X, windowPos.Y));
            IntPtr textureID = MarioGabeKasper.Engine.Core.Window.GetFrameBuffer().GetTextureID();

            ImGui.Image(textureID, new Vector2(windowSize.X, windowSize.Y), new Vector2(0,1), new Vector2(1,0));                                   

            ImGui.End();
        }

        private static Vector2 GetLargestSizeForViewport()
        {

        }

        private static Vector2 GetCenteredPosForViewport()
        {
            
        }
    }
}
