using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MortensKomeback2
{
    internal class MousePointer
    {
        #region Fields

        private bool inventoryOpen = false;
        private bool detectItem = false;
        private bool rightClickActive = false;

        #endregion

        #region Properties

        /// <summary>
        /// Returns a 1x1 pixel CollisionBox at the tip of the mousepointer
        /// </summary>
        public Rectangle CollisionBox
        {
            get { return new Rectangle((int)GameWorld.MousePosition.X, (int)GameWorld.MousePosition.Y, 1, 1); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a MousePointer with the intent of enabling "collision" with Button-class objects
        /// </summary>
        /// <param name="graphics">Needed for translating precise location of mouse in comparison to game</param>
        public MousePointer()
        {

        }

        #endregion

        #region Methods

        public void RightClickEvent()
        {
            inventoryOpen = GameWorld.DetectInventory();
            rightClickActive = GameWorld.DetectRightClickMenu();

            if (!inventoryOpen && !rightClickActive)
            {
                GameWorld.newGameObjects.Add(new Button(GameWorld.MousePosition, 10));
            }

            else
            {

                if (!rightClickActive && !GameWorld.MenuActive)
                {
                    foreach (Item item in GameWorld.playerInventory)
                        CheckCollision(item);
                    foreach (Item item in GameWorld.equippedPlayerInventory)
                        CheckCollision(item);
                }

                if (GameWorld.RightMouseButtonClick && !GameWorld.MenuActive && !detectItem)
                {
                    GameWorld.newGameObjects.Add(new Button(GameWorld.MousePosition, 0));
                }

                detectItem = false;
            }
        }


        public void CheckCollision(Item item)
        {
            if (CollisionBox.Intersects(item.CollisionBox))
            {
                detectItem = true;
                GameWorld.newGameObjects.Add(new Button(GameWorld.MousePosition, ref item));
            }
        }

        #endregion
    }
}