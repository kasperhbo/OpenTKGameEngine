using System.Numerics;

namespace MarioGabeKasper.Engine.Utils;

public static class Math
{
    public static Vector2 OpenTkToDefault(OpenTK.Mathematics.Vector2 val)
    {
        return new Vector2(val.X, val.Y);
    }
    
    public static  OpenTK.Mathematics.Vector2 DefaultToOpenTk(Vector2 val)
    {
        return new OpenTK.Mathematics.Vector2(val.X, val.Y);
    }
}