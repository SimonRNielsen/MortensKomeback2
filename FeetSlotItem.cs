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


        public FeetSlotItem(int playerClass)
        {
            sprite = GameWorld.commonSprites["feetItem"];
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

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
