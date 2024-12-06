using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MortensKomeback2
{
    /// <summary>
    /// Abstract superclass for Item(s)
    /// </summary>
    public abstract class Item : GameObject
    {
        #region Fields

        protected string itemName;
        protected int healthBonus;
        protected int damageBonus;
        protected int damageReductionBonus;
        protected float speedBonus;
        protected bool isEquipped = false;
        protected bool isUsed = false;
        protected bool isUseable = false;
        protected bool isFound = false;
        protected bool isPickedUp = false;
        protected bool collision;
        protected Texture2D standardSprite;
        private float timer;
        private float revealTimer = 1.5f;

        #endregion

        #region Properties

        public int HealthBonus { get => healthBonus; }
        public int DamageBonus { get => damageBonus; }
        public int DamageReductionBonus { get => damageReductionBonus; }
        public float SpeedBonus { get => speedBonus; }
        public bool IsEquipped { get => isEquipped; set => isEquipped = value; }
        public bool IsUsed { get => isUsed; set => isUsed = value; }
        public bool IsUseable { get => isUseable; }
        public bool IsFound { get => isFound; set => isFound = value; }
        public Texture2D StandardSprite { get => standardSprite; }
        public bool IsPickedUp { get => isPickedUp; set => isPickedUp = value; }

        #endregion

        #region Constructor
        //Abstract class
        #endregion

        #region Methods

        /// <summary>
        /// Handles reset of collision and rotation of sprite + reset of timer that "reveals" the item
        /// </summary>
        /// <param name="gameTime">Synchronizes timer</param>
        public override void Update(GameTime gameTime)
        {

            collision = false;
            if (GameWorld.hiddenItems.Contains(this))
            {
                if (isFound)
                {
                    rotation += 0.2f;
                    timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (timer >= revealTimer)
                    {
                        rotation = 0f;
                        isFound = false;
                    }
                }

            }
        }

        /// <summary>
        /// Sets collision bool that's triggered remotely
        /// </summary>
        public void OnCollision()
        {

            collision = true;

        }

        /// <summary>
        /// Overriden from base to add a graphic mouse-over effect that shows the items "stat"-bonus'
        /// </summary>
        /// <param name="spriteBatch">Draw logic</param>
        public override void Draw(SpriteBatch spriteBatch)
        {

            base.Draw(spriteBatch);
            if (collision)
            {
                if (!(this is OffHandItem))
                {
                    spriteBatch.Draw(GameWorld.commonSprites["statPanel"], new Vector2(position.X + (sprite.Width / 2) + 25, position.Y - (sprite.Height / 2)), null, drawColor, rotation, Vector2.Zero, scale, objectSpriteEffects[spriteEffectIndex], 0.99999f);
                    spriteBatch.DrawString(GameWorld.mortensKomebackFont, $"{itemName}\nDmg+:{damageBonus}\nDR+:{damageReductionBonus}\nHP+:{healthBonus}\nSpd+:{speedBonus}", new Vector2(position.X + (sprite.Width / 2) + 30, position.Y - (sprite.Height / 2)), Color.Black, 0f, Vector2.Zero, 2f, SpriteEffects.None, 1f);
                }
                else if ((this is OffHandItem) && !isEquipped)
                {
                    spriteBatch.Draw(GameWorld.commonSprites["statPanel"], new Vector2(position.X + (sprite.Width / 2) + 25, position.Y - (sprite.Height / 2)), null, drawColor, rotation, Vector2.Zero, scale, objectSpriteEffects[spriteEffectIndex], 0.99999f);
                    spriteBatch.DrawString(GameWorld.mortensKomebackFont, $"{itemName}\nDmg+:{damageBonus}\nDR+:{damageReductionBonus}\nHP+:{healthBonus}\nSpd+:{speedBonus}", new Vector2(position.X + (sprite.Width / 2) + 30, position.Y - (sprite.Height / 2)), Color.Black, 0f, Vector2.Zero, 2f, SpriteEffects.None, 1f);
                }
                else
                {
                    spriteBatch.Draw(GameWorld.commonSprites["statPanel"], new Vector2(position.X, position.Y - (sprite.Height * 2.5f)), null, drawColor, rotation, Vector2.Zero, scale, objectSpriteEffects[spriteEffectIndex], 0.99999f);
                    spriteBatch.DrawString(GameWorld.mortensKomebackFont, $"{itemName}\nDmg+:{damageBonus}\nDR+:{damageReductionBonus}\nHP+:{healthBonus}\nSpd+:{speedBonus}", new Vector2(position.X + 5, position.Y - (sprite.Height * 2.5f)), Color.Black, 0f, Vector2.Zero, 2f, SpriteEffects.None, 1f);
                }
            }

        }

        #endregion
    }
}
