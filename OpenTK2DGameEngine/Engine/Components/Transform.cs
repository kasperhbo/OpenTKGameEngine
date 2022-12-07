using MarioGabeKasper.Engine.GUI;
using System;
using System.Numerics;
using System.Reflection;
using System.Xml.Serialization;
// using OpenTK.Mathematics;

namespace MarioGabeKasper.Engine.Components
{
    public class Transform : DefaultImGuiFieldWindow
    {
        public Vector2 Position;
        public Vector2 Scale;

        public Transform()
        {
            Init(new Vector2(), new Vector2());
        }

        public Transform(Vector2 position)
        {
            Init(position, new Vector2(100, 100));
        }

        public Transform(Vector2 position, Vector2 scale)
        {
            Init(position, scale);
        }

        private void Init(Vector2 position, Vector2 scale)
        {
            this.Position = position;
            this.Scale = scale;
        }

        public Transform Copy()
        {
            var t = new Transform(new Vector2(Position.X, Position.Y), new Vector2(Scale.X, Scale.Y));
            return t;
        }

        public void Copy(Transform to)
        {
            to.Position = Position;
            to.Scale = Scale;
        }

        public override bool Equals(object o)
        {
            if (o == null) return false;
            if (!(o is Transform)) return false;

            var t = (Transform)o;
            return t.Position.Equals(Position) && t.Scale.Equals(Scale);
        }

        public void ImGui()
        {
            CreateDefaultFieldWindow();
        }
    }
}