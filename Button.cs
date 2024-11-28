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

        private Color[] buttonColor = new Color[3] { Color.DarkRed, Color.Yellow, Color.White };
        private Color[] backgroundColor = new Color[2] { Color.White, Color.Gray };
        private int buttonColorIndex;
        private int backgroundColorIndex;
        private int buttonID;
        private bool collision = false;
        private bool itemButton = false;
        private string buttonText;
        private float textXDisplacement;
        private Item playerItem;

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
                case 0:
                    sprite = GameWorld.commonSprites["button"];
                    buttonText = "Unpause";
                    textXDisplacement = -18;
                    break;
                case 1:
                    sprite = GameWorld.commonSprites["button"];
                    buttonText = "Close";
                    textXDisplacement = -7;
                    break;
                case 2:
                    sprite = GameWorld.commonSprites["button"];
                    buttonText = "Exit";
                    textXDisplacement = -3;
                    break;
                case 3:
                    sprite = GameWorld.commonSprites["button"];
                    buttonText = "Start";
                    textXDisplacement = -7;
                    break;
                case 4:
                    sprite = GameWorld.commonSprites["button"];
                    buttonText = "Restart";
                    textXDisplacement = -18;
                    break;
                case 10:
                    sprite = GameWorld.commonSprites["menuButton"];
                    buttonText = "Inventory";
                    textXDisplacement = -38;
                    itemButton = true;
                    GameWorld.newGameObjects.Add(new Button(new Vector2(position.X, position.Y + sprite.Height), 11));
                    break;
                case 11:
                case 101:
                    sprite = GameWorld.commonSprites["menuButton"];
                    buttonText = "Cancel";
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
                buttonText = "Unequip";
            }
            else
            {
                buttonText = "Equip";
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
                    if (collision)
                        buttonColorIndex = 1;
                    else
                        buttonColorIndex = 0;
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
            spriteBatch.DrawString(GameWorld.mortensKomebackFont, buttonText, new Vector2(Position.X + textXDisplacement, Position.Y), buttonColor[buttonColorIndex], 0f, new Vector2(18, 8), 2f, SpriteEffects.None, 0.999f);

        }


        public override void LoadContent(ContentManager content)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
