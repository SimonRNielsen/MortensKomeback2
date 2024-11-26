using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace MortensKomeback2
{
    internal class MainHandItem : Item, ITwoHandedItem
    {
        #region Fields

        private bool isTwoHanded = false;
        private readonly ImplementTwohanded implementTwohanded = new ImplementTwohanded();
        private readonly ITwoHandedItem implement;

        #endregion

        #region Properties


        public bool IsTwoHanded { get => isTwoHanded; }

        #endregion

        #region Constructor


        public MainHandItem(int playerClass)
        {
            damageBonus = 5;
            switch (playerClass)
            {
                case 1:
                    sprite = GameWorld.commonSprites["mainHandItem"]; //Fighter
                    itemName = "Sword";
                    break;
                case 2:
                    sprite = GameWorld.commonSprites["mainHandItem"]; //Ranger
                    itemName = "Sling";
                    implement = implementTwohanded;
                    break;
                case 3:
                    sprite = GameWorld.commonSprites["mainHandItem"]; //Mage
                    itemName = "Staff";
                    implement = implementTwohanded;
                    break;
                
            }
        }


        #endregion

        #region Methods


        public override void LoadContent(ContentManager content)
        {
            if (implement is ITwoHandedItem && !isTwoHanded)
            {
                isTwoHanded = true;
                damageBonus = implement.StatBoost(damageBonus);
            }
        }

        public override void OnCollision(GameObject gameObject)
        {
            //
        }

        public override void Update(GameTime gameTime)
        {
            //
        }

        #endregion
    }
}
