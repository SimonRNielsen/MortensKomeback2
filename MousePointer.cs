using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MortensKomeback2
{
    internal class MousePointer : GameObject
    {
        #region Fields

        private GraphicsDeviceManager _graphics;

        #endregion

        #region Properties

        /// <summary>
        /// Returns a 1x1 pixel CollisionBox at the tip of the mousepointer
        /// </summary>
        public override Rectangle CollisionBox
        {
            get { return new Rectangle((int)GameWorld.MousePosition.X, (int)GameWorld.MousePosition.Y , 1, 1); }

        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a MousePointer with the intent of enabling "collision" with Button-class objects
        /// </summary>
        /// <param name="graphics">Needed for translating precise location of mouse in comparison to game</param>
        public MousePointer(GraphicsDeviceManager graphics)
        {
            this.health = 9999999;
            this.scale = 1;
            this.layer = 0;
            _graphics = graphics;
        }

        #endregion

        #region Methods

        public override void LoadContent(ContentManager content)
        {
            //throw new NotImplementedException();
        }

        public override void OnCollision(GameObject gameObject)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Updates mouses coordinates for realtime tracking used in world design/creation
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //GameWorld.ActualMousePosition = new Vector2((int)(GameWorld.MousePosition.X / GameWorld.Camera.Zoom) - (int)((float)_graphics.PreferredBackBufferWidth / 2 / GameWorld.Camera.Zoom) + (int)GameWorld.Camera.Position.X, (int)(GameWorld.MousePosition.Y / GameWorld.Camera.Zoom) - (int)((float)_graphics.PreferredBackBufferHeight / 2 / GameWorld.Camera.Zoom) + 20 + (int)GameWorld.Camera.Position.Y);
            //GameWorld.mouseX = (int)(GameWorld.mousePosition.X / GameWorld.Camera.Zoom) - (int)((float)_graphics.PreferredBackBufferWidth / 2 / GameWorld.Camera.Zoom) + (int)GameWorld.Camera.Position.X;
            //GameWorld.mouseY = (int)(GameWorld.mousePosition.Y / GameWorld.Camera.Zoom) - (int)((float)_graphics.PreferredBackBufferHeight / 2 / GameWorld.Camera.Zoom) + 20 + (int)GameWorld.Camera.Position.Y;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Nothing to draw
        }

        #endregion
    }
}