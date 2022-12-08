using MarioGabeKasper.Engine.Components;
using OpenTK.Mathematics;

namespace MarioGabeKasper.Engine.Core;

public class SceneCamera : Camera
{
    public SceneCamera(string name, Transform transform) : base(name, transform, true)
    {
    }
}