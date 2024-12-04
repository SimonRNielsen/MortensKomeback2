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
        #endregion

        //Constructor is instantiated in GameWorld 
        //There it will be given the right sprite
        #region Constructors
        public HealthBar(Vector2 placement, Texture2D initalSprite, float layer)
        {
            this.position = placement;
            this.sprite = initalSprite;
            this.layer = layer;
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
           
        }
        #endregion





    }
}
