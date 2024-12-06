using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace MortensKomeback2
{
    public abstract class Character : GameObject
    {
        #region field
        private int currentHealth;
        private HealthBar healthbar;

        #endregion

        #region properties
        protected int CurrentHealth { get => currentHealth; set => currentHealth = value; }

        #endregion

        #region constructor
        protected Character()
        { }

        #endregion

        #region method
        public override void LoadContent(ContentManager content)
        {
           
        }

        public override void OnCollision(GameObject gameObject)
        {
            
        }

        public override void Update(GameTime gameTime)
        {

        }

        public abstract void Movement(GameTime gameTime);


        public abstract void Animation(GameTime gameTime);

        
        #endregion
    }
}
