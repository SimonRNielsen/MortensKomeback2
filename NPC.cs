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
    internal class NPC : Character
    {
        #region field
        private string[] npcClass = new string[2] {"Munk", "Nun"};

        #endregion

        #region properti

        #endregion

        #region constructor
        public NPC()
        {


        }
        #endregion

        #region method
        public override void LoadContent(ContentManager content)
        {
            //this.Sprite = content.Load<Texture2D>("Sprites\\Charactor\\goose0");
        }

        public override void OnCollision(GameObject gameObject)
        {
            if (gameObject is Player)
            {
                //Load quest 
            }
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
