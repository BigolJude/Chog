using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace Chog.Objects
{
    internal class SceneObject
    {
        private const int COLLISION_BUFFER = 5;
        protected string name;
        protected float top;
        protected float bottom;
        protected float left;
        protected float right;
        protected Texture2D texture;
        protected Vector2 position;
        protected Vector2 origin;
        protected float speed;
        protected bool visible;
        protected bool physics;
        protected bool canMoveUp;
        protected bool canMoveDown;
        protected bool canMoveLeft;
        protected bool canMoveRight;

        public SceneObject(string mName, Vector2 mPosition, Texture2D mTexture, float mSpeed, bool mVisible, bool mPhysics)
        {
            this.name = mName;
            this.speed = mSpeed;
            this.position = mPosition;
            this.texture = mTexture;
            this.visible = mVisible;
            this.physics = mPhysics;
            SetDimensions();
        }
        private void SetDimensions()
        {
            this.top = position.Y - texture.Height / 2;
            this.bottom = position.Y + texture.Height / 2;
            this.left = position.X - texture.Width / 2;
            this.right = position.X + texture.Width / 2;
            this.origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }
        public void SetMoveability(List<SceneObject> sceneObjects, GraphicsDeviceManager _graphics)
        {
            canMoveDown = true;
            canMoveUp = true;
            canMoveLeft = true;
            canMoveRight = true;

            SetDimensions();

            foreach (SceneObject sceneObject in sceneObjects)
            {
                if (this.right >= _graphics.PreferredBackBufferWidth)
                    this.canMoveRight = false;

                if (this.left <= 0)
                    this.canMoveLeft = false;

                if (this.bottom >= _graphics.PreferredBackBufferHeight)
                    this.canMoveDown = false;

                if (this.Top <= 0)
                    this.canMoveUp = false;

                if (!sceneObject.name.Equals(this.name))
                {
                    if(this.ObjectsAllignedHorizontally(sceneObject))
                    {
                        if (sceneObject.bottom > this.top && sceneObject.bottom < this.bottom)
                            this.canMoveUp = false;

                        if (sceneObject.top < this.bottom && sceneObject.top > this.top)
                            this.canMoveDown = false;
                    }

                    if(this.ObjectsAllignedVertically(sceneObject))
                    {
                        if (sceneObject.right > this.left && sceneObject.left < this.left)
                            this.canMoveLeft = false;

                        if (sceneObject.left < this.right && sceneObject.right > this.right)
                            this.canMoveRight = false;
                    }
                }
            }
        }

        #region Allignment Checks
        private bool ObjectsAllignedVertically(SceneObject sceneObject)
        {
            return (sceneObject.bottom > this.top + COLLISION_BUFFER && sceneObject.bottom < this.bottom) ||
                   (sceneObject.top + COLLISION_BUFFER < this.bottom && sceneObject.bottom > this.bottom) ||
                   (sceneObject.top < this.top && this.bottom < sceneObject.bottom) ||
                   (sceneObject.top >= this.top && this.bottom > sceneObject.bottom);
        }
        private bool ObjectsAllignedHorizontally(SceneObject sceneObject)
        {
            return (sceneObject.right > this.left + COLLISION_BUFFER && sceneObject.right < this.right) ||
                   (sceneObject.left + COLLISION_BUFFER < this.right && sceneObject.left > this.left) ||
                   (sceneObject.left < this.left && this.right < sceneObject.right) ||
                   (sceneObject.left >= this.left && this.right >= sceneObject.right);
        }
        #endregion

        #region Values
        public string Name
        {
            get { return this.name; }
        }
        public bool Visible
        {
            get { return visible; }
            set { this.visible = value; }
        }
        public void ToggleVisibility()
        {
            this.visible = !this.visible;
        }
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { this.position = value; }
        }
        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public bool Physics
        {
            get { return physics; }
            set { physics = value; }
        }
        public float PositionY
        {
            get { return position.Y; }
            set
            {
                SetDimensions();
                position.Y = value;
            }
        }
        public float PositionX
        {
            get { return position.X; }
            set
            {
                SetDimensions();
                position.X = value;
            }
        }
        public float Width
        {
            get { return texture.Width; }
        }
        public float Height
        {
            get { return texture.Height; }
        }
        public float Top
        {
            get
            {
                SetDimensions();
                return top;
            }
        }
        public float Bottom
        {
            get
            {
                SetDimensions();
                return bottom;
            }
        }
        public float Left
        {
            get
            {
                SetDimensions();
                return left;
            }
        }
        public float Right
        {
            get
            {
                SetDimensions();
                return right;
            }
        }
        public bool CanMoveDown
        {
            get { return canMoveDown; }
        }
        public bool CanMoveUp
        {
            get { return canMoveUp; }
        }
        public bool CanMoveLeft
        {
            get { return canMoveLeft; }
        }
        public bool CanMoveRight
        {
            get { return canMoveRight; }
        }
        #endregion
    }
}