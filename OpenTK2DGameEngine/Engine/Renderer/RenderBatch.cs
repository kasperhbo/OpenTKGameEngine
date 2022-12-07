using System;
using System.Collections.Generic;
using MarioGabeKasper.Engine.Components;
using MarioGabeKasper.Engine.Core;
using MarioGabeKasper.Engine.Utils;
using OpenTK.Graphics.OpenGL4;

namespace MarioGabeKasper.Engine.Renderer
{
    public class RenderBatch : IComparable<RenderBatch>
    {
        private readonly int _colorOffset;
        private readonly int _colorSize = 4;
        private readonly int _maxBatchSize;
        private readonly int _positionOffset;

        //Vertex
        //======
        //Pos                       //Color                     //TexCoords     //texid
        //float, float,             float,float,float,float      float,float    float
        private readonly int _positionSize = 2;
        private readonly Shader _shader;

        private readonly SpriteRenderer[] _sprites;
        private readonly int _texCoordSize = 2;
        private readonly int _texCoorOffset;
        private readonly int _texIdOffset;
        private readonly int _texIdSize = 1;
        private readonly int[] _texSlots = { 0, 1, 2, 3, 4, 5, 6, 7 };

        private readonly int _vertexSize;
        private readonly int _vertexSizeInBytes;
        private readonly float[] _vertices;
        private readonly Camera camera;

        private readonly List<Texture> textures = new();
        private int _numSprites;

        private int _vaoId, _vboId;

        private int zIndex;

        public RenderBatch(int maxBatchSize, Camera camera, int zIndex)
        {
            this.zIndex = zIndex;
            _positionOffset = 0;
            _colorOffset = _positionOffset + _positionSize * sizeof(float);
            _texCoorOffset = _colorOffset + _colorSize * sizeof(float);
            _texIdOffset = _texCoorOffset + _texCoordSize * sizeof(float);

            _vertexSize = 9;
            _vertexSizeInBytes = _vertexSize * sizeof(float);

            this.camera = camera;

            _shader = AssetPool.GetShader(new ShaderSource("../../../default.vert", "../../../default.frag"));

            _sprites = new SpriteRenderer[maxBatchSize];

            _maxBatchSize = maxBatchSize;

            //4 vert qauds
            _vertices = new float[maxBatchSize * 4 * _vertexSize];

            _numSprites = 0;
            HasRoom = true;
        }

        public bool HasRoom { get; private set; }

        public void Start()
        {
            //Generate and bind vao
            _vaoId = GL.GenVertexArray();
            GL.BindVertexArray(_vaoId);

            //Allocate space for verts
            _vboId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboId);
            GL.BufferData(BufferTarget.ArrayBuffer,
                _vertices.Length * sizeof(float),
                _vertices,
                BufferUsageHint.DynamicDraw);

