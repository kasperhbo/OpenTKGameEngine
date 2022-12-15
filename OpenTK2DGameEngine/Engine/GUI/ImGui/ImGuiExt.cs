using System;
using ImGuiNET;

namespace MarioGabeKasper.Engine.GUI.ImGuiExtras;


public static class ImGuiExtension
{
    public static unsafe bool IsValid(this ImGuiPayloadPtr payload)
    {
        return payload.NativePtr != null;
    }
}