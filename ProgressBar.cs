using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortensKomeback2
{
    internal class ProgressBar : GameObject
    {
        protected Texture2D barBackground;
        protected Texture2D bar;
        protected int maxHealth;
        protected int currentHealth;
        protected Rectangle part;

        public ProgressBar(Texture2D bg, Texture2D fg, int max, Vector2 pos )
        {
            barBackground = bg;
            bar = fg;
            maxHealth = max;
            currentHealth = max;
            position = pos;
            part = new(0, 0, bar.Width, bar.Height);
        }

        public override void LoadContent(ContentManager content)
        {
            throw new NotImplementedException();
        }

        public override void OnCollision(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            currentHealth = health;
            part.Width = (int)(currentHealth / maxHealth * bar.Width);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(barBackground, position, Color.White);
            spriteBatch.Draw(bar, position, part, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            base.Draw(spriteBatch);
        }
    }
}
