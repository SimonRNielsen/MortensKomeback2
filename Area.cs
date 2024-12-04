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

    
    internal class Area : GameObject
    {

        #region fields
        //public static Dictionary<string, Rectangle> RoomBoundaries = new()
        //{
        //    {"Room1", new Rectangle(50,50,800,600) },
        //    {"Room2", new Rectangle(500,50,700,500) },
        //    {"Room2", new Rectangle(500,50,700,500) },
        //    {"Room2", new Rectangle(500,50,700,500) },
        //    {"Room2", new Rectangle(500,50,700,500) },
        //    {"Room2", new Rectangle(500,50,700,500) },
        //    {"Room2", new Rectangle(500,50,700,500) },
        //    {"Room2", new Rectangle(500,50,700,500) }
        //};

        ////Current room boundary
        //public static Rectangle CurrentRoomBoundary = RoomBoundaries["Room1"];


        #endregion

        #region Properties

        #endregion

        #region Constructor
        public Area(Vector2 placement, int areaArray, string roomName )
        {
            this.position = placement;
            this.layer = 0.0f;
            this.scale = 1f;
            this.health = 1;
            this.sprites = GameWorld.animationSprites["areaStart"];
            this.sprite = sprites[areaArray];
        }

        #endregion

        #region Method
        public override void LoadContent(ContentManager content)
        {
            
        }

        public override void OnCollision(GameObject gameObject)
        {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        //public void ChangeRoom(string newRoom)
        //{
        //    if (RoomBoundaries.ContainsKey(newRoom))
        //    {
        //        CurrentRoomBoundary = RoomBoundaries[newRoom];
        //    }
        //}


        #endregion
    }
}
