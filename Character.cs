using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace MortensKomeback2
{
    public abstract class Character : GameObject
    {
        #region field
        protected bool surfaceContact = false; //I don't know if I need it
        private int currentHealth;
        private HealthBar healthbar;
        private bool battleActive;

        #endregion

        #region properties
        protected int CurrentHealth { get => currentHealth; set => currentHealth = value; }

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
         


        }



        public abstract void Movement(GameTime gameTime);

        //void Interact(GameObject gameObject);

        public abstract void Animation(GameTime gameTime);

        //public virtual void GetHit(int damage)
        //{
        //    health -= damage;

        //    if (health <= 0) ;
        //        //isAlive = false;

        //}
    }
}
