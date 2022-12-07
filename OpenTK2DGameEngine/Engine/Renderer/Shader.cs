using System;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace MarioGabeKasper.Engine.Renderer
{
    public class Shader
    {
        private readonly string fragmentSource;

        private readonly string vertexSource;
        private string filepath;
        private int shaderProgramID;

        public Shader(string vertexFilePath, string fragmentFilePath)
        {
            vertexSource = File.ReadAllText(vertexFilePath);
            fragmentSource = File.ReadAllText(fragmentFilePath);
        }
        
        public Shader(bool fromFile, string vertexFilePath, string fragmentFilePath)
        {
            vertexSource = vertexFilePath;
            fragmentSource = fragmentFilePath;
        }

        public void compile()
        {
            int vertexID, fragmentID;

            vertexID = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexID, vertexSource);
            GL.CompileShader(vertexID);

            GL.GetShader(vertexID, ShaderParameter.CompileStatus, out var succes);
            if (succes != (int)All.True)
            {
                // We can use `GL.GetShaderInfoLog(shader)` to get information about the error.
                var infoLog = GL.GetShaderInfoLog(vertexID);
                throw new Exception($"Error occurred whilst compiling Shader({vertexID}).\n\n{infoLog}");
            }

            fragmentID = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentID, fragmentSource);
            GL.CompileShader(fragmentID);

            GL.GetShader(fragmentID, ShaderParameter.CompileStatus, out succes);
            if (succes != (int)All.True)
            {
                // We can use `GL.GetShaderInfoLog(shader)` to get information about the error.
                var infoLog = GL.GetShaderInfoLog(fragmentID);
                throw new Exception($"Error occurred whilst compiling Shader({fragmentID}).\n\n{infoLog}");
            }

            shaderProgramID = GL.CreateProgram();
            GL.AttachShader(shaderProgramID, vertexID);
            GL.AttachShader(shaderProgramID, fragmentID);
            GL.LinkProgram(shaderProgramID);

#pragma warning disable CS0618 // Type or member is obsolete
            GL.GetProgram(shaderProgramID, ProgramParameter.LinkStatus, out succes);
#pragma warning restore CS0618 // Type or member is obsolete

            if (succes != (int)All.True)
                // We can use `GL.GetProgramInfoLog(program)` to get information about the error.
                throw new Exception($"Error occurred whilst linking Program({shaderProgramID})");
        }

        public void use()
        {
            GL.UseProgram(shaderProgramID);
        }

        public void detach()
        {
            GL.UseProgram(0);
        }

        public void uploadMat4f(string varName, Matrix4 mat4)
        {
            use();
            var varLocation = GL.GetUniformLocation(shaderProgramID, varName);
            GL.UniformMatrix4(varLocation, false, ref mat4);
        }

        public void uploadMat3f(string varName, Matrix3 mat3)
        {
            use();
            var varLocation = GL.GetUniformLocation(shaderProgramID, varName);

            GL.UniformMatrix3(varLocation, false, ref mat3);
        }

        public void uploadVec4f(string varName, Vector4 vec)
        {
            use();
            var varLocation = GL.GetUniformLocation(shaderProgramID, varName);

            GL.Uniform4(varLocation, vec);
        }

        public void uploadVec3f(string varName, Vector3 vec)
        {
            use();
            var varLocation = GL.GetUniformLocation(shaderProgramID, varName);

            GL.Uniform3(varLocation, vec);
        }

        public void uploadVec2f(string varName, Vector2 vec)
        {
            use();
            var varLocation = GL.GetUniformLocation(shaderProgramID, varName);

            GL.Uniform2(varLocation, vec);
        }

        public void uploadFloat(string varName, float val)
        {
            use();
            var varLocation = GL.GetUniformLocation(shaderProgramID, varName);

            GL.Uniform1(varLocation, val);
        }

        public void uploadInt(string varName, int val)
        {
            use();
            var varLocation = GL.GetUniformLocation(shaderProgramID, varName);

            GL.Uniform1(varLocation, val);
        }

        public void uploadIntArray(string varName, int[] data)
        {
            use();
            var varLocation = GL.GetUniformLocation(shaderProgramID, varName);
            GL.Uniform1(varLocation, data.Length, data);
        }

        public void uploadTexture(string varName, int slot)
        {
            use();
            var varLocation = GL.GetUniformLocation(shaderProgramID, varName);

            GL.Uniform1(varLocation, slot);
        }
    }
}