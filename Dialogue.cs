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
    internal class Dialogue : GameObject
    {

        #region fields
        private string nPCText;

        private List<string> dialogueLines;  // Stores the lines of dialogue
        private int currentLineIndex;        // Tracks the current line being displayed
        private SpriteFont dialogueFont;             // Font for displaying text
        private Vector2 textPosition;      // Where the text is drawn
        private Vector2 boxPosition = new Vector2(100, 1000);         // Where the dialogue box is drawn

        #endregion

        #region properties

        #endregion

        #region constructor
        public Dialogue(Vector2 placement)
        {
            this.position = placement;
            sprite = GameWorld.commonSprites["dialogueBox"];
            Vector2 boxPosition = new Vector2(100, 1000);
            Vector2 textPosition = new Vector2(100, 1000);

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
            
        }

        public void NPCdialogue()
        { 
        switch (nPCText)
            {
                case "Nun":
                    //if (QuestItem)
                    //{ 
                    //    //skriv dette
                    //}
                    //else 
                    //skriv dette

                    Console.WriteLine("Npc tekst 1");
                    break;
                case "Munk":
                    Console.WriteLine("Npc tekst 2");

                    break;
                case "Boss":
                    Console.WriteLine("Npc tekst 3");

                    break;
                case "Letter":
                    Console.WriteLine("Npc tekst 4");

                    break;
                default:
                    Console.WriteLine("This NPC has no further quests");

                    break;
            }


        }

        #endregion
    }
}
