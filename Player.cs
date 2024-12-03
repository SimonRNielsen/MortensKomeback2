﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace MortensKomeback2
{
    internal class Player : Character
    {
        #region field
        private PlayerClass playerClass;
        private float timeElapsed;
        private int currentIndex;
        private bool praying;
        private bool interact;
        private bool inventory;
        private byte interactRange = 100;
        private List<NPC> nPCList;

        /// <summary>
        /// Bool to change the spriteEffectIndex so the player face the direction is walking 
        /// </summary>
        private bool direction = true;

        #endregion

        #region properti

        #endregion

        #region constructor
        public Player(PlayerClass playerClass, List<NPC> nPCs)
        {
            this.speed = 600;
            this.health = 100;
            this.fps = 2f;
            this.playerClass = playerClass;
            nPCList = nPCs;
        }

        #endregion

        #region method
        public override void LoadContent(ContentManager content)
        {
            sprites = new Texture2D[4];

            switch (playerClass)
            {
                case PlayerClass.Crusader:
                    break;
                case PlayerClass.Munk:
                    break;
                case PlayerClass.Bishop:
                    sprites = GameWorld.animationSprites["BishopMorten"];
                    break;
            }

            this.Sprite = sprites[0];
        }

        public override void OnCollision(GameObject gameObject)
        {
            //if (gameObject is Door)
            //{
            // Open door to net area
            //}

            if (gameObject is Item)
            {
                //Collect item
            }

            if (gameObject is AvSurface)
            {
                //Take damage
            }
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput();
            Movement(gameTime);
            Animation(gameTime);
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
                this.spriteEffectIndex = 1;
                direction = true;
            }

            //Player moves right when pressed D
            if (keyState.IsKeyDown(Keys.D))
            {
                velocity += new Vector2(1, 0);
                this.spriteEffectIndex = 0;
                direction = false;
            }

            //Normalizing the velocity so if the player press more than one key the velocity will grow greater and greater 
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }

            //The player is Pray
            if (keyState.IsKeyDown(Keys.P) && !praying)
            {
                Pray(interactRange);
                praying = true;
            }

            if (keyState.IsKeyUp(Keys.P))
                praying = false;

            if (keyState.IsKeyDown(Keys.E) && !interact)
            {
                Interact(interactRange);
                interact = true;
            }

            if (keyState.IsKeyUp(Keys.E))
                interact = false;

            if (keyState.IsKeyDown(Keys.I) && !inventory && !GameWorld.DetectInventory())
            {
                GameWorld.newGameObjects.Add(new Menu(GameWorld.Camera.Position, 0));
                inventory = true;
            }
            else if (keyState.IsKeyDown(Keys.I) && !inventory && GameWorld.DetectInventory())
            {
                GameWorld.CloseMenu = true;
                inventory = true;
            }

            if (keyState.IsKeyUp(Keys.I))
                inventory = false;

        }

        /// <summary>
        /// The players movement calculated a the product of velocity, speed and deltaTime
        /// </summary>
        /// <param name="gameTime">A GameTime</param>
        public override void Movement(GameTime gameTime)
        {
            //Calculating the deltatime which is the time that has passed since the last frame
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //The player is moving by the result of HandleInput and deltaTime
            position += (velocity * speed * deltaTime);
        }

        /// <summary>
        /// Making an animation of the sprites
        /// </summary>
        /// <param name="gameTime">A GameTime</param>
        public override void Animation(GameTime gameTime)
        {
            //If the velocity is equal to Vect2.Zero there will not be any animation
            if (velocity == Vector2.Zero)
            {
                return;
            }

            //Adding the time which has passed since the last update
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            currentIndex = (int)(timeElapsed * fps);

            sprite = sprites[currentIndex];

            //Restart the animation
            if (currentIndex >= sprites.Length - 1)
            {
                timeElapsed = 0;
                currentIndex = 0;
            }
        }

        /// <summary>
        /// Flips a bool on all items within a certain distance from the Player (currently set to 300 pixels)
        /// </summary>
        private void Pray(byte range)
        {

            foreach (Item item in GameWorld.hiddenItems)
            {
                float distance = Vector2.Distance(position, item.Position);
                if (distance < range * 3 && distance > -range * 3)
                    item.IsFound = true;
            }

        }

        /// <summary>
        /// Makes the Player do a certain interaction with Item/NPC class depending on what it is
        /// </summary>
        /// <param name="range">Determines the radius for which the Player "interacts with items nearby</param>
        private void Interact(byte range)
        {

            bool nPCNearby = false;
            float distance;

            foreach (NPC nPC in nPCList)
            {
                distance = Vector2.Distance(nPC.Position, position);
                if (distance < range && distance > -range)
                {
                    nPCNearby = true;
                    InitiateDialog(nPC);
                }
                if (nPCNearby)
                    break;
            }

            if (!nPCNearby)
            {
                foreach (Item item in GameWorld.hiddenItems)
                {
                    distance = Vector2.Distance(position, item.Position);
                    if (distance < range && distance > -range)
                    {
                        item.IsPickedUp = true;
                        item.IsFound = false;
                        item.Sprite = item.StandardSprite;
                        GameWorld.playerInventory.Add(item);
                    }
                }
            }

        }

        /// <summary>
        /// Currently empty template to initiate dialog between Player and predetermined NPC
        /// </summary>
        /// <param name="nPC">NPC to initate dialog with</param>
        private void InitiateDialog(NPC nPC)
        {

        }

        #endregion
    }
}
