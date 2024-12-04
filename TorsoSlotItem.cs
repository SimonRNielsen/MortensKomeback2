using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace MortensKomeback2
{
    internal class TorsoSlotItem : Item
    {
        #region Fields



        #endregion

        #region Properties



        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for TorsoSlotItem class
        /// </summary>
        /// <param name="playerClass">Used to determine what class "Player" is and if any special logic should be applied</param>
        /// <param name="found">Set to true if already in players inventory</param>
        /// <param name="spawnPosition">Used to set spawnposition</param>
        public TorsoSlotItem(int playerClass, bool found, Vector2 spawnPosition)
        {

            position = spawnPosition;
            layer = 0.95f;
            switch (playerClass)
            {
                case 1:
                    damageReductionBonus = 10;
                    itemName = "Sturdy robe";
                    break;

                case 2:
                    damageReductionBonus = 5;
                    itemName = "Common robe";
                    healthBonus = 10;
                    break;

                case 3:
                    healthBonus = 20;
                    itemName = "Fancy robe";
                    break;
            }
            if (found)
                sprite = GameWorld.commonSprites["torsoItem"];
            else
            {
                sprite = GameWorld.commonSprites["blink"];
                standardSprite = GameWorld.commonSprites["torsoItem"];
            }

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
