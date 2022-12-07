using System;
using System.Numerics;

namespace MarioGabeKasper.Engine.Renderer
{
    public struct Line2DStruct
    {
        public Vector2 from;
        public Vector2 to;

        public Vector3 color;
        public int lifeTime;

        public Line2DStruct(Vector2 from, Vector2 to, Vector3 color, int lifeTime)
        {
            this.from = from;
            this.to = to;
            this.color = color;
            this.lifeTime = lifeTime;
        }
    }
    
    public class Line2D
    {
        public Line2DStruct lineData = new Line2DStruct();

        public Line2D(Line2DStruct lineData)
        {
            this.lineData = lineData;
        }
        
        public int BeginFrame()
        {
            lineData.lifeTime--;
            return lineData.lifeTime;
        }
        
    }
}