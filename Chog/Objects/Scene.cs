using Chog.Objects.Participants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;

namespace Chog.Objects
{
    internal class Scene
    {
        private const string CONTENT_FILE = @"..\..\..\Content\";
        private const int GRAVITY = 300;
        private List<SceneObject> sceneObjects = new List<SceneObject>();
        private Vector2 location;
        private ContentManager content;
        private Texture2D background;
        private Player player;
        private GraphicsDevice graphicsDevice;

        public Scene(Vector2 mLocation, ContentManager mContent, GraphicsDevice mGraphicsDevice)
        {
            this.graphicsDevice = mGraphicsDevice;
            this.location = mLocation;
            this.content = mContent;
            Load();
        }
        public void Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager _graphics, GameTime gameTime)
        {
            _spriteBatch.Draw(
                this.background,
                new Vector2(0, 0),
                new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),
                Color.White,
                0f,
                new Vector2(0, 0),
                new Vector2((float)_graphics.GraphicsDevice.Viewport.Width,
                            (float)_graphics.GraphicsDevice.Viewport.Height),
                SpriteEffects.None,
                0f);

            foreach (SceneObject sceneObject in sceneObjects)
            {
                sceneObject.SetMoveability(sceneObjects, _graphics);
                float sceneObjectVelocity = GRAVITY * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (sceneObject.Physics && sceneObject.CanMoveDown)
                {
                    sceneObject.PositionY += sceneObjectVelocity;
                    sceneObject.SetAnimation("falling.png");
                }

                if (sceneObject.Visible)
                {
                    sceneObject.Animation.Update(gameTime);
                    sceneObject.Animation.Draw(sceneObject.Position, _spriteBatch);
                }
            }
        }
        public void Load()
        {
            sceneObjects.Clear();
            XElement Scenes = XElement.Load(CONTENT_FILE + @"ChogContent.xml");
            var elements = Scenes.Descendants("scene").Where(e =>
                                                             (float)e.Attribute("locationX") == location.X &&
                                                             (float)e.Attribute("locationY") == location.Y);

            this.background = Texture2D.FromFile(graphicsDevice, CONTENT_FILE + elements.First().Attribute("backgroundTexture").Value);

            foreach (var element in elements.Elements().Where(e => e.Name == "sceneObject"))
            {
                string elementName = (string)element.Attribute("name");

                if (elementName == "player")
                {
                    this.player = new Player(
                        elementName,
                        new Vector2((float)element.Attribute("positionX"),
                        (float)element.Attribute("positionY")),
                        LoadTextures(element),
                        (float)element.Attribute("speed"),
                        (bool)element.Attribute("visible"),
                        (bool)element.Attribute("physics"));
                    sceneObjects.Add(this.player);
                }
                else
                {
                    SceneObject sceneObject = new SceneObject(
                        elementName,
                        new Vector2((float)element.Attribute("positionX"),
                        (float)element.Attribute("positionY")),
                        LoadTextures(element),
                        (float)element.Attribute("speed"),
                        (bool)element.Attribute("visible"),
                        (bool)element.Attribute("physics"));
                    sceneObjects.Add(sceneObject);
                }
            }
        }

        private Dictionary<string, Animation> LoadTextures(XElement element)
        {
            Dictionary<string, Animation> textureMap = new Dictionary<string, Animation>();

            var textureElements = element.Elements();

            foreach (XElement textureElement in textureElements.Where(x => x.Name == "texture"))
            { 
                string textureName = (string)textureElement.Attribute("name");

                Animation animation = new Animation(
                    Texture2D.FromFile(graphicsDevice, CONTENT_FILE + textureName), 
                    (int)textureElement.Attribute("frameCount"), 
                    textureName, 
                    (int)textureElement.Attribute("frameHeight"), 
                    (int)textureElement.Attribute("frameWidth"));

                textureMap.Add(textureName, animation);
            }
            return textureMap;
        }

        #region Values
        public Vector2 Location
        {
            get { return location; }
            set { location = value; }
        }
        public float LocationX
        {
            set { location.X = value; }
        }
        public float LocationY
        {
            set { location.Y = value; }
        }
        public Player Player
        {
            get { return player; }
            set { player = value; }
        }
        #endregion
    }
}
