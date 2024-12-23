﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MortensKomeback2
{
    internal class OffHandItem : Item
    {
        #region Fields



        #endregion

        #region Properties



        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for OffHandItem class
        /// </summary>
        /// <param name="playerClass">Used to determine what class "Player" is and if any special logic should be applied</param>
        /// <param name="enhanced">If true, applies a 60% bonus to damageBonus</param>
        /// <param name="found">Set to true if already in players inventory</param>
        /// <param name="spawnPosition">Used to set spawnposition</param>
        public OffHandItem(PlayerClass playerClass, Vector2 spawnPosition, bool enhanced, bool found)
        {

            sprite = GameWorld.commonSprites["shield"];
            layer = 0.95f;
            damageReductionBonus = 5;
            position = spawnPosition;
            if (enhanced)
            {
                itemName = "Blessed ";
                damageReductionBonus = (int)(damageReductionBonus * 1.6f);
            }
            switch ((int)playerClass)
            {
                case 1:
                    itemName += "Shield";
                    break;
                case 2:
                    isUsed = true; //Deletes item - class doesn't use items in off-hand
                    break;
                case 3:
                    isUsed = true; //Deletes item - class doesn't use items in off-hand
                    break;
            }
            if (!found)
            {
                standardSprite = sprite;
                sprite = GameWorld.commonSprites["blink"];
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
