using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortensKomeback2
{
    internal class Menu : GameObject
    {
        #region Fields

        private int menuType;
        private static int keyCount;
        private static int healItem;
        private bool isInventory = false;
        protected bool isMenu = false;
        protected bool buttonObsolete = false;

        #endregion

        #region Properties

        public bool IsInventory { get => isInventory; }
        public bool IsMenu { get => isMenu; }
        public bool ButtonObsolete { get => buttonObsolete; set => buttonObsolete = value; }

        #endregion

        #region Constructor

        public Menu() { }

        public Menu(Vector2 position, int type)
        {
            isMenu = true;
            menuType = type;
            Position = position;
            layer = 0.9f;
            switch (menuType)
            {
                case 0:
                    isInventory = true;
                    sprite = GameWorld.commonSprites["inventory"];
                    GameWorld.newGameObjects.Add(new Button(new Vector2(Position.X, Position.Y + 300), 1));
                    break;
                case 1:
                    sprite = GameWorld.commonSprites["winScreen"];
                    break;
                case 2:
                    sprite = GameWorld.commonSprites["loseScreen"];
                    break;
                case 3:
                    sprite = GameWorld.commonSprites["introScreen"];
                    break;
            }
        }

        #endregion

        #region Methods


        public override void Update(GameTime gameTime)
        {
            if (isInventory)
            {
                ShowInventory();
                CountUseables();
            }
        }


        public override void LoadContent(ContentManager content)
        {
            //throw new NotImplementedException();
        }


        public override void OnCollision(GameObject gameObject)
        {
            //throw new NotImplementedException();
        }


        /// <summary>
        /// Modified to recieve individual sprite, position, rotation, scale, spriteeffects and layerdepth from each individual gameobject
        /// </summary>
        /// <param name="spriteBatch">Drawing tool</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position, null, drawColor, rotation, new Vector2(Sprite.Width / 2, Sprite.Height / 2), scale, objectSpriteEffects[SpriteEffectIndex], layer);
            switch (menuType)
            {
                case 0:

                    break;
                default:
                    break;
            }
        }

        private void ShowInventory()
        {
            int spacer = 0;
            int nextLine = 0;
            int addLineCounter = 0;
            foreach (Item item in GameWorld.playerInventory)
            {
                if (!(item is QuestItem))
                {
                    item.Position = new Vector2(position.X - 750 + (96 * nextLine), position.Y - 350 + spacer);
                    spacer += 96;
                    addLineCounter++;
                    if (addLineCounter == 9)
                    {
                        nextLine++;
                        addLineCounter = 0;
                        spacer = 0;
                    }
                }
            }
            foreach (Item item in GameWorld.equippedPlayerInventory)
            {
                if (item is MainHandItem)
                    item.Position = new Vector2(position.X + 300, position.Y - 200);
                if (item is TorsoSlotItem)
                    item.Position = new Vector2(position.X + 500, position.Y - 200);
                if (item is OffHandItem)
                    item.Position = new Vector2(position.X + 700, position.Y - 200);
                if (item is FeetSlotItem)
                    item.Position = new Vector2(position.X + 500, position.Y);
            }
        }

        private void CountUseables()
        {
            keyCount = 0;
            healItem = 0;
            foreach (Item item in GameWorld.playerInventory)
            {
                if (item is QuestItem)
                {
                    if ((item as QuestItem).IsKey)
                        keyCount++;
                    if ((item as QuestItem).HealItem)
                        healItem++;
                }
            }
        }
        #endregion
    }
}
