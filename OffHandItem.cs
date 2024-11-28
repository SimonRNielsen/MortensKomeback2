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


        public OffHandItem(int playerClass, bool enhanced)
        {
            sprite = GameWorld.commonSprites["offHandItem"];
            layer = 0.95f;
            damageReductionBonus = 5;
            if (enhanced)
                damageReductionBonus = (int)(damageReductionBonus * 1.6f);
            switch (playerClass)
            {
                case 1:
                    itemName = "Shield";
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

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
