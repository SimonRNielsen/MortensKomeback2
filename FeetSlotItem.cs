using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace MortensKomeback2
{
    internal class FeetSlotItem : Item
    {
        #region Fields



        #endregion

        #region Properties



        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for FeetSlotItem class
        /// </summary>
        /// <param name="playerClass">Used to determine what class "Player" is and if any special logic should be applied</param>
        /// <param name="found">Set to true if already in players inventory</param>
        /// <param name="spawnPosition">Used to set spawnposition</param>
        public FeetSlotItem(int playerClass, bool found, Vector2 spawnPosition)
        {
            if (found)
            {
                sprite = GameWorld.commonSprites["feetItem"];
            }
            else
            {
                standardSprite = GameWorld.commonSprites["feetItem"];
                sprite = GameWorld.commonSprites["blink"];
            }
            position = spawnPosition;
            itemName = "Boots of speed";
            layer = 0.95f;
            speedBonus = 5f;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="content">Not used</param>
        public override void LoadContent(ContentManager content)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="gameObject">Not Used</param>
        public override void OnCollision(GameObject gameObject)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
