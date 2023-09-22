using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Chog.Objects
{


    internal class Animation
    {
        private string animationName;
        private int currentFrame = 0;
        private int frameCount;
        private float frameSpeed = 0.01f;
        private int frameHeight;
        private int frameWidth;
        private Texture2D texture;
        private bool isLoop = true;

        private float timer = 0f;


        public Animation(Texture2D mTexture, int mFrameCount, string animationName, int mFrameHeight, int mFrameWidth)
        {
            this.texture = mTexture;
            this.frameCount = mFrameCount;
            this.frameWidth = mFrameWidth;
            this.frameHeight = mFrameHeight;
            this.animationName = animationName;
        }
        private void getNextFrameIndex()
        {
            if (currentFrame < frameCount - 1)
                this.currentFrame = currentFrame + 1;
            else if (isLoop)
                this.currentFrame = 0;
            else
                this.Stop();
            Debug.WriteLine(currentFrame);
        }
        public string AnimationName
        {
            get { return animationName; }
        }
        public Texture2D Texture
        {
            get { return texture; }
        }
        public int FrameHeight
        {
            get { return frameHeight; }
        }
        public int FrameWidth
        {
            get { return frameWidth; }
        }
        public void Stop()
        {
            timer = 0f;
            currentFrame = 0;
        }
        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(timer >= frameSpeed)
            {
                timer = 0f;
                this.getNextFrameIndex();
            }
        }
        public void Draw(Vector2 position, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture,
                             position,
                             new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight),
                             Color.White);
        }
    }
}
