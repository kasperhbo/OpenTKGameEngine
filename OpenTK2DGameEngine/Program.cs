using System;
using System.Text.Json;
using ImGuiNET;
using MarioGabeKasper.Engine;
using MarioGabeKasper.Engine.Core;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace MarioGabeKasper
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Window.Get();
            
            //TODO: Find a better place to put this
            ImGui.SaveIniSettingsToDisk("../../../imgui/imgui.ini");
            Window.GetScene().Save();
        }
    }
}