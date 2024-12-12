using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MortensKomeback2
{
    public class NPC : Character
    {
        #region field
        private string[] npcClass = new string[3] {"Munk", "Nun", "Letter"};
        private int classSelection;
        private string spriteNPC;
        private bool textBubble = false;

        #endregion

        #region properti

        public string NPCClass { get => npcClass[classSelection]; set => NPCClass = value; }
        public bool TextBubble { get => textBubble; set => textBubble = value; }
     
    #endregion

    #region constructor
    public NPC(int npcClass, int spriteNPC, Vector2 placement)
        {
            classSelection = npcClass;
            position = placement;

            switch (spriteNPC)
            {
                case 0:
                    sprite = GameWorld.commonSprites["monkNPC"];
                    break;
                case 1:
                    sprite = GameWorld.commonSprites["nunNPC"];
                    break;
            }
        }
        #endregion

        #region method
        public override void LoadContent(ContentManager content)
        {
            //this.Sprite = content.Load<Texture2D>("Sprites\\Charactor\\goose0");
        }

        public override void OnCollision(GameObject gameObject)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Movement(GameTime gameTime)
        {
            //Is standing still
        }

        public override void Animation(GameTime gameTime)
        {
            //No animation needed
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (textBubble)
                spriteBatch.Draw(GameWorld.commonSprites["talkPrompt"], new Vector2(position.X - GameWorld.commonSprites["talkPrompt"].Width, position.Y - sprite.Height / 2 - GameWorld.commonSprites["talkPrompt"].Height * 2), null, Color.White, rotation, Vector2.Zero, scale * 2, objectSpriteEffects[0], layer + 0.1f);
            textBubble = false;
        }


        #endregion
    }
}
