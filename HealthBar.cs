using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace MortensKomeback2
{
    internal class HealthBar : GameObject
    {
        #region Fields
        private Texture2D initalSprite;

        private Rectangle backgroundRectangle;
        private Rectangle foregroundRectangle;
        private bool enemyHealthbar = false;
        Enemy enemy;
        #endregion

        /// <summary>
        /// Constructor is instantiated in GameWorld 
        /// </summary>
        /// <param name="layer">Layer of the healtbar</param>
        /// <param name="type">Type of bar</param>
        //There it will be given the right sprite
        #region Constructors
        public HealthBar(float layer, int type)
        {
            switch (type)
            {
                case 1:
                    sprite = GameWorld.commonSprites["healthBarRed"];
                    break;
            }
            this.layer = layer;
        }
        /// <summary>
        /// Overload of Healthbar cusntructor for an enemy healtbar
        /// </summary>
        /// <param name="layer">Layer of the healtbar</param>
        /// <param name="type">Type of bar</param>
        /// <param name="enemy">The instance of an enemy, the healtbar should correspond to</param>
        public HealthBar(float layer, int type, Enemy enemy)
        {
            switch (type)
            {
                case 1:
                    sprite = GameWorld.commonSprites["healthBarRed"];
                    break;
            }
            enemyHealthbar = true;
            this.layer = layer;
            this.enemy = enemy;
        }


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
            backgroundRectangle = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
            foregroundRectangle = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
        }

        public override void Update(GameTime gameTime)
        {
            if (enemyHealthbar == false)
                this.position = new Vector2(GameWorld.Camera.Position.X - 750, GameWorld.Camera.Position.Y - 460);
            if (enemyHealthbar == true)
                this.position = new Vector2(GameWorld.Camera.Position.X + 450, GameWorld.Camera.Position.Y - 460);


            UpdateHealth();
        }

        public void UpdateHealth()
        {
            // Calculate the width of the foreground based on the health percentage
            float healthPercentage = 100;
            if (enemyHealthbar == false)
                healthPercentage = (float)(GameWorld.PlayerInstance.Health / (float)(GameWorld.PlayerInstance.MaxHealth + GameWorld.PlayerInstance.HealthBonus));
            else if (enemyHealthbar == true)
                healthPercentage = (float)enemy.Health / 15f;
            foregroundRectangle.Width = (int)(backgroundRectangle.Width * healthPercentage);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!GameWorld.DetectInOutro() && !GameWorld.DetectInventory())
            {
                // Draw the background
                spriteBatch.Draw(GameWorld.commonSprites["healthBarBlack"], position, backgroundRectangle, Color.White, rotation, Vector2.Zero, scale, SpriteEffects.None, layer);

                // Draw the foreground (current health)
                spriteBatch.Draw(sprite, position, foregroundRectangle, Color.White, rotation, Vector2.Zero, scale, SpriteEffects.None, layer + 0.1f);
            }
        }
        #endregion

    }
}
