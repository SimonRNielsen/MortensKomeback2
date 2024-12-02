using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortensKomeback2
{
    /// <summary>
    /// Overlay that shows status of health, ammo and quest items
    /// </summary>
    internal class GUI : GameObject
    {
        #region Fields
        private static int healthCount;
        private Vector2 heartPosition;
        private Vector2 weaponClassPosition; 
        private bool mortenAlive;
        #endregion

        #region Properties
        /// <summary>
        /// Accessing Players health
        /// </summary>
      //  public static int HealthCount { get => healthCount; set => healthCount = value; }

        /// <summary>
        /// Shows picked class sprite-weapon
        /// </summary>
       // public Texture2D WeaponSprite { get => weaponSprite; set => weaponSprite = value; }

        /// <summary>
        /// Shows quest items picked up. For example: Keys, rosary..
        /// </summary>
        //public Texture2D[] QuestItemSprite { get => questItemSprite; set => questItemSprite = value; }

        #endregion

        #region Constructor                                                   
        public GUI(Vector2 placement)
        {
            this.position = placement;
            heartPosition = new Vector2(200, 200);
            weaponClassPosition = new Vector2(200, 300);
        }
        #endregion
        
        #region Methods


        public override void LoadContent(ContentManager content)
        {
    
        }

        public override void OnCollision(GameObject gameObject)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
           

            //if (mortenAlive)
            //{ 

            //healthCount = GameWorld.PlayerInstance.Health;
            //}

            //// Update GUI state based on player status
            //if (GameWorld.PlayerInstance != null)
            //{
            //    mortenAlive = GameWorld.PlayerInstance.Health > 0;
            //    healthCount = GameWorld.PlayerInstance.Health;
            //}

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the heart icon for health
            spriteBatch.Draw(GameWorld.commonSprites["heartSprite"], heartPosition, Color.White);

            // Draw weapon icon (or other relevant GUI elements)
            spriteBatch.Draw(GameWorld.commonSprites["weaponClassSprite"], weaponClassPosition, Color.White);

            // Optionally, draw health as text
            //SpriteFont font = GameWorld.commonSprites["mortensKomebackFont"];
            spriteBatch.DrawString(GameWorld.mortensKomebackFont, $"Health: {healthCount}", new Vector2(60, 25), Color.Red);
        }





        #endregion
    }
}
