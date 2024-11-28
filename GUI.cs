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
        private Texture2D heartSprite;
        private Texture2D weaponSprite;
        //private Texture2D[] questItemSprite;
        private static int healthCount;
        private Vector2 heartPosition;
        private Vector2 weaponPosition;
        private Vector2 questItemPosition;
        private SpriteFont mortalKombatFont;

       

        #endregion

        #region Properties
        /// <summary>
        /// Accessing Players health
        /// </summary>
        public static int HealthCount { get => healthCount; set => healthCount = value; }

        /// <summary>
        /// Shows picked class sprite-weapon
        /// </summary>
        public Texture2D WeaponSprite { get => weaponSprite; set => weaponSprite = value; }

        /// <summary>
        /// Shows quest items picked up. For example: Keys, rosary..
        /// </summary>
        //public Texture2D[] QuestItemSprite { get => questItemSprite; set => questItemSprite = value; }

        #endregion

        #region Constructor
        public GUI(/*int xPosition, int yPosition*/)
        {
            //this.position.X = xPosition;
            //this.position.Y = yPosition;
            heartPosition.Y = -500;
            heartPosition.X = -900;
            weaponPosition.Y = -400;
            weaponPosition.X = -900;
            questItemPosition.Y = -300;
            questItemPosition.X = -900;
            
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
            
              
        }
        #endregion
    }
}