            //Create And Upload indices
            var eboID = GL.GenBuffer();
            var indices = GenerateIndices();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, eboID);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices,
                BufferUsageHint.StaticDraw);

            //Enable attrib pointers
            GL.VertexAttribPointer(0, _positionSize, VertexAttribPointerType.Float, false, _vertexSizeInBytes,
                _positionOffset);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, _colorSize, VertexAttribPointerType.Float, false, _vertexSizeInBytes,
                _colorOffset);
            GL.EnableVertexAttribArray(1);

            GL.VertexAttribPointer(2, _texCoordSize, VertexAttribPointerType.Float, false, _vertexSizeInBytes,
                _texCoorOffset);
            GL.EnableVertexAttribArray(2);

            GL.VertexAttribPointer(3, _texIdSize, VertexAttribPointerType.Float, false, _vertexSizeInBytes,
                _texIdOffset);
            GL.EnableVertexAttribArray(3);
        }


        public void Render()
        {
            var rebufferData = false;

            for (var i = 0; i < _numSprites; i++)
            {
                var spr = _sprites[i];

                if (spr.IsDirty())
                {
                    rebufferData = true;
                    LoadVertexProperties(i);
                    spr.SetClean();
                }
            }

            if (rebufferData)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vboId);
                var ptr = IntPtr.Zero;
                GL.BufferSubData(BufferTarget.ArrayBuffer, ptr, _vertices.Length * sizeof(float), _vertices);
            }

            _shader.use();

            _shader.uploadMat4f("uProjection", camera.GetProjectionMatrix());
            _shader.uploadMat4f("uView", camera.GetViewMatrix());

            for (var i = 0; i < textures.Count; i++)
            {
                GL.ActiveTexture(TextureUnit.Texture0 + i + 1);
                textures[i].bind();
            }

            _shader.uploadIntArray("uTextures", _texSlots);

            GL.BindVertexArray(_vaoId);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            GL.DrawElements(PrimitiveType.Triangles, _numSprites * 6, DrawElementsType.UnsignedInt, 0);

            // Unbind everything
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);

            GL.BindVertexArray(0);

            for (var i = 0; i < textures.Count; i++) textures[i].unbind();

            _shader.detach();
        }

        public void AddSprite(SpriteRenderer spriteRenderer)
        {
            //Get index and render object
            var index = _numSprites;
            _sprites[index] = spriteRenderer;
            _numSprites++;

            if (spriteRenderer.GetTexture() != null)
                if (!textures.Contains(spriteRenderer.GetTexture()))
                    textures.Add(spriteRenderer.GetTexture());

            //Add properties to vert array
            LoadVertexProperties(index);

            if (_numSprites >= _maxBatchSize) HasRoom = false;
            // Console.WriteLine("Creating new Batch");
        }

        private void LoadVertexProperties(int index)
        {
            var sprite = _sprites[index];

            //Find offset in array
            var offset = index * 4 * _vertexSize;

            float xAdd = 1;
            float yAdd = 1;

            var color = sprite.GetColor();
            var texCoords = sprite.GetTexCoords();

            var texID = 0;

            //Find texture in textures list
            if (sprite.GetTexture() != null)
                for (var i = 0; i < textures.Count; i++)
                    if (textures[i].Equals(sprite.GetTexture()))
                    {
                        texID = i + 1;
                        break;
                    }

            for (var i = 0; i < 4; i++)
            {
                if (i == 1)
                    yAdd = 0.0f;
                else if (i == 2)
                    xAdd = 0.0f;
                else if (i == 3)
                    yAdd = 1.0f;

                _vertices[offset] = sprite.GetGameObject().GetTransform().Position.X +
                                    xAdd * sprite.GetGameObject().GetTransform().Scale.X;

                _vertices[offset + 1] = sprite.GetGameObject().GetTransform().Position.Y +
                                        yAdd * sprite.GetGameObject().GetTransform().Scale.Y;

                //Load color
                _vertices[offset + 2] = color.X;
                _vertices[offset + 3] = color.Y;
                _vertices[offset + 4] = color.Z;
                _vertices[offset + 5] = color.W;

                //load tex coord
                _vertices[offset + 6] = texCoords[i].X;
                _vertices[offset + 7] = texCoords[i].Y;

                //load tex id
                _vertices[offset + 8] = texID;

                offset += _vertexSize;
            }
        }

        private int[] GenerateIndices()
        {
            //6 indices per qaud
            var elements = new int[6 * _maxBatchSize];

            for (var i = 0; i < _maxBatchSize; i++) LoadElementIndices(elements, i);

            return elements;
        }

        private void LoadElementIndices(int[] elements, int index)
        {
            var offsetArrayIndex = 6 * index;
            var offset = 4 * index;

            // 3, 2, 0, 0, 2, 1        7, 6, 4, 4, 6, 5
            // Triangle 1
            elements[offsetArrayIndex] = offset + 3;
            elements[offsetArrayIndex + 1] = offset + 2;
            elements[offsetArrayIndex + 2] = offset + 0;

            // Triangle 2
            elements[offsetArrayIndex + 3] = offset + 0;
            elements[offsetArrayIndex + 4] = offset + 2;
            elements[offsetArrayIndex + 5] = offset + 1;
        }

        public bool HasTextureRoom()
        {
            return textures.Count < 8;
        }

        public bool HasTexture(Texture tex)
        {
            return textures.Contains(tex);
        }

        public int GetZIndex()
        {
            return this.zIndex;
        }

        public int CompareTo(RenderBatch other)
        {
            if (this.zIndex < other.zIndex)
                return -1;
            
            if (this.zIndex == other.zIndex)
                return 0;
            
            return 1;
        }
    }
}