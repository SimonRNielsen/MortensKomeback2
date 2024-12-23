﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MortensKomeback2
{
    public abstract class GameObject
    {
        protected Texture2D sprite;
        protected Texture2D[] sprites;
        protected Vector2 position;
        protected Vector2 origin;
        protected Vector2 velocity;
        private float fps;
        protected float scale = 1f;
        protected float layer = 0.000001f;
        protected float speed;
        protected float rotation;
        protected int health = 1;
        protected int spriteEffectIndex;
        protected bool isAlive = true;
        protected SpriteEffects[] objectSpriteEffects = new SpriteEffects[3] { SpriteEffects.None, SpriteEffects.FlipHorizontally, SpriteEffects.FlipVertically };
        protected Color drawColor = Color.White;

        public int Health
        {
            get => health;
            set
            {
                if (this is Player)
                {
                    if (health > (this as Player).MaxHealth + (this as Player).HealthBonus)
                        health = (this as Player).MaxHealth + (this as Player).HealthBonus;
                    if (value >= (this as Player).MaxHealth + (this as Player).HealthBonus)
                        health = (this as Player).MaxHealth + (this as Player).HealthBonus;
                    else
                        health = value;
                }
                else
                    health = value;
                if (health <= 0)
                    isAlive = false;
            }
        }
        public Texture2D Sprite { get => sprite; set => sprite = value; }
        public Vector2 Position { get => position; set => position = value; }
        public virtual Rectangle CollisionBox
        {
            get { return new Rectangle((int)(Position.X - (Sprite.Width / 2) * scale), (int)(Position.Y - (Sprite.Height / 2) * scale), (int)(Sprite.Width * scale), (int)(Sprite.Height * scale)); }
        }
        public bool IsAlive { get => isAlive; set => isAlive = value; }

        public int SpriteEffectIndex { get => spriteEffectIndex; set => spriteEffectIndex = value; }
        public Texture2D[] Sprites { get => sprites; set => sprites = value; }
        public float Rotation { get => rotation; set => rotation = value; }
        public float Fps { get => fps; set => fps = value; }

        public abstract void OnCollision(GameObject gameObject);

        public abstract void LoadContent(ContentManager content);

        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Modified to recieve individual sprite, position, rotation, scale, spriteeffects and layerdepth from each individual gameobject
        /// </summary>
        /// <param name="spriteBatch">Drawing tool</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position, null, drawColor, Rotation, new Vector2(Sprite.Width / 2, Sprite.Height / 2), scale, objectSpriteEffects[SpriteEffectIndex], layer);
        }

        /// <summary>
        /// Checking if two objects is colliding 
        /// </summary>
        /// <param name="gameObject">A GameObject</param>
        public virtual void CheckCollision(GameObject gameObject)
        {
            if (CollisionBox.Intersects(gameObject.CollisionBox))
            {
                OnCollision(gameObject);
            }
        }
    }
}
