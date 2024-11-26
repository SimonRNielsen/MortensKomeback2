using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MortensKomeback2
{
    internal class QuestItem : Item
    {
        #region Fields

        private bool isUsed = false;

        #endregion

        #region Properties

        public bool IsUsed { get => isUsed; set => isUsed = value; }

        #endregion

        #region Constructor


        public QuestItem()
        {
            sprite = GameWorld.commonSprites["questItem"];
            itemName = "Key";
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
            if (isUsed)
                health = 0;
        }

        #endregion
    }
}
