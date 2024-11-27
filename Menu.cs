using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        private bool addButtons = false;
        private bool isInventory = false;
        protected bool isMenu = false;

        #endregion

        #region Properties

        public bool IsInventory { get => isInventory; }
        public bool IsMenu { get => isMenu; }

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
                int spacer = 0;
                foreach (Item item in GameWorld.playerInventory)
                {
                    item.Position = new Vector2(position.X - 800f, position.Y - 500f + (float)spacer);
                    spacer += item.Sprite.Height;
                }
                foreach (Item item in GameWorld.equippedPlayerInventory)
                {
                    if (item is MainHandItem)
                        item.Position = new Vector2(position.X + 150, position.Y - 200);
                    if (item is TorsoSlotItem)
                        item.Position = new Vector2(position.X + 300, position.Y - 200);
                    if (item is OffHandItem)
                        item.Position = new Vector2(position.X + 450, position.Y - 200);
                    if (item is FeetSlotItem)
                        item.Position = new Vector2(position.X + 300, position.Y);
                }
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

        #endregion
    }
}
