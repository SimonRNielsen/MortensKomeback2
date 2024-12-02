using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortensKomeback2
{
    internal class Door : Obstacle
    {
        #region field
        private int doorNumber;
        protected bool doorOpen;
        private bool locked;
        #endregion

        #region properties
        public bool DoorOpen { get => doorOpen; set => doorOpen = value; }

        #endregion

        #region constructor
        public Door(int xPosition, int yPosition, int doorNumber) : base(xPosition, yPosition)
        {
            this.doorNumber = doorNumber;
            DoorOpen = false;
        }


        #endregion

        #region method
        public override void LoadContent(ContentManager content)
        {
            if(locked)
            {

            }
            if (DoorOpen) //Open
            {

            }
            else //Closed
            {

            }
        }


        public void loadDoor(int doorNumber)
        {
            switch (doorNumber)
            {
                case 1: GameWorld.PlayerInstance.Position = new Vector2(200, 200);
                    break;
                case 2: locked = true;
                    break;
            }
        }


        #endregion
    }
}
