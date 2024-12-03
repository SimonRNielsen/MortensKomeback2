using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortensKomeback2
{
    public abstract class Character : GameObject
    {
        #region field
        protected bool surfaceContact = false; //I don't know if I need it
        protected int damage;


        #endregion

        #region Properties
        /// <summary>
        /// Property for accessing the base damage a character can deal to other characters
        /// </summary>
        public int Damage { get => damage; set => damage = value; }
        #endregion

        public override void LoadContent(ContentManager content)
        {
           
        }

        public override void OnCollision(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }



        public abstract void Movement(GameTime gameTime);

        //void Interact(GameObject gameObject);

        public abstract void Animation(GameTime gameTime);
    }
}
