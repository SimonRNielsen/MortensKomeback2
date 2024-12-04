using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace MortensKomeback2
{
    internal class MainHandItem : Item
    {
        #region Fields

        private bool isTwoHanded = false;
        private readonly ImplementTwoHanded implementTwohanded = new ImplementTwoHanded();
        private readonly ITwoHandedItem implement;

        #endregion

        #region Properties


        public bool IsTwoHanded { get => isTwoHanded; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for MainHandItem class
        /// </summary>
        /// <param name="playerClass">Used to determine what class "Player" is and if any special logic should be applied</param>
        /// <param name="spawnPosition">Used to set spawnposition</param>
        /// <param name="enhanced">If true, applies a 60% bonus to damageBonus</param>
        /// <param name="found">Set to true if already in players inventory</param>
        public MainHandItem(PlayerClass playerClass, Vector2 spawnPosition, bool enhanced, bool found)
        {

            damageBonus = 5;
            position = spawnPosition;
            layer = 0.95f;
            if (enhanced)
            {
                damageBonus = (int)(damageBonus * 1.6f);
                itemName = "Blessed ";
            }
            switch ((int)playerClass)
            {
                case 1:
                    sprite = GameWorld.commonSprites["mainHandItem"]; //Fighter
                    itemName += "Sword";
                    break;
                case 2:
                    sprite = GameWorld.commonSprites["mainHandItem"]; //Ranger
                    implement = implementTwohanded;
                    isTwoHanded = true;
                    damageBonus = implement.StatBoost(damageBonus);
                    itemName += "Sling";
                    break;
                case 3:
                    sprite = GameWorld.commonSprites["mainHandItem"]; //Mage
                    implement = implementTwohanded;
                    isTwoHanded = true;
                    damageBonus = implement.StatBoost(damageBonus);
                    itemName += "Staff";
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
            
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="gameObject">Not Used</param>
        public override void OnCollision(GameObject gameObject)
        {

        }

        #endregion
    }
}
