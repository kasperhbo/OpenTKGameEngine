using System.Numerics;
using ImGuiNET;

namespace MarioGabeKasper.Engine.GUI;

public static class StyleGrey
{
    public static void GetStyle(){
        ImGuiStylePtr style = ImGui.GetStyle();

        var colors = ImGui.GetStyle().Colors;
            
        colors[(int)ImGuiCol.Text]                  = new Vector4(1.00f, 1.00f, 1.00f, 1.00f);
        colors[(int)ImGuiCol.TextDisabled]          = new Vector4(0.50f, 0.50f, 0.50f, 1.00f);
        colors[(int)ImGuiCol.WindowBg]              = new Vector4(0.13f, 0.14f, 0.15f, 1.00f);
        colors[(int)ImGuiCol.ChildBg]               = new Vector4(0.13f, 0.14f, 0.15f, 1.00f);
        colors[(int)ImGuiCol.PopupBg]               = new Vector4(0.13f, 0.14f, 0.15f, 1.00f);
        colors[(int)ImGuiCol.Border]                = new Vector4(0.43f, 0.43f, 0.50f, 0.50f);
        colors[(int)ImGuiCol.BorderShadow]          = new Vector4(0.00f, 0.00f, 0.00f, 0.00f);
        colors[(int)ImGuiCol.FrameBg]               = new Vector4(0.25f, 0.25f, 0.25f, 1.00f);
        colors[(int)ImGuiCol.FrameBgHovered]        = new Vector4(0.38f, 0.38f, 0.38f, 1.00f);
        colors[(int)ImGuiCol.FrameBgActive]         = new Vector4(0.67f, 0.67f, 0.67f, 0.39f);
        colors[(int)ImGuiCol.TitleBg]               = new Vector4(0.08f, 0.08f, 0.09f, 1.00f);
        colors[(int)ImGuiCol.TitleBgActive]         = new Vector4(0.08f, 0.08f, 0.09f, 1.00f);
        colors[(int)ImGuiCol.TitleBgCollapsed]      = new Vector4(0.00f, 0.00f, 0.00f, 0.51f);
        colors[(int)ImGuiCol.MenuBarBg]             = new Vector4(0.14f, 0.14f, 0.14f, 1.00f);
        colors[(int)ImGuiCol.ScrollbarBg]           = new Vector4(0.02f, 0.02f, 0.02f, 0.53f);
        colors[(int)ImGuiCol.ScrollbarGrab]         = new Vector4(0.31f, 0.31f, 0.31f, 1.00f);
        colors[(int)ImGuiCol.ScrollbarGrabHovered]  = new Vector4(0.41f, 0.41f, 0.41f, 1.00f);
        colors[(int)ImGuiCol.ScrollbarGrabActive]   = new Vector4(0.51f, 0.51f, 0.51f, 1.00f);
        colors[(int)ImGuiCol.CheckMark]             = new Vector4(0.11f, 0.64f, 0.92f, 1.00f);
        colors[(int)ImGuiCol.SliderGrab]            = new Vector4(0.11f, 0.64f, 0.92f, 1.00f);
        colors[(int)ImGuiCol.SliderGrabActive]      = new Vector4(0.08f, 0.50f, 0.72f, 1.00f);
        colors[(int)ImGuiCol.Button]                = new Vector4(0.25f, 0.25f, 0.25f, 1.00f);
        colors[(int)ImGuiCol.ButtonHovered]         = new Vector4(0.38f, 0.38f, 0.38f, 1.00f);
        colors[(int)ImGuiCol.ButtonActive]          = new Vector4(0.67f, 0.67f, 0.67f, 0.39f);
        colors[(int)ImGuiCol.Header]                = new Vector4(0.22f, 0.22f, 0.22f, 1.00f);
        colors[(int)ImGuiCol.HeaderHovered]         = new Vector4(0.25f, 0.25f, 0.25f, 1.00f);
        colors[(int)ImGuiCol.HeaderActive]          = new Vector4(0.67f, 0.67f, 0.67f, 0.39f);
        colors[(int)ImGuiCol.SeparatorHovered]      = new Vector4(0.41f, 0.42f, 0.44f, 1.00f);
        colors[(int)ImGuiCol.SeparatorActive]       = new Vector4(0.26f, 0.59f, 0.98f, 0.95f);
        colors[(int)ImGuiCol.ResizeGrip]            = new Vector4(0.00f, 0.00f, 0.00f, 0.00f);
        colors[(int)ImGuiCol.ResizeGripHovered]     = new Vector4(0.29f, 0.30f, 0.31f, 0.67f);
        colors[(int)ImGuiCol.ResizeGripActive]      = new Vector4(0.26f, 0.59f, 0.98f, 0.95f);
        colors[(int)ImGuiCol.Tab]                   = new Vector4(0.08f, 0.08f, 0.09f, 0.83f);
        colors[(int)ImGuiCol.TabHovered]            = new Vector4(0.33f, 0.34f, 0.36f, 0.83f);
        colors[(int)ImGuiCol.TabActive]             = new Vector4(0.23f, 0.23f, 0.24f, 1.00f);
        colors[(int)ImGuiCol.TabUnfocused]          = new Vector4(0.08f, 0.08f, 0.09f, 1.00f);
        colors[(int)ImGuiCol.TabUnfocusedActive]    = new Vector4(0.13f, 0.14f, 0.15f, 1.00f);
        colors[(int)ImGuiCol.DockingPreview]        = new Vector4(0.26f, 0.59f, 0.98f, 0.70f);
        colors[(int)ImGuiCol.DockingEmptyBg]        = new Vector4(0.20f, 0.20f, 0.20f, 1.00f);
        colors[(int)ImGuiCol.PlotLines]             = new Vector4(0.61f, 0.61f, 0.61f, 1.00f);
        colors[(int)ImGuiCol.PlotLinesHovered]      = new Vector4(1.00f, 0.43f, 0.35f, 1.00f);
        colors[(int)ImGuiCol.PlotHistogram]         = new Vector4(0.90f, 0.70f, 0.00f, 1.00f);
        colors[(int)ImGuiCol.PlotHistogramHovered]  = new Vector4(1.00f, 0.60f, 0.00f, 1.00f);
        colors[(int)ImGuiCol.TextSelectedBg]        = new Vector4(0.26f, 0.59f, 0.98f, 0.35f);
        colors[(int)ImGuiCol.DragDropTarget]        = new Vector4(0.11f, 0.64f, 0.92f, 1.00f);
        colors[(int)ImGuiCol.NavHighlight]          = new Vector4(0.26f, 0.59f, 0.98f, 1.00f);
        colors[(int)ImGuiCol.NavWindowingHighlight] = new Vector4(1.00f, 1.00f, 1.00f, 0.70f);
        colors[(int)ImGuiCol.NavWindowingDimBg]     = new Vector4(0.80f, 0.80f, 0.80f, 0.20f);
        colors[(int)ImGuiCol.ModalWindowDimBg]      = new Vector4(0.80f, 0.80f, 0.80f, 0.35f);
        
        
        // colors[(int)ImGuiCol.Separator]             = new style.Colors[ImGuiCol_Border];
        colors[(int)ImGuiCol.Separator]             = colors[(int)ImGuiCol.Border];
        
        style.GrabRounding                           = style.FrameRounding = 2.3f;
    }
}