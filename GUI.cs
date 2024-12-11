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
        private Vector2 helpTextPosition; 
        private string helpText;
        private string keyBindings;
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
        public GUI(Vector2 placement)
        {
            this.layer = 0.9f;
            this.position = placement;
        }
        #endregion
        
        #region Methods


        public override void LoadContent(ContentManager content)
        {
            helpText = "H - Help";
            keyBindings = "E - Interact with items or NPCs \n I - Inventory \n right-click on items to equip \n Enter - Close Dialogue \n P - Pray";

        }

        public override void OnCollision(GameObject gameObject)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            
         
        }
     
        #endregion
    }
}
