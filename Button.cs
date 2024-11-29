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
    internal class Button : Menu
    {
        #region Fields

        private Item playerItem;
        private Color[] backgroundColor = new Color[2] { Color.White, Color.Gray };
        private int backgroundColorIndex;
        private int buttonID;
        private bool collision = false;
        private bool itemButton = false;

        #endregion

        #region Properties

        public override Rectangle CollisionBox
        {
            get { return new Rectangle((int)Position.X - (sprite.Width / 2), (int)Position.Y - (sprite.Height / 2), sprite.Width, sprite.Height); }
        }
        public bool ItemButton { get => itemButton; }

        #endregion

        #region Constructors

        public Button(Vector2 spawnPosition, int buttonType)
        {
            position = spawnPosition;
            layer = 0.998f;
            buttonID = buttonType;
            switch (buttonID)
            {
                case 0: //Not currently in use
                    sprite = GameWorld.commonSprites["button"];
                    text = "Unpause";
                    textXDisplacement = -18;
                    break;
                case 1:
                    sprite = GameWorld.commonSprites["button"];
                    text = "Close";
                    textXDisplacement = -7;
                    break;
                case 2:
                    sprite = GameWorld.commonSprites["button"];
                    text = "Exit";
                    textXDisplacement = +2;
                    break;
                case 3:
                    sprite = GameWorld.commonSprites["button"];
                    text = "Start";
                    textXDisplacement = -7;
                    break;
                case 4:
                    sprite = GameWorld.commonSprites["button"];
                    text = "Restart";
                    textXDisplacement = -25;
                    break;
                case 10:
                    sprite = GameWorld.commonSprites["menuButton"];
                    text = "Inventory";
                    textXDisplacement = -38;
                    itemButton = true;
                    GameWorld.newGameObjects.Add(new Button(new Vector2(position.X, position.Y + sprite.Height), 11));
                    break;
                case 11:
                case 101:
                    sprite = GameWorld.commonSprites["menuButton"];
                    text = "Cancel";
                    textXDisplacement = -13;
                    itemButton = true;
                    break;
            }
        }

        public Button(Vector2 spawnPosition, ref Item item)
        {
            position = spawnPosition;
            playerItem = item;
            itemButton = true;
            buttonID = 100;
            layer = 0.998f;
            sprite = GameWorld.commonSprites["menuButton"];
            if (playerItem.IsEquipped)
            {
                text = "Unequip";
                textXDisplacement = -18;
            }
            else
            {
                text = "Equip";
            }
            GameWorld.newGameObjects.Add(new Button(new Vector2(position.X, position.Y + sprite.Height), 101));
        }

        #endregion

        #region Methods


        public void OnCollision()
        {
            collision = true;
            if (GameWorld.LeftMouseButtonClick)
                switch (buttonID)
                {
                    case 0:
                    case 1:
                        foreach (Item item in GameWorld.playerInventory)
                            item.Position = new Vector2(item.Position.X - 10000, item.Position.Y - 10000);
                        foreach (Item item in GameWorld.equippedPlayerInventory)
                            item.Position = new Vector2(item.Position.X - 10000, item.Position.Y - 10000);
                        GameWorld.CloseMenu = true;
                        break;

                    case 2:
                        GameWorld.ExitGame();
                        break;

                    case 3:
                        GameWorld.StartGame();
                        GameWorld.CloseMenu = true;
                        break;

                    case 4:
                        GameWorld.InitiateRestart();
                        GameWorld.CloseMenu = true;
                        break;

                    case 10:
                        GameWorld.newGameObjects.Add(new Menu(GameWorld.Camera.Position, 0));
                        GameWorld.MarkMenuItemsObsolete();
                        break;

                    case 11:
                        GameWorld.CloseMenu = true;
                        break;

                    case 100:
                        if (playerItem.IsEquipped)
                        {
                            playerItem.IsEquipped = false;
                            GameWorld.playerInventory.Add(playerItem);
                            GameWorld.equippedPlayerInventory.Remove(playerItem);
                        }
                        else
                        {
                            playerItem.IsEquipped = true;
                            GameWorld.equippedPlayerInventory.Add(playerItem);
                            GameWorld.playerInventory.Remove(playerItem);
                        }
                        GameWorld.MarkMenuItemsObsolete();
                        break;

                    case 101:
                        GameWorld.MarkMenuItemsObsolete();
                        break;

                }
        }


        public override void Update(GameTime gameTime)
        {
            switch (buttonID)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                    if (collision)
                        textColorIndex = 1;
                    else
                        textColorIndex = 0;
                    break;

                case 10:
                case 11:
                case 100:
                case 101:
                    if (collision)
                        backgroundColorIndex = 1;
                    else
                        backgroundColorIndex = 0;
                    break;

                default:
                    throw new NotImplementedException();
            }
            collision = false;
        }


        public void CheckCollision(MousePointer mousePointer)
        {
            if (CollisionBox.Intersects(mousePointer.CollisionBox))
            {
                OnCollision();
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(sprite, Position, null, backgroundColor[backgroundColorIndex], rotation, new Vector2(sprite.Width / 2, sprite.Height / 2), scale, SpriteEffects.None, layer);
            spriteBatch.DrawString(GameWorld.mortensKomebackFont, text, new Vector2(Position.X + textXDisplacement, Position.Y), textColor[textColorIndex], 0f, new Vector2(18, 8), 2f, SpriteEffects.None, 0.999f);

        }


        public override void LoadContent(ContentManager content)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
