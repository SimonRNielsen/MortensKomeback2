using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace MortensKomeback2
{
    public class NPC : Character
    {
        #region field
        private string[] npcClass = new string[3] {"Munk", "Nun", "Letter"};
        private int classSelection;

        #endregion

        #region properti

        public string NPCClass { get => npcClass[classSelection]; set => NPCClass = value; }

        #endregion

        #region constructor
        public NPC(int npcClass)
        {
            classSelection = npcClass;

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
            throw new NotImplementedException();
        }

        public override void Movement(GameTime gameTime)
        {
            //Is standing still
        }

        public override void Animation(GameTime gameTime)
        {
            //No animation needed
        }




        #endregion
    }
}
