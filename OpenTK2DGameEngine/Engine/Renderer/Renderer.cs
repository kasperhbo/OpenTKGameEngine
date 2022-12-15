using System;
using System.Collections.Generic;

using MarioGabeKasper.Engine.Components;
using MarioGabeKasper.Engine.Core;

namespace MarioGabeKasper.Engine.Renderer
{
    public class Renderer
    {
        private readonly List<RenderBatch> batches = new();
        private readonly int maxBatchSize = 10000;

        public void Add(GameObject go, Camera cam)
        {
            var spr = (SpriteRenderer)go.GetComponent<SpriteRenderer>();

            if (spr != null)
                Add(spr, cam);
        }

        private void Add(SpriteRenderer spr, Camera cam)
        {
            var added = false;
            foreach (var batch in batches)
                if (batch.HasRoom && batch.ZIndex == spr.Parent.ZIndex)
                {
                    var tex = spr.Texture;
                    if (tex == null || batch.HasTexture(tex) || batch.TextureRoom)
                    {
                        batch.AddSprite(spr);
                        added = true;
                        break;
                    }
                }

            if (!added)
            {
                // Console.WriteLine("new batch");
                var newBatch = new RenderBatch(maxBatchSize, cam, spr.Parent.ZIndex);
                newBatch.Start();
                batches.Add(newBatch);
                newBatch.AddSprite(spr);
                batches.Sort();
            }
        }


        public void Render()
        {
            foreach (var batch in batches) batch.Render();
        }
    }
}