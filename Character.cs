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
        public abstract void Movement(GameTime gameTime);

        //void Interact(GameObject gameObject);

        public abstract void Animation(GameTime gameTime);
    }
}
