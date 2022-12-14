using System;
using ImGuiNET;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MarioGabeKasper.Engine.GUI;

public class ImGuiLayer
{
    private unsafe Window* glfwWindow;

    private long[] mouseCursors = new long[(int)ImGuiMouseCursor.COUNT];

    public unsafe ImGuiLayer(Window* glfwWindow)
    {
        this.glfwWindow = glfwWindow;
    }

    public void InitImGui()
    {
        // IMPORTANT!!
        // This line is critical for Dear ImGui to work.
        ImGui.CreateContext();
        
        // ------------------------------------------------------------
        // Initialize ImGuiIO config
        ImGuiIOPtr io = ImGui.GetIO();

        io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;
        io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
        io.BackendFlags |= ImGuiBackendFlags.HasMouseCursors;
        io.WantSaveIniSettings = true;     
        
        SetKeyMappings();
        
    }

    private static void SetKeyMappings()
    {
        ImGuiIOPtr io = ImGui.GetIO();
        
        io.KeyMap[(int)ImGuiKey.Tab] = (int)Keys.Tab;
        io.KeyMap[(int)ImGuiKey.LeftArrow] = (int)Keys.Left;
        io.KeyMap[(int)ImGuiKey.RightArrow] = (int)Keys.Right;
        io.KeyMap[(int)ImGuiKey.UpArrow] = (int)Keys.Up;
        io.KeyMap[(int)ImGuiKey.DownArrow] = (int)Keys.Down;
        io.KeyMap[(int)ImGuiKey.PageUp] = (int)Keys.PageUp;
        io.KeyMap[(int)ImGuiKey.PageDown] = (int)Keys.PageDown;
        io.KeyMap[(int)ImGuiKey.Home] = (int)Keys.Home;
        io.KeyMap[(int)ImGuiKey.End] = (int)Keys.End;
        io.KeyMap[(int)ImGuiKey.Delete] = (int)Keys.Delete;
        io.KeyMap[(int)ImGuiKey.Backspace] = (int)Keys.Backspace;
        io.KeyMap[(int)ImGuiKey.Enter] = (int)Keys.Enter;
        io.KeyMap[(int)ImGuiKey.Escape] = (int)Keys.Escape;
        io.KeyMap[(int)ImGuiKey.A] = (int)Keys.A;
        io.KeyMap[(int)ImGuiKey.C] = (int)Keys.C;
        io.KeyMap[(int)ImGuiKey.V] = (int)Keys.V;
        io.KeyMap[(int)ImGuiKey.X] = (int)Keys.X;
        io.KeyMap[(int)ImGuiKey.Y] = (int)Keys.Y;
        io.KeyMap[(int)ImGuiKey.Z] = (int)Keys.Z;
    }

    private static void SetMouseMappings()
    {
        ImGuiIOPtr io = ImGui.GetIO();
        
    }
    
}