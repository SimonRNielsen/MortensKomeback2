using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MortensKomeback2
{
    internal class Environment : GameObject
    {

        //constructor
        public Environment(Vector2 placement, int environment)
        {
            position = placement;
            sprite = GameWorld.animationSprites["environment"][environment];
            scale = 1.4f;
        }

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
    }
}
