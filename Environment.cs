using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MortensKomeback2
{
    internal class Environment : GameObject
    {

        //constructor
        public Environment(Vector2 placement, int environment, float scaling)
        {
            position = placement;
            sprite = GameWorld.animationSprites["environment"][environment];
            scale = 1.4f; //standard size
            scale = scaling; //able to set in the constructor
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
