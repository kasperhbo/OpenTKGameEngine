using System;

namespace MarioGabeKasper.Engine.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public sealed class GuiIgnoreAttribute : Attribute
{
}