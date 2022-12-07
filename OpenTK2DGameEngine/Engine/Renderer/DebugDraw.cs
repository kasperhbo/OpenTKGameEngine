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
        private static int maxLines = 500;

        private static List<Line2D> lines = new List<Line2D>();

        private static float[] vertexArray = new float[maxLines * 6 * 2];
        private static Shader shader = AssetPool.GetShader(new ShaderSource("../../../debugLine2D.vert", "../../../debugLine2D.frag"));

        private static int vaoID;
        private static int vboID;

        private static bool started = false;

        public static void Start()
        {
            //Generate VAO
            vaoID = GL.GenVertexArray();
            GL.BindVertexArray(vaoID);

            //Create vbo
            vboID = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboID);

            GL.BufferData(BufferTarget.ArrayBuffer, vertexArray.Length * sizeof(float), vertexArray,
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
            if (!started)
            {
                Start();
                started = true;
            }
            
            //Remove all dead lines
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].BeginFrame() < 0)
                {
                    lines.RemoveAt(i);
                    i--;
                }
            }
        }
        
        public static void Draw()
        {
            if (lines.Count <= 0)
                return;

            int index = 0;
            foreach (var line in lines)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 position = i == 0 ? line.lineData.from : line.lineData.to;
                    Vector3 color = line.lineData.color;
                    
                    //load position
                    vertexArray[index] = position.X;
                    vertexArray[index + 1] = position.Y;
                    vertexArray[index + 2] = -10.0f;
                   
                    //load color
                    vertexArray[index + 3] = color.X;
                    vertexArray[index + 4] = color.Y;
                    vertexArray[index + 5] = color.Z;

                    index += 6;
                }
            }
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboID);
            var ptr = IntPtr.Zero;
            GL.BufferSubData(BufferTarget.ArrayBuffer, ptr, vertexArray.Length * sizeof(float), vertexArray);
            
            shader.use();
            shader.uploadMat4f("uProjection", Window.GetScene().GetCamera().GetProjectionMatrix());
            shader.uploadMat4f("uView", Window.GetScene().GetCamera().GetViewMatrix());
            
            GL.BindVertexArray(vaoID);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            
            
            GL.DrawArrays(PrimitiveType.Lines, 0, lines.Count * 6 * 2);
            
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.BindVertexArray(0);
            
            shader.detach();
        }


        public static void AddLine2D(Vector2 from, Vector2 to)
        {
            if (lines.Count >= maxLines) return;
            AddLine2D(from, to, new Vector3(0,1,0), 1);
        }
        
        public static void AddLine2D(Vector2 from, Vector2 to, Vector3 color)
        {
            if (lines.Count >= maxLines) return;
            AddLine2D(from, to, color, 1);
        }
        
        public static void AddLine2D(Vector2 from, Vector2 to, Vector3 color, int lifeTime)
        {
            if (lines.Count >= maxLines) return;
            DebugDraw.lines.Add(new Line2D(new Line2DStruct(from, to, color, lifeTime)));
        }
    }
}