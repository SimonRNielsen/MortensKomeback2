using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace MortensKomeback2
{
    internal class HealthBar : GameObject
    {
        #region Fields
        private Texture2D initalSprite;

        private Texture2D barBackground;
        private Texture2D barForeground;
        private Rectangle backgroundRectangle;
        private Rectangle foregroundRectangle;
        #endregion

        //Constructor is instantiated in GameWorld 
        //There it will be given the right sprite
        #region Constructors
        public HealthBar( Texture2D initalSprite, float layer)
        {
            this.sprite = initalSprite;
            this.layer = layer;
        }

        //public HealthBar(Texture2D background, Texture2D foreground, int maxHealth, Vector2 position, Vector2 size)
        //{
        //    barBackground = background;
        //    barForeground = foreground;
        //    this.maxHealth = maxHealth;
        //    currentHealth = maxHealth;


        //    // Set rectangles based on position and size
        //    backgroundRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        //    foregroundRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        //}

        #endregion

        #region Properties
        /// <summary>
        /// Gives the possibility to read the sprite, but most importantly change it in GameWorld
        /// </summary>
        public Texture2D InitalSprite { get => initalSprite; set => initalSprite = value; }
        #endregion

        #region Methods
        public override void OnCollision(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public override void LoadContent(ContentManager content)
        {
            //if (sprite == GameWorld.commonSprites["healthBarRed"])
            //{
            //    // Calculate health bar foreground width
            //    //int healthPercentage = (int)CurrentHealth / MaxHealth;
            //    //Rectangle healthBarRect = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, (int)(healthBarForeground.Width * healthPercentage), healthBarForeground.Height);

            //    int healthPercentage = 50;
            //    Rectangle healthBarRect = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, (int)(healthBarForeground.Width * healthPercentage), healthBarForeground.Height);

            //}
        }

        public override void Update(GameTime gameTime)
        {
            this.position = new Vector2(GameWorld.Camera.Position.X - 680, GameWorld.Camera.Position.Y -460);

        }

        public void UpdateHealth(int health)
        {
            this.health = Health;

            // Calculate the width of the foreground based on the health percentage
            float healthPercentage = (float)health / GameWorld.PlayerInstance.HealthMax;
            foregroundRectangle.Width = (int)(backgroundRectangle.Width * healthPercentage);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the background
            spriteBatch.Draw(barBackground, backgroundRectangle, Color.White);

            // Draw the foreground (current health)
            spriteBatch.Draw(barForeground, foregroundRectangle, Color.White);
        }
        #endregion





    }
}
