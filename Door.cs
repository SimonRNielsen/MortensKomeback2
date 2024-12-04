using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortensKomeback2
{
    internal class Door : GameObject
    {
        #region field
        private DoorTypes type;
        private int doorNumber;
        protected bool doorOpen;
        private bool locked;
        #endregion

        #region properties
        public bool DoorOpen { get => doorOpen; set => doorOpen = value; }

        #endregion

        #region constructor
        public Door(DoorTypes dt) 
        {
            this.type = dt;
            DoorOpen = false;
            this.layer = 0.2f;
        }


        #endregion

        #region method
        public override void LoadContent(ContentManager content)
        {
            this.sprites = GameWorld.animationSprites["areaStart"];
            switch (type)
            {
                case DoorTypes.Open:
                    this.Sprite = sprites[6];
                    break;
                case DoorTypes.Closed:
                    this.Sprite = sprites[5];
                    break;
                case DoorTypes.Locked:
                    this.Sprite = sprites[5];
                    break;
            }
            
        }


        
        

        public override void OnCollision(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
