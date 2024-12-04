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
        private int maxHealth;
        private int currentHealth;
        private HealthBar healthbar;
        private bool battleActive;

        protected int MaxHealth { get => maxHealth; set => maxHealth = value; }
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
            //Vector2 newPosition = position + velocity;

            //Rectangle bounds = GameWorld.CurrentRoomBoundary;

            //if (newPosition.X > bounds.Left)
            //    newPosition.X = bounds.Left;
            //if (newPosition.Y < bounds.Top)
            //    newPosition.Y = bounds.Top;
            //if (newPosition.X > bounds.Right /*- sprite.Width*/)
            //    newPosition.X = bounds.Right /*- sprite.Width*/;
            //if (newPosition.Y < bounds.Bottom /*- sprite.Height*/)
            //    newPosition.Y = bounds.Bottom /*- sprite.Height*/;

            //position = newPosition;


            if (battleActive)
            { 

            }
        }



        public abstract void Movement(GameTime gameTime);

        //void Interact(GameObject gameObject);

        public abstract void Animation(GameTime gameTime);

        public virtual void GetHit(int damage)
        {
            health -= damage;

            if (health <= 0) ;
                //isAlive = false;

        }
    }
}
