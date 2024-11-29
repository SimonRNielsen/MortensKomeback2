using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MortensKomeback2
{
    internal class QuestItem : Item
    {
        #region Fields

        private bool isKey = false;
        private bool healItem = false;

        #endregion

        #region Properties

        public bool IsKey { get => isKey; }
        public bool HealItem { get => healItem; }

        #endregion

        #region Constructor


        public QuestItem(int itemType, bool found)
        {
            switch (itemType)
            {
                case 0:
                    standardSprite = GameWorld.commonSprites["questItem"];
                    itemName = "Key";
                    isUseable = true;
                    isKey = true;
                    break;
                case 1:
                    standardSprite = GameWorld.commonSprites["questItem"];
                    itemName = "Gooseblood";
                    isUseable = true;
                    healItem = true;
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
