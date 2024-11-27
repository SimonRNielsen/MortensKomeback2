namespace MortensKomeback2
{
    /// <summary>
    /// Abstract superclass for Item(s)
    /// </summary>
    public abstract class Item : GameObject
    {
        #region Fields

        protected string itemName;
        protected int healthBonus;
        protected int damageBonus;
        protected int damageReductionBonus;
        protected float speedBonus;
        protected bool isEquipped = false;

        #endregion

        #region Properties

        public string ItemName { get => itemName; }
        public int HealthBonus { get => healthBonus; }
        public int DamageBonus { get => damageBonus; }
        public int DamageReductionBonus { get => damageReductionBonus; }
        public float SpeedBonus { get => speedBonus; }
        public bool IsEquipped { get => isEquipped; set => isEquipped = value; }

        #endregion

        #region Constructor
        //Abstract class
        #endregion

        #region Methods



        #endregion
    }
}
