using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace MortensKomeback2
{
    internal class OffHandItem : Item
    {
        #region Fields



        #endregion

        #region Properties



        #endregion

        #region Constructor


        public OffHandItem(int playerClass, bool enhanced, bool found)
        {
            standardSprite = GameWorld.commonSprites["offHandItem"];
            if (found)
                sprite = standardSprite;
            else
                sprite = GameWorld.commonSprites["blink"];
            layer = 0.95f;
            damageReductionBonus = 5;
            if (enhanced)
            {
                itemName = "Enhanced ";
                damageReductionBonus = (int)(damageReductionBonus * 1.6f);
            }
            switch (playerClass)
            {
                case 1:
                    itemName += "Shield";
                    break;
                case 2:
                    isUsed = true;
                    break;
                case 3:
                    isUsed = true;
                    break;
            }
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
