using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortensKomeback2
{
    internal class BattleField : GameObject
    {
        #region Fields
        private int playerDamage;
        private int enemyDamage;
        private int itemDamage;



        #endregion

        #region Properties

        #endregion

        #region Constructor
        public BattleField(Player player, Enemy enemy)
        {
            playerDamage = 1; //player.Damage;
            enemyDamage = 1; //enemy.Damage;
            itemDamage = 1; //player.ItemDamage;

        }

        #endregion

        #region Methods
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
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        /// <summary>
        /// This method makes the action happen that the player and enemy has decided
        /// </summary>
        private void TakeAction(int chosenAction, int playerClass)
        {
            switch (chosenAction)
            {
                case 1:
                    if(playerClass ==1)
                    {
                        MeleeAttack();
                    }
                    else
                    {
                        RangedAttack();
                    }
                    break;
                case 2:
                    if(playerClass ==1)
                    {
                        Block();
                    }
                    else
                    {
                        Evade();
                    }
                    break;
                case 3:
                    Heal();
                    break;

                default:
                    break;
            }
        }


        private void MeleeAttack()
        {
            int damage = playerDamage + itemDamage;

        }

        private void RangedAttack()
        {

        }

        private void Block()
        {

        }

        private void Evade()
        {

        }

        private void Heal()
        {

        }


        #endregion
    }
}
