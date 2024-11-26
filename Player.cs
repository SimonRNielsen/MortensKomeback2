using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;

namespace MortensKomeback2
{
    internal class Player : GameObject, ICharacter
    {
        #region field
        private int playerClass;

        #endregion

        #region properti
        public int PlayerClass { get => playerClass; set => playerClass = value; }

        #endregion

        #region constructor
        public Player()
        {
            this.speed = 600;
            this.health = 100;
        }

        #endregion

        #region method
        public override void LoadContent(ContentManager content)
        {
            //sprites = new Texture2D[];

            this.Sprite = content.Load<Texture2D>("Sprites\\Charactor\\morten_sprite2"); //Only a test sprite of Morten
        }

        public override void OnCollision(GameObject gameObject)
        {

        }

        public override void Update(GameTime gameTime)
        {
            HandleInput();
            Movement(gameTime);
        }



        /// <summary>
        /// Handle the players keyboard input
        /// </summary>
        public void HandleInput()
        {
            //Reseting the velocity so Morten will stand still when there is no key pressed down
            velocity = Vector2.Zero;

            //Get the keyboard state
            KeyboardState keyState = Keyboard.GetState();

            //Player moves up when pressed W
            if (keyState.IsKeyDown(Keys.W))
            {
                velocity += new Vector2(0, -1);
            }

            //Player moves down when pressed S
            if (keyState.IsKeyDown(Keys.S))
            {
                velocity += new Vector2(0, 1);
            }

            //Player moves left when pressed A
            if (keyState.IsKeyDown(Keys.A))
            {
                velocity += new Vector2(-1, 0);
            }

            //Player moves right when pressed D
            if (keyState.IsKeyDown(Keys.D))
            {
                velocity += new Vector2(1, 0);
            }

            //Normalizing the velocity so if the player press more than one key the velocity will grow greater and greater 
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }

            //The player is Pray
            if (keyState.IsKeyDown(Keys.P))
            {

            }
        }

        /// <summary>
        /// The players movement calculated a the product of velocity, speed and deltaTime
        /// </summary>
        /// <param name="gameTime">A GameTime</param>
        public void Movement(GameTime gameTime)
        {
            //Calculating the deltatime which is the time that has passed since the last frame
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //The player is moving by the result of HandleInput and deltaTime
            position += (velocity * speed * deltaTime);
        }

        public void Interact(GameObject gameObject)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
