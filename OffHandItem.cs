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


        public OffHandItem(int playerclass)
        {
            sprite = GameWorld.commonSprites["offHandItem"];
            damageReductionBonus = 5;
            switch (playerclass)
            {
                case 1:
                    itemName = "Shield";
                    break;
                case 2:
                    health = 0;
                    break;
                case 3:
                    health = 0;
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
