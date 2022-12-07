using MarioGabeKasper.Engine.Renderer;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioGabeKasper.Engine.Core
{
    public class FrameBuffer
    {
        private int fboID = 0;
        private Texture texture = null; 

        public FrameBuffer(int width, int height)
        {
            //Generate FrameBuffer
            fboID = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fboID);

            //Create the texture to render the data to
            this.texture = new Texture(width, height);
            
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, 
                TextureTarget.Texture2D, this.texture.GetTexID(), 0);
            
            //Create render buffer, store depth info
            int rboId = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rboId);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent32, width,
                height);
            
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, 
                FramebufferAttachment.DepthAttachment, 
                RenderbufferTarget.Renderbuffer, 
                rboId);
            
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                throw new Exception("Error: framebuffer is not complete");
            }
            
            //Bind frame buffer
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fboID);
        }

        public void UnBind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public int GetFBOID()
        {
            return fboID;
        }
        
        public int GetTextureID()
        {
            return texture.GetTexID();
        }
    }
}
