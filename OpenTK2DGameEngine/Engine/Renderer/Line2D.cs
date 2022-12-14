using System;
using System.Numerics;

namespace MarioGabeKasper.Engine.Renderer
{
    public struct Line2DStruct
    {
        public Vector2 From;
        public Vector2 To;

        public Vector3 Color;
        public int LifeTime;

        public Line2DStruct(Vector2 from, Vector2 to, Vector3 color, int lifeTime)
        {
            From = from;
            To = to;
            Color = color;
            LifeTime = lifeTime;
        }
    }
    
    public class Line2D
    {
        public Line2DStruct LineData = new Line2DStruct();

        public Line2D(Line2DStruct lineData)
        {
            this.LineData = lineData;
        }
        
        public int BeginFrame()
        {
            LineData.LifeTime--;
            return LineData.LifeTime;
        }
        
    }
}