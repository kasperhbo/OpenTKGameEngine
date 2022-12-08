using System;
using System.Collections.Generic;
using System.Numerics;
using Accord.Diagnostics;
using MarioGabeKasper.Engine.Core;
using MarioGabeKasper.Engine.Utils;
using OpenTK.Graphics.OpenGL;

namespace MarioGabeKasper.Engine.Renderer
{
    public class DebugDraw
    {
        private static int _maxLines = 500;

        private static List<Line2D> _lines = new List<Line2D>();

        private static float[] _vertexArray = new float[_maxLines * 6 * 2];
        private static Shader _shader = AssetPool.GetShader(new ShaderSource("../../../debugLine2D.vert", "../../../debugLine2D.frag"));

        private static int _vaoId;
        private static int _vboId;

        private static bool _started = false;

        public static void Start()
        {
            //Generate VAO
            _vaoId = GL.GenVertexArray();
            GL.BindVertexArray(_vaoId);

            //Create vbo
            _vboId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboId);

            GL.BufferData(BufferTarget.ArrayBuffer, _vertexArray.Length * sizeof(float), _vertexArray,
                BufferUsageHint.DynamicDraw);
            
            //Position
            GL.VertexAttribPointer(0,3,VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            
            //Color
            GL.VertexAttribPointer(1,3,VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            //
            // GL.LineWidth(4.0f);        

        }

        public static void BeginFrame()
        {
            if (!_started)
            {
                Start();
                _started = true;
            }
            
            //Remove all dead lines
            for (int i = 0; i < _lines.Count; i++)
            {
                if (_lines[i].BeginFrame() < 0)
                {
                    _lines.RemoveAt(i);
                    i--;
                }
            }
        }
        
        public static void Draw()
        {
            if (_lines.Count <= 0)
                return;

            int index = 0;
            foreach (var line in _lines)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 position = i == 0 ? line.LineData.From : line.LineData.To;
                    Vector3 color = line.LineData.Color;
                    
                    //load position
                    _vertexArray[index] = position.X;
                    _vertexArray[index + 1] = position.Y;
                    _vertexArray[index + 2] = -10.0f;
                   
                    //load color
                    _vertexArray[index + 3] = color.X;
                    _vertexArray[index + 4] = color.Y;
                    _vertexArray[index + 5] = color.Z;

                    index += 6;
                }
            }
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboId);
            var ptr = IntPtr.Zero;
            GL.BufferSubData(BufferTarget.ArrayBuffer, ptr, _vertexArray.Length * sizeof(float), _vertexArray);
            
            _shader.Use();
            _shader.UploadMat4F("uProjection", Window.GetScene().GetCamera().GetProjectionMatrix());
            _shader.UploadMat4F("uView", Window.GetScene().GetCamera().GetViewMatrix());
            
            GL.BindVertexArray(_vaoId);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            
            GL.DrawArrays(PrimitiveType.Lines, 0, _lines.Count * 6 * 2);
            
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.BindVertexArray(0);
            
            _shader.Detach();
        }


        public static void AddLine2D(Vector2 from, Vector2 to)
        {
            if (_lines.Count >= _maxLines) return;
            AddLine2D(from, to, new Vector3(0,1,0), 1);
        }
        
        public static void AddLine2D(Vector2 from, Vector2 to, Vector3 color)
        {
            if (_lines.Count >= _maxLines) return;
            AddLine2D(from, to, color, 1);
        }
        
        public static void AddLine2D(Vector2 from, Vector2 to, Vector3 color, int lifeTime)
        {
            if (_lines.Count >= _maxLines) return;
            DebugDraw._lines.Add(new Line2D(new Line2DStruct(from, to, color, lifeTime)));
        }
    }
}