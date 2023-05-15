using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Chog.Objects.Participants
{
    internal class Player : SceneObject
    {
        private int jumpHeight = 20;
        private int currentJumpHeight;
        private bool jumping = false;

        public Player(string mName, Vector2 mPosition, Texture2D mTexture, float mSpeed, bool mVisible, bool mPhysics) : base(mName, mPosition, mTexture, mSpeed, mVisible, mPhysics)
        {

        }

        public void GetPlayerMovement(GameTime gameTime)
        {
            float playerVelocity = this.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            var keyState = Keyboard.GetState();

            if (this.jumping)
            {
                if(currentJumpHeight < jumpHeight)
                {
                    currentJumpHeight += 1;
                    this.PositionY -= (playerVelocity + (jumpHeight - currentJumpHeight));
                }
                else
                {
                    this.jumping = false;
                    currentJumpHeight = 0;
                    jumpHeight = 20;
                }
            }
            else
            {
                if (keyState.IsKeyDown(Keys.W) && this.CanMoveUp && !this.CanMoveDown)
                {
                    this.PositionY -= playerVelocity;
                    this.jumping = true;
                }
            }

            if (keyState.IsKeyDown(Keys.A) && this.CanMoveLeft)
                this.PositionX -= playerVelocity;

            if (keyState.IsKeyDown(Keys.S) && this.CanMoveDown)
                this.PositionY += playerVelocity;

            if (keyState.IsKeyDown(Keys.D) && this.CanMoveRight)
                this.PositionX += playerVelocity;
        }
        
    }
}
