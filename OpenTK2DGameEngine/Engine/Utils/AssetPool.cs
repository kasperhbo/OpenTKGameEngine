using System;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using MarioGabeKasper.Engine.Components;
using MarioGabeKasper.Engine.Renderer;

public struct ShaderSource
{
    public readonly string VertexPath;
    public readonly string FragmentPath;

    public ShaderSource(string vertexPath, string fragmentPath)
    {
        this.VertexPath = vertexPath;
        this.FragmentPath = fragmentPath;
    }
}

namespace MarioGabeKasper.Engine.Utils
{
    public class AssetPool
    {
        public static readonly Dictionary<ShaderSource, Shader> Shaders = new();
        public static readonly Dictionary<string, Texture> Textures = new();
        public static readonly Dictionary<string, SpriteSheet> SpriteSheets = new();
        public static readonly Dictionary<string, Sprite> Sprites = new();

        /// <summary>
        ///     Getting a shader if not existing create one
        /// </summary>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public static Shader GetShader(ShaderSource sourceName)
        {
            if (Shaders.TryGetValue(sourceName, out var shaderOut)) return shaderOut;

            var shader = new Shader(sourceName.VertexPath, sourceName.FragmentPath);
            shader.Compile();
            Shaders.Add(sourceName, shader);
            return shader;
        }

        /// <summary>
        ///     Getting a texture if not existing create one
        /// </summary>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public static Texture GetTexture(string sourceName)
        {
            string fullSource = sourceName;
            
            if (Textures.TryGetValue(fullSource, out var textureOut))
                return textureOut;
            
            var texture = new Texture();
            texture.Init(fullSource);
            Textures.Add(fullSource, texture);
            return texture;
        }

        public static void AddSpriteSheet(string sourceName, SpriteSheet spriteSheet)
        {
            string fullSource = sourceName;
            
            if (!SpriteSheets.ContainsKey(fullSource))
                SpriteSheets.Add(fullSource, spriteSheet);
        }

        public static SpriteSheet GetSpriteSheet(string sourceName)
        {
            string fullSource = sourceName;
            if (!SpriteSheets.ContainsKey(fullSource))
                Console.WriteLine("Error trying to get non excisting sprite sheet in asset pool");

            return SpriteSheets.GetValueOrDefault(fullSource, null);
        }
    }
}