using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MortensKomeback2
{
    /// <summary>
    /// Overlay that shows status of health, ammo and quest items
    /// </summary>
    internal class GUI : GameObject
    {
        #region Fields
        string keyBindings;
        Vector2 helpTextPosition = new Vector2(0,0);
        private Color textHeaderColor;
        private Vector2 textOrigin;
        private float textScale;

        //private static int healthCount;
        //private Vector2 heartPosition;
        //private Vector2 weaponClassPosition; 
        #endregion

        #region Properties
        /// <summary>
        /// Accessing Players health
        /// </summary>
        //  public static int HealthCount { get => healthCount; set => healthCount = value; }

        /// <summary>
        /// Shows picked class sprite-weapon
        /// </summary>
        // public Texture2D WeaponSprite { get => weaponSprite; set => weaponSprite = value; }

        /// <summary>
        /// Shows quest items picked up. For example: Keys, rosary..
        /// </summary>
        //public Texture2D[] QuestItemSprite { get => questItemSprite; set => questItemSprite = value; }

        #endregion

        #region Constructor                                                   
        public GUI()
        {
            layer = 1;
            position = helpTextPosition;
            keyBindings = "WASD to move \n P to pray for finding items \n E to interact and pick up items \n ENTER to close dialog \n I to open inventory \n You can also use left mousebutton to open and navigate inventory";

        }
        #endregion

        #region Methods


        public override void LoadContent(ContentManager content)
        {

        }

        public override void OnCollision(GameObject gameObject)
        {

        }

        public override void Update(GameTime gameTime)
        {


        }
      


        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawString(GameWorld.mortensKomebackFont, keyBindings, helpTextPosition, textHeaderColor, 0, textOrigin, textScale, SpriteEffects.None, layer + 1f);

        }
        #endregion
    }
}
