namespace MortensKomeback2
{
    /// <summary>
    /// Abstract superclass for Item(s)
    /// </summary>
    public abstract class Item : GameObject
    {
        #region Fields

        protected int healthBonus;
        protected int damageBonus;
        protected int damageReductionBonus;
        protected float speedBonus;

        #endregion

        #region Properties

        public int HealthBonus { get => healthBonus; }
        public int DamageBonus { get => damageBonus; }
        public int DamageReductionBonus { get => damageReductionBonus; }
        public float SpeedBonus { get => speedBonus; }

        #endregion

        #region Constructor
        //Abstract class
        #endregion

        #region Methods



        #endregion
    }
}
