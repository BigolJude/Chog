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

        #region Movement Variables

        protected float top;
        protected float bottom;
        protected float left;
        protected float right;
        protected bool canMoveUp;
        protected bool canMoveDown;
        protected bool canMoveLeft;
        protected bool canMoveRight;
        protected bool physics;
        protected float speed;

        #endregion

        #region Position Variables

        protected Vector2 position;
        protected Vector2 origin;

        #endregion

        #region Apperance Variables

        protected Dictionary<string, Animation> animationMap;
        protected int currentTextureIndex = 0;
        protected Animation animation;
        protected bool visible;

        #endregion

        public SceneObject(string mName, Vector2 mPosition, Dictionary<string, Animation> mTextureMap, float mSpeed, bool mVisible, bool mPhysics)
        {
            this.name = mName;
            this.speed = mSpeed;
            this.position = mPosition;
            this.animationMap = mTextureMap;
            this.visible = mVisible;
            this.physics = mPhysics;
            
            SetAnimation("idle.png");
            SetDimensions();
        }
        private void SetDimensions()
        {
            this.top = position.Y;
            this.bottom = position.Y + animation.FrameHeight;
            this.left = position.X;
            this.right = position.X + animation.FrameWidth;
            this.origin = new Vector2(animation.FrameWidth / 2, animation.FrameHeight / 2);

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

        public void SetAnimation(string animationName)
        {
            animationMap.TryGetValue(animationName, out this.animation);
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

        #region Gets and Sets
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
        public Dictionary<string, Animation> TextureMap
        {
            get { return animationMap; }
            set { animationMap = value; }
        }
        public Animation Animation
        {
            get { return animation; }
            set { animation = value; }
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
            get { return animation.FrameWidth; }
        }
        public float Height
        {
            get { return animation.FrameHeight; }
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