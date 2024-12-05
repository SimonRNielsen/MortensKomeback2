using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

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

        /// <summary>
        /// Constructor for Button-class with visual presentation and functionality determined by "buttonType" int
        /// </summary>
        /// <param name="spawnPosition">Sets position for the Button</param>
        /// <param name="buttonType">0 = Unpause, 1 = Close, 2 = Exit, 3 = Start, 4 = Restart, 10 = Open inventory, 11 & 101 = Cancel (different stages)</param>
        public Button(Vector2 spawnPosition, int buttonType)
        {

            position = spawnPosition;
            layer = 0.998f;
            buttonID = buttonType;
            switch (buttonID)
            {
                case 0:
                    sprite = GameWorld.commonSprites["button"];
                    text = "Unpause";
                    layer = 0.9999f;
                    textXDisplacement = -25;
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
                    layer = 0.9999f;
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
                case 5:
                    sprite = GameWorld.commonSprites["button"];
                    text = "Crusader";
                    textXDisplacement = -28;
                    break;
                case 6:
                    sprite = GameWorld.commonSprites["button"];
                    text = "Monk";
                    textXDisplacement = -3;
                    break;
                case 7:
                    sprite = GameWorld.commonSprites["button"];
                    text = "Bishop";
                    textXDisplacement = -12;
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

        /// <summary>
        /// Overload of constructor for Button-class for use with "items"
        /// </summary>
        /// <param name="spawnPosition">Sets position for the Button</param>
        /// <param name="item">Takes "Item"-class reference to manipulate it according to input</param>
        public Button(Vector2 spawnPosition, ref Item item)
        {

            position = spawnPosition;
            playerItem = item;
            itemButton = true;
            buttonID = 100;
            layer = 0.998f;
            sprite = GameWorld.commonSprites["menuButton"];
            if (GameWorld.equippedPlayerInventory.Contains(playerItem))
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

        /// <summary>
        /// Applies mouse-over effect and "Button"-pressed logic according to pre-selected construction parameters
        /// </summary>
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

                    case 5:
                        GameWorld.PlayerInstance.IsAlive = false;
                        GameWorld.PlayerInstance = new Player(PlayerClass.Crusader, GameWorld.nPCs);
                        GameWorld.newGameObjects.Add(GameWorld.PlayerInstance);
                        GameWorld.CloseMenu = true;
                        break;

                    case 6:
                        GameWorld.PlayerInstance.IsAlive = false;
                        GameWorld.PlayerInstance = new Player(PlayerClass.Monk, GameWorld.nPCs);
                        GameWorld.newGameObjects.Add(GameWorld.PlayerInstance);
                        GameWorld.CloseMenu = true;
                        break;

                    case 7:
                        GameWorld.PlayerInstance.IsAlive = false;
                        GameWorld.PlayerInstance = new Player(PlayerClass.Bishop, GameWorld.nPCs);
                        GameWorld.newGameObjects.Add(GameWorld.PlayerInstance);
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
                        if (GameWorld.equippedPlayerInventory.Contains(playerItem))
                        {
                            playerItem.IsEquipped = false;
                            GameWorld.playerInventory.Add(playerItem);
                            GameWorld.equippedPlayerInventory.Remove(playerItem);
                            GameWorld.commonSounds["equipItem"].Play();
                        }
                        else
                        {
                            playerItem.IsEquipped = true;
                            GameWorld.equippedPlayerInventory.Add(playerItem);
                            GameWorld.playerInventory.Remove(playerItem);
                            GameWorld.commonSounds["equipItem"].Play();
                        }
                        GameWorld.MarkMenuItemsObsolete();
                        break;

                    case 101:
                        GameWorld.MarkMenuItemsObsolete();
                        break;

                }

        }

        /// <summary>
        /// Sets collision to false on each pass to check anew if mouse "collides" with the "Button", also applies the actual effect or resets it
        /// </summary>
        /// <param name="gameTime">Not used, but part of inherited class parameters</param>
        /// <exception cref="NotImplementedException">Non-valid Button has been constructed</exception>
        public override void Update(GameTime gameTime)
        {

            switch (buttonID)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
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

        /// <summary>
        /// Checks collision between CollisionBox'es
        /// </summary>
        /// <param name="mousePointer">Checks for position of MousePointers CollisionBox</param>
        public void CheckCollision(MousePointer mousePointer)
        {

            if (CollisionBox.Intersects(mousePointer.CollisionBox))
                OnCollision();

        }

        /// <summary>
        /// Overrides inherited Draw to also apply a DrawString and the visual mouse-over effect
        /// </summary>
        /// <param name="spriteBatch">Drawing logic</param>
        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(sprite, Position, null, backgroundColor[backgroundColorIndex], rotation, new Vector2(sprite.Width / 2, sprite.Height / 2), scale, SpriteEffects.None, layer);
            spriteBatch.DrawString(GameWorld.mortensKomebackFont, text, new Vector2(Position.X + textXDisplacement, Position.Y), textColor[textColorIndex], 0f, new Vector2(18, 8), 1.8f, SpriteEffects.None, layer + 0.00001f);

        }

        /// <summary>
        /// Not used for Button-class
        /// </summary>
        /// <param name="content">ContentManager logic</param>
        public override void LoadContent(ContentManager content)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
