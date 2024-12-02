﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MortensKomeback2
{
    internal class MousePointer
    {
        #region Fields

        private bool detectItem = false;

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
        /// Constructs a MousePointer with the intent of enabling "collision" with Button- & Item-class objects
        /// </summary>
        public MousePointer()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles input dependant on different conditions when run (initiated in GameWorld by right-clicking with mouse)
        /// </summary>
        public void RightClickEvent()
        {

            if (!GameWorld.DetectInventory() && !GameWorld.DetectRightClickMenu() && !GameWorld.MenuActive)
            {
                GameWorld.newGameObjects.Add(new Button(GameWorld.MousePosition, 10));
            }

            else
            {

                if (!GameWorld.DetectRightClickMenu() && GameWorld.DetectInventory())
                {
                    foreach (Item item in GameWorld.playerInventory)
                        CheckCollision(item);
                    foreach (Item item in GameWorld.equippedPlayerInventory)
                        CheckCollision(item);
                }

                /* No current conditions where this should be possible at the moment
                if (GameWorld.RightMouseButtonClick && !GameWorld.MenuActive && !detectItem)
                {
                    GameWorld.newGameObjects.Add(new Button(GameWorld.MousePosition, 0));
                }
                */

                detectItem = false;
            }

        }

        /// <summary>
        /// Detects if the mouse is colliding with the item and creates a button to handle that item
        /// </summary>
        /// <param name="item">Item being collided with</param>
        public void CheckCollision(Item item)
        {

            if (!(item is QuestItem))
                if (CollisionBox.Intersects(item.CollisionBox))
                {
                    detectItem = true;
                    GameWorld.newGameObjects.Add(new Button(GameWorld.MousePosition, ref item));
                }

        }

        /// <summary>
        /// Runs a Mouse-Over effect by activating the items OnCollision function, as long as it's not of the "QuestItem" class (should never happen so no need to check collision there)
        /// </summary>
        public void MouseOver()
        {

            foreach (Item item in GameWorld.playerInventory)
                if (!(item is QuestItem))
                    if (item.CollisionBox.Intersects(CollisionBox))
                        item.OnCollision();

            foreach (Item item in GameWorld.equippedPlayerInventory)
                if (!(item is QuestItem))
                    if (item.CollisionBox.Intersects(CollisionBox))
                        item.OnCollision();

        }

        #endregion
    }
}