using System;
using System.Collections.Generic;
using System.Resources;
using MarioGabeKasper.Engine.Components;
using MarioGabeKasper.Engine.Renderer;

public struct ShaderSource
{
    public readonly string vertexPath;
    public readonly string fragmentPath;

    public ShaderSource(string vertexPath, string fragmentPath)
    {
        this.vertexPath = vertexPath;
        this.fragmentPath = fragmentPath;
    }
}

namespace MarioGabeKasper.Engine.Utils
{
    public class AssetPool
    {
        private static readonly Dictionary<ShaderSource, Shader> _shaders = new();
        private static readonly Dictionary<string, Texture> _textures = new();
        private static readonly Dictionary<string, SpriteSheet> _spriteSheets = new();

        /// <summary>
        ///     Getting a shader if not existing create one
        /// </summary>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public static Shader GetShader(ShaderSource sourceName)
        {
            if (_shaders.TryGetValue(sourceName, out var shaderOut)) return shaderOut;

            var shader = new Shader(sourceName.vertexPath, sourceName.fragmentPath);
            shader.compile();
            _shaders.Add(sourceName, shader);
            return shader;
        }

        /// <summary>
        ///     Getting a texture if not existing create one
        /// </summary>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public static Texture GetTexture(string sourceName)
        {
            string fullSource = "../../../Resources/Textures/" + sourceName;
            if (_textures.TryGetValue(fullSource, out var textureOut)) return textureOut;

            var texture = new Texture();
            texture.Init(fullSource);
            _textures.Add(fullSource, texture);
            return texture;
        }

        public static void AddSpriteSheet(string sourceName, SpriteSheet spriteSheet)
        {
            string fullSource = "../../../Resources/Textures/" + sourceName;
            if (!_spriteSheets.ContainsKey(fullSource))
                _spriteSheets.Add(fullSource, spriteSheet);
        }

        public static SpriteSheet GetSpriteSheet(string sourceName)
        {
            string fullSource = "../../../Resources/Textures/" + sourceName;
            if (!_spriteSheets.ContainsKey(fullSource))
                Console.WriteLine("Error trying to get non excisting sprite sheet in asset pool");

            return _spriteSheets.GetValueOrDefault(fullSource, null);
        }
    }
}