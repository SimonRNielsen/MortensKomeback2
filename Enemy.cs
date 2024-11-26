using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortensKomeback2
{
    internal class Enemy : GameObject, ICharacter
    {
        #region field
        private GraphicsDeviceManager _graphics;
        private Vector2 enemyDirection;

        #endregion

        #region properti

        #endregion

        #region constructor
        public Enemy()
        {
            this.speed = 300;
        }

        #endregion

        #region method

        public override void LoadContent(ContentManager content)
        {
            this.Sprite = content.Load<Texture2D>("Sprites\\Charactor\\goose0"); //Only a test sprite of Morten
        }

        public override void OnCollision(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            Movement(gameTime);
        }

        public void Movement(GameTime gameTime)
        {
            //Calculating the deltatime which is the time that has passed since the last frame
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            velocity = new Vector2(0, 1);
            enemyDirection = new Vector2(0, 1);

            //The player is moving by the result of HandleInput and deltaTime
            position += (velocity * speed * deltaTime);

            if (position.Y >= 1080/2 + this.sprite.Height *2)
            {
                
            }
        }

        public void Interact(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
