using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //Vector2 boxPosition = new Vector2(100, 1000);
            Vector2 textPosition = new Vector2(position.X, position.Y);
            NPCDialogue(character);
            dialogue = true;
            layer = layer + 0.1f;
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

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                GameWorld.Dialogue = false;
                isAlive = false;
            }

        }

        public void NPCDialogue(Character character)
        {
            GameWorld.Dialogue = true;
            string caseName = string.Empty;
            if (character is NPC)
                caseName = (character as NPC).NPCClass;
            if (character is Boss)
                caseName = "Boss";
            switch (caseName)
            {
                case "Munk":
                    nPCText = "Hej, jeg hedder Kaj, jeg er munk, folk kalder mig Kaj Munk";
                    break;
                case "Nun":
                    //if (QuestItem)
                    //{ 
                    //    //skriv dette
                    //      (character as NPC).NPCClass = "";
                    //}
                    //else
                    nPCText = "Jeg er foede for oekologisk-minded kannibaler";
                    break;
                case "Boss":
                    nPCText = "I AM GOOSIFER! *Evil laughter*";
                    break;
                case "Letter":
                    nPCText = "It's a mi, da pope-a";
                    break;
                default:
                    nPCText = "This NPC has no further quests";
                    break;
            }

        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (dialogue)
                spriteBatch.DrawString(GameWorld.mortensKomebackFont, nPCText, new Vector2(textPosition.X - (1920/2) + 100, textPosition.Y - (1080/2) + 700), GameWorld.GrayGoose, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, layer + 0.1f);
        }

        #endregion
    }
}
