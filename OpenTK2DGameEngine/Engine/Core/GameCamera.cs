using MarioGabeKasper.Engine.Components;

namespace MarioGabeKasper.Engine.Core;

public class GameCamera : Camera
{
    public GameCamera(string name, Transform transform) : base(name, transform, false)
    {
    }
}