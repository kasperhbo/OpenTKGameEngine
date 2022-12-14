using MarioGabeKasper.Engine.Components;
using OpenTK.Mathematics;

namespace MarioGabeKasper.Engine.Core;

public class GameCamera : Camera
{
    // public GameCamera(string name, Transform transform) : base(name, transform, false)
    // {
    // }
    public GameCamera(Vector3 position) : base(position)
    {
    }
}