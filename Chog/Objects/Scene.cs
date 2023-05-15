using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;
using Chog.Objects.Participants;

namespace Chog.Objects
{
    internal class Scene
    {
        private const int GRAVITY = 300;
        private List<SceneObject> sceneObjects = new List<SceneObject>();
        private Vector2 location;
        private ContentManager content;
        private Texture2D background;
        private Player player;

        public Scene(Vector2 location, ContentManager mContent)
        {
            this.location = location;
            this.content = mContent;
            Load();
        }
        public void Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager _graphics, GameTime gameTime)
        {
            _spriteBatch.Draw(
                background,
                new Vector2(0, 0),
                new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),
                Color.White,
                0f,
                new Vector2(0, 0),
                new Vector2((float)_graphics.GraphicsDevice.Viewport.Width / background.Width,
                            (float)_graphics.GraphicsDevice.Viewport.Height / background.Height),
                SpriteEffects.None,
                0f);

            foreach (SceneObject sceneObject in sceneObjects)
            {
                sceneObject.SetMoveability(sceneObjects, _graphics);
                float sceneObjectVelocity = GRAVITY * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (sceneObject.Physics && sceneObject.CanMoveDown)
                    sceneObject.PositionY += sceneObjectVelocity;

                if (sceneObject.Visible)
                    _spriteBatch.Draw(
                        sceneObject.Texture,
                        sceneObject.Position,
                        null,
                        Color.White,
                        0f,
                        sceneObject.Origin,
                        Vector2.One,
                        SpriteEffects.None,
                        0f);
            }
        }
        public void Load()
        {
            sceneObjects.Clear();
            XElement Scenes = XElement.Load(@"D:\XML\XmlFileTest.xml");
            var elements = Scenes.Descendants("scene").Where(e =>
                                                             (float)e.Attribute("locationX") == location.X &&
                                                             (float)e.Attribute("locationY") == location.Y);

            this.background = content.Load<Texture2D>(elements.First().Attribute("backgroundTexture").Value);

            foreach (var element in elements.Elements().Where(e => e.Name == "sceneObject"))
            {
                string elementName = (string)element.Attribute("name");

                if (elementName == "player")
                {
                    this.player = new Player(
                        elementName,
                        new Vector2((float)element.Attribute("positionX"),
                        (float)element.Attribute("positionY")),
                        content.Load<Texture2D>((string)element.Attribute("texture")),
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
                        content.Load<Texture2D>((string)element.Attribute("texture")),
                        (float)element.Attribute("speed"),
                        (bool)element.Attribute("visible"),
                        (bool)element.Attribute("physics"));
                    sceneObjects.Add(sceneObject);
                }
            }
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
