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

        /// <summary>
        /// Constructor for QuestItem class - also used for healing items
        /// </summary>
        /// <param name="itemType">0 = Key (secondary quest objective), 1 = Blood of Geesus (healing item), 2 = Popes sceptre (main quest objective), 3 = Monks bible, 4 = Nuns rosary</param>
        /// <param name="found">Set to true if already in players inventory</param>
        /// <param name="spawnPosition">Used to set spawnposition</param>
        public QuestItem(int itemType, bool found, Vector2 spawnPosition)
        {

            position = spawnPosition;
            layer = 0.95f;
            switch (itemType)
            {
                case 0:
                    sprite = GameWorld.commonSprites["key"];
                    itemName = "Key";
                    isUseable = true;
                    isKey = true;
                    break;
                case 1:
                    sprite = GameWorld.commonSprites["potion"];
                    itemName = "Blood of Geesus";
                    isUseable = true;
                    healItem = true;
                    break;
                case 2:
                    sprite = GameWorld.commonSprites["scepter"];
                    itemName = "Popes sceptre";
                    break;
                case 3:
                    sprite = GameWorld.commonSprites["bible"];
                    itemName = "Monks bible";
                    break;
                case 4:
                    sprite = GameWorld.commonSprites["rosary"];
                    itemName = "Nuns rosary";
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
