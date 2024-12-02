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


        public FeetSlotItem(int playerClass, bool found)
        {
            standardSprite = GameWorld.commonSprites["feetItem"];
            if (found)
            {
                sprite = standardSprite;
            }
            else
            {
                sprite = GameWorld.commonSprites["blink"];
            }
            itemName = "Boots of speed";
            layer = 0.95f;
            speedBonus = 5f;
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
