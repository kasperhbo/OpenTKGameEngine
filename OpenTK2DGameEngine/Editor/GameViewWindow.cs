﻿using ImGuiNET;
using MarioGabeKasper.Engine.Core;
using System;
using System.Numerics;
using MarioGabeKasper.Engine;

namespace MarioGabeKasper.Editor
{
    public class GameViewWindow
    {
        private static bool isPlaying = false;

        private static float leftX, rightX, topY, bottomY;
        
        public void Imgui()
        {
            if (ImGui.BeginMenuBar())
                                    {
                                        if (ImGui.BeginMenu("File"))
                                        {
                                            ImGui.MenuItem("New Project");
                                            ImGui.MenuItem("Load Project");
                                            ImGui.MenuItem("Save Scene");
                                            ImGui.EndMenu();
                                        }
                                        ImGui.EndMenuBar();
                                    }
                                    
                                    ImGui.Begin("Game Viewport", 
                                        ImGuiWindowFlags.NoScrollbar 
                                        | ImGuiWindowFlags.NoScrollWithMouse 
                                        | ImGuiWindowFlags.MenuBar 
                                        | ImGuiWindowFlags.NoResize
                                        | ImGuiWindowFlags.NoCollapse
                                        | ImGuiWindowFlags.NoTitleBar);
                        
                                    if(ImGui.BeginMenuBar()){
                                        if (ImGui.MenuItem("Play", "", isPlaying, !isPlaying))
                                        {
                                            
                                        }
                                        if (ImGui.MenuItem("Stop", "", !isPlaying, isPlaying))
                                        {
                                            
                                        }
                                        ImGui.SetNextItemWidth(300 + 2);
                                        
                                        
                                        ImGuiNET.ImGui.SliderFloat("Camera multiplier ", ref Window.Get().Settings.SceneCameraSpeedMultiplier, 1F, 8F);
                                        
                                        ImGui.EndMenuBar();
                                    }
                                    
                                    Vector2 windowSize = GetLargestSizeForViewport();
                                    Vector2 windowPos = GetCenteredPosForViewport(windowSize);
                                    ImGui.SetCursorPos(new Vector2(windowPos.X, windowPos.Y));
                        
                                    Vector2 topLeft = ImGui.GetCursorScreenPos();
                                    topLeft.X -= ImGui.GetScrollX();
                                    topLeft.Y -= ImGui.GetScrollY();
                                    
                                    leftX = topLeft.X;
                                    bottomY = topLeft.Y;
                                    rightX = topLeft.X + windowSize.X;
                                    topY = topLeft.Y + windowSize.Y;
                        
                                    IntPtr textureId = new IntPtr(Window.Get().FrameBuffer.TextureID);
                                    ImGui.Image(textureId, new Vector2(windowSize.X, windowSize.Y), new Vector2(0,1), new Vector2(1,0));
                        
                                    ImGui.End();
        }

        public static bool GetWantCaptureMouse()
        {
            // return KeyboardInput.MouseX >= leftX && KeyboardInput.MouseX <= rightX &&
            //        KeyboardInput.MouseY >= bottomY && KeyboardInput.MouseY <= topY;
            return true;

        }

        public void DrawSlider(int x, int y, int width, int height, float min, float max, ref float value, string label = "")
        {
            ImGui.SetNextItemWidth(width + 2);
            var style = ImGui.GetStyle();
            var framePadding = style.FramePadding;
            style.FramePadding = new Vector2(0, (height - ImGui.GetFontSize() + 1) * 0.5f);
            ImGui.SliderFloat(label, ref value, min, max, "");
            style.FramePadding = framePadding;
        }
        
        private static Vector2 GetLargestSizeForViewport()
        {
            Vector2 windowSize = new Vector2();
            windowSize = ImGui.GetContentRegionAvail();
            
            windowSize.X -= ImGui.GetScrollX();
            windowSize.Y -= ImGui.GetScrollY();

            float aspectWidth = windowSize.X;
            float aspectHeigth = aspectWidth / Window.Get().TargetAspectRatio;
            
            if (aspectHeigth > windowSize.X)
            {
                aspectHeigth = windowSize.Y;
                aspectWidth = aspectHeigth * Window.Get().TargetAspectRatio;
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
