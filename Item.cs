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

        public string ItemName { get => itemName; }
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

        public override void Update(GameTime gameTime)
        {
            collision = false;
            if (GameWorld.hiddenItems.Contains(this))
            {
                if (isFound)
                {
                    timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (timer >= revealTimer)
                        isFound = false;
                    rotation += 0.2f;
                }
                else
                {
                    rotation = 0f;
                    timer = 0f;
                }
            }
        }

        public void OnCollision()
        {
            collision = true;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (collision)
            {
                spriteBatch.Draw(GameWorld.commonSprites["statPanel"], new Vector2(position.X + (sprite.Width / 2) + 25, position.Y - (sprite.Height / 2)), null, drawColor, rotation, Vector2.Zero, scale, objectSpriteEffects[spriteEffectIndex], 0.92f);
                spriteBatch.DrawString(GameWorld.mortensKomebackFont, $"{itemName}\nDmg+:{damageBonus}\nDR+:{damageReductionBonus}\nHP+:{healthBonus}\nSpd+:{speedBonus}", new Vector2(position.X + (sprite.Width / 2) + 30, position.Y - (sprite.Height / 2)), Color.Black, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0.93f);
            }
        }

        #endregion
    }
}
