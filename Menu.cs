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

        private Texture2D twoHandedSprite;
        private static int keyCount;
        private static int healItem;
        private int menuType;
        protected int textColorIndex;
        private bool isInventory = false;
        private bool drawTwoHanded = false;
        protected bool isMenu = false;
        protected bool buttonObsolete = false;
        protected string text;
        protected float textXDisplacement;
        protected Color[] textColor = new Color[3] { Color.DarkRed, Color.Yellow, Color.White };

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
                    GameWorld.newGameObjects.Add(new Button(new Vector2(Position.X, Position.Y + 400), 1));
                    break;
                case 1:
                    sprite = GameWorld.commonSprites["winScreen"];
                    GameWorld.newGameObjects.Add(new Button(new Vector2(Position.X - 300, Position.Y + 500), 4));
                    GameWorld.newGameObjects.Add(new Button(new Vector2(Position.X + 300, Position.Y + 500), 2));
                    break;
                case 2:
                    sprite = GameWorld.commonSprites["loseScreen"];
                    GameWorld.newGameObjects.Add(new Button(new Vector2(Position.X - 300, Position.Y + 500), 4));
                    GameWorld.newGameObjects.Add(new Button(new Vector2(Position.X + 300, Position.Y + 500), 2));
                    break;
                case 3:
                    sprite = GameWorld.commonSprites["introScreen"];
                    GameWorld.newGameObjects.Add(new Button(new Vector2(Position.X - 300, Position.Y + 500), 3));
                    GameWorld.newGameObjects.Add(new Button(new Vector2(Position.X + 300, Position.Y + 500), 2));
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
            base.Draw(spriteBatch);
            switch (menuType)
            {
                case 0:
                    if (drawTwoHanded)
                        spriteBatch.Draw(twoHandedSprite, new Vector2(position.X + 700 - (twoHandedSprite.Width / 2), position.Y - 200 - (twoHandedSprite.Height / 2)), null, drawColor, rotation, Vector2.Zero, scale, objectSpriteEffects[spriteEffectIndex], layer + 0.1f);
                    spriteBatch.Draw(GameWorld.PlayerInstance.Sprite, new Vector2(position.X - 400, position.Y - 400), null, drawColor, rotation, Vector2.Zero, scale * 3, objectSpriteEffects[spriteEffectIndex], layer + 0.1f);
                    spriteBatch.DrawString(GameWorld.mortensKomebackFont, "Backpack", new Vector2(position.X - 735, position.Y - 450), textColor[textColorIndex], 0f, new Vector2(18, 8), 2f, SpriteEffects.None, layer + 0.1f);
                    spriteBatch.Draw(GameWorld.commonSprites["healItem"], new Vector2(position.X + 300 - (GameWorld.commonSprites["healItem"].Width / 2), position.Y + 300 - (GameWorld.commonSprites["healItem"].Height / 2)), null, drawColor, rotation, Vector2.Zero, scale, objectSpriteEffects[spriteEffectIndex], layer + 0.1f);
                    spriteBatch.DrawString(GameWorld.mortensKomebackFont, $"Blood of Geesus:{healItem}", new Vector2(position.X + 400, position.Y + 300), textColor[textColorIndex], 0f, new Vector2(18, 8), 2f, SpriteEffects.None, layer + 0.1f);
                    spriteBatch.Draw(GameWorld.commonSprites["questItem"], new Vector2(position.X + 300 - (GameWorld.commonSprites["questItem"].Width / 2), position.Y + 425 - (GameWorld.commonSprites["questItem"].Height / 2)), null, drawColor, rotation, Vector2.Zero, scale, objectSpriteEffects[spriteEffectIndex], layer + 0.1f);
                    spriteBatch.DrawString(GameWorld.mortensKomebackFont, $"Keys:{keyCount}", new Vector2(position.X + 400, position.Y + 425), textColor[textColorIndex], 0f, new Vector2(18, 8), 2f, SpriteEffects.None, layer + 0.1f);
                    spriteBatch.DrawString(GameWorld.mortensKomebackFont, "Mainhand", new Vector2(position.X + 270, position.Y - 125), textColor[textColorIndex], 0f, new Vector2(18, 8), 2f, SpriteEffects.None, layer + 0.1f);
                    spriteBatch.DrawString(GameWorld.mortensKomebackFont, "Torso", new Vector2(position.X + 495, position.Y - 125), textColor[textColorIndex], 0f, new Vector2(18, 8), 2f, SpriteEffects.None, layer + 0.1f);
                    spriteBatch.DrawString(GameWorld.mortensKomebackFont, "Offhand", new Vector2(position.X + 680, position.Y - 125), textColor[textColorIndex], 0f, new Vector2(18, 8), 2f, SpriteEffects.None, layer + 0.1f);
                    spriteBatch.DrawString(GameWorld.mortensKomebackFont, "Feet", new Vector2(position.X + 500, position.Y + 75), textColor[textColorIndex], 0f, new Vector2(18, 8), 2f, SpriteEffects.None, layer + 0.1f);
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
            keyCount = 0;
            healItem = 0;

            foreach (Item item in GameWorld.playerInventory)
            {
                if (!(item is QuestItem))
                {
                    item.Position = new Vector2(position.X - 752 + (96 * nextLine), position.Y - 352 + spacer);
                    spacer += 96;
                    addLineCounter++;
                    if (addLineCounter == 9)
                    {
                        nextLine++;
                        addLineCounter = 0;
                        spacer = 0;
                    }
                }
                else if (item is QuestItem)
                {
                    if ((item as QuestItem).IsKey)
                        keyCount++;
                    if ((item as QuestItem).HealItem)
                        healItem++;
                }
            }

            int mainHandItem = 0;
            int offHandItem = 0;
            int torsoItem = 0;
            int feetItem = 0;
            Item tempItem;
            drawTwoHanded = false;

            do
            {
                if (mainHandItem > 1)
                {
                    tempItem = GameWorld.equippedPlayerInventory.Find(item => item is MainHandItem);
                    tempItem.IsEquipped = false;
                    GameWorld.playerInventory.Add(tempItem);
                    GameWorld.equippedPlayerInventory.Remove(tempItem);
                }
                if (offHandItem > 1)
                {
                    tempItem = GameWorld.equippedPlayerInventory.Find(item => item is OffHandItem);
                    tempItem.IsEquipped = false;
                    GameWorld.playerInventory.Add(tempItem);
                    GameWorld.equippedPlayerInventory.Remove(tempItem);
                }
                if (torsoItem > 1)
                {
                    tempItem = GameWorld.equippedPlayerInventory.Find(item => item is TorsoSlotItem);
                    tempItem.IsEquipped = false;
                    GameWorld.playerInventory.Add(tempItem);
                    GameWorld.equippedPlayerInventory.Remove(tempItem);
                }
                if (feetItem > 1)
                {
                    tempItem = GameWorld.equippedPlayerInventory.Find(item => item is FeetSlotItem);
                    tempItem.IsEquipped = false;
                    GameWorld.playerInventory.Add(tempItem);
                    GameWorld.equippedPlayerInventory.Remove(tempItem);
                }

                mainHandItem = 0;
                offHandItem = 0;
                torsoItem = 0;
                feetItem = 0;

                foreach (Item item in GameWorld.equippedPlayerInventory)
                {
                    if (item is MainHandItem)
                    {
                        if ((item as MainHandItem).IsTwoHanded)
                        {
                            drawTwoHanded = true;
                            twoHandedSprite = item.Sprite;
                        }
                        item.Position = new Vector2(position.X + 300, position.Y - 200);
                        mainHandItem++;
                    }
                    if (item is TorsoSlotItem)
                    {
                        item.Position = new Vector2(position.X + 500, position.Y - 200);
                        torsoItem++;
                    }
                    if (item is OffHandItem)
                    {
                        item.Position = new Vector2(position.X + 700, position.Y - 200);
                        offHandItem++;
                    }
                    if (item is FeetSlotItem)
                    {
                        item.Position = new Vector2(position.X + 500, position.Y);
                        feetItem++;
                    }
                }
            } while (mainHandItem > 1 || offHandItem > 1 || torsoItem > 1 || feetItem > 1);

        }

        #endregion
    }
}
