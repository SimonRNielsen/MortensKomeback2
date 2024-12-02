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


        public TorsoSlotItem(int playerClass, bool found)
        {
            standardSprite = GameWorld.commonSprites["torsoItem"];
            layer = 0.95f;
            switch (playerClass)
            {
                case 1:
                    damageReductionBonus = 10;
                    itemName = "Sturdy robe";
                    break;

                case 2:
                    damageReductionBonus = 5;
                    itemName = "Flexible robe";
                    healthBonus = 10;
                    break;
                case 3:
                    healthBonus = 20;
                    itemName = "Comfortable robe";
                    break;
            }
            if (found)
                sprite = standardSprite;
            else
                sprite = GameWorld.commonSprites["blink"];
        }

        #endregion

        #region Methods


        public override void LoadContent(ContentManager content)
        {
            //throw new NotImplementedException();
        }

        public override void OnCollision(GameObject gameObject)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
