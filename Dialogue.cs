using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MortensKomeback2
{
    internal class Dialogue : GameObject
    {

        #region fields
        private string nPCText;

        private List<string> dialogueLines;  // Stores the lines of dialogue
        private int currentLineIndex;        // Tracks the current line being displayed
        private SpriteFont dialogueFont;             // Font for displaying text
        private Vector2 textPosition;      // Where the text is drawn
        private Vector2 boxPosition;        // Where the dialogue box is drawn
        private bool dialogue;
        private bool bossFight = false;
        private float gracePeriod = 1f;
        private float grace;


        #endregion

        #region properties

        #endregion

        #region constructor
        public Dialogue(Vector2 placement)
        {
            this.position = placement;
            sprite = GameWorld.commonSprites["dialogueBox"];
            //Vector2 boxPosition = new Vector2(100, 1000);
            Vector2 textPosition = new Vector2(100, 1000);

        }

        public Dialogue(Vector2 placement, Character character)
        {
            this.position = placement;
            sprite = GameWorld.commonSprites["dialogueBox"];
            Vector2 textPosition = new Vector2(position.X, position.Y);
            NPCDialogue(character);
            dialogue = true;
            layer = 0.8f;
            if (character is NPC)
                if ((character as NPC).NPCClass == "Letter")
                    bossFight = true;
        }

        #endregion

        #region methods

        public override void LoadContent(ContentManager content)
        {

        }

        public override void OnCollision(GameObject gameObject)
        {

        }

        public override void Update(GameTime gameTime)
        {
            grace += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (GameWorld.PlayerInstance.CloseDialog)
            {
                GameWorld.Dialogue = false;
                isAlive = false;
            }
            else if (dialogue == false && Keyboard.GetState().IsKeyDown(Keys.Enter) == true)
            {
                GameWorld.Dialogue = false;
                isAlive = false;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Enter) == true && bossFight && grace >= gracePeriod)
            {
                GameWorld.Dialogue = false;
                isAlive = false;
            }

        }

        public void NPCDialogue(Character character)
        {
            GameWorld.Dialogue = true;
            QuestItem tempItem;
            string questItem;
            string caseName = string.Empty;
            if (character is NPC)
                caseName = (character as NPC).NPCClass;
            if (character is Boss)
                caseName = "Boss";
            switch (caseName)
            {
                case "Munk":
                    questItem = "Monks bible";
                    tempItem = FindQuestItem(GameWorld.playerInventory, questItem);
                    if (tempItem != null)
                    {
                        nPCText = "Thank you for finding my bible for me! Here, have this key";
                        GameWorld.playerInventory.Add(new QuestItem(0, true, Vector2.Zero));
                        GameWorld.playerInventory.Remove(tempItem);
                        GameWorld.commonSounds["equipItem"].Play();
                        character.Sprite = GameWorld.commonSprites["monkNPCbible"];
                    }
                    else if (FindQuestItem(GameWorld.playerInventory, questItem) == null && FindQuestItem(GameWorld.hiddenItems, questItem) == null)
                        nPCText = "*This NPC has no further quests*\nPress enter to continue";
                    else
                        nPCText = $"Hi {GameWorld.PlayerInstance.PlayerClass}, the geese in the nave of the cathedral chased me down whilst I was reading my bible. \nI had to flee in here - but I forgot my bible! \nWill you please fetch it for me? I'm too scared to go in there again \n\nOh and if you are having trouble finding things, just ask the Lord for help and PRAY by pressing 'p'\n\n\n\nPress 'ENTER' to close";
                    break;
                case "Nun":
                    questItem = "Nuns rosary";
                    tempItem = FindQuestItem(GameWorld.playerInventory, questItem);
                    if (tempItem != null)
                    {
                        nPCText = "Thank you for finding my rosary for me! Here, have this key";
                        GameWorld.playerInventory.Add(new QuestItem(0, true, Vector2.Zero));
                        GameWorld.playerInventory.Remove(tempItem);
                        GameWorld.commonSounds["equipItem"].Play();
                        character.Sprite = GameWorld.commonSprites["nunNPCrosary"];
                    }
                    else if (FindQuestItem(GameWorld.playerInventory, questItem) == null && FindQuestItem(GameWorld.hiddenItems, questItem) == null)
                        nPCText = "*This NPC has no further quests*\nPress enter to continue";
                    else
                        nPCText = $"Hi {GameWorld.PlayerInstance.PlayerClass}, those evil geese have stolen my rosary so now I can't pray, \ncould you defeat them and get it back for me?\n\nI think it might have left it in the furthest room so you have to look for it afterwards.\nOh and I've heard someone left a key in there too.\n\n\n\nPress 'ENTER' to close";
                    break;
                case "Boss":
                    nPCText = "I AM GOOSIFER! *Evil honks*\nNOW YOU DIE!!!";
                    break;
                case "Letter":
                    nPCText = "I GOT IT! Goosifer was the culprit who stole the popes sceptre, now I can send it back to him with divine magic from the next room!\n*To win proceed into the next room down (needs key)*";
                    break;
                default:
                    nPCText = "This NPC has no quests";
                    break;
            }

        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (dialogue)
                spriteBatch.DrawString(GameWorld.mortensKomebackFont, nPCText, new Vector2(GameWorld.Camera.Position.X + textPosition.X - (1920 / 2) + 120, GameWorld.Camera.Position.Y + textPosition.Y - (1080 / 2) + 715), GameWorld.GrayGoose, 0f, Vector2.Zero, 1.9f, SpriteEffects.None, layer + 0.2f);
        }


        private QuestItem FindQuestItem(List<Item> list, string itemName)
        {
            return list.FindAll(item => item is QuestItem).ConvertAll(questItem => (QuestItem)questItem).Find(questItem => questItem.ItemName == itemName);
        }


        #endregion
    }
}
