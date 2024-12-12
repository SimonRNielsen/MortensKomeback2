using Microsoft.Xna.Framework;

namespace MortensKomeback2
{
    internal class Boss : Enemy
    {
        #region Fields

        private bool startDialogue = false;
        private bool doOnce = false;

        #endregion

        #region Properties

        public bool StartDialogue { set => startDialogue = value; }

        #endregion

        #region Constructor

        public Boss(Vector2 placement) : base (placement)
        {
            sprites = GameWorld.animationSprites["goosifer"];
            sprite = sprites[0];
            position = placement;
            health = 200;
            maxHealth = health;
            Damage = 20;
        }

        #endregion
        #region Methods

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!doOnce && startDialogue)
            {
                doOnce = true;
                GameWorld.newGameObjects.Add(new Dialogue(new Vector2(GameWorld.Camera.Position.X, GameWorld.Camera.Position.Y + 320), this));
            }
            if (doOnce && startDialogue && !GameWorld.Dialogue && !GameWorld.BattleActive)
            {
                GameWorld.BattleActive = true;
                GameWorld.newGameObjects.Add(new BattleField(this));
                GameWorld.PlayMusic(2); //should play battlemusic
            }
        }

        #endregion
    }
}
