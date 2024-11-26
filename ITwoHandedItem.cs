using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortensKomeback2
{
    internal interface ITwoHandedItem
    {
        int StatBoost(int damagebonus)
        {
            int twoHandedBonusMultiplier = 2;
            return damagebonus * twoHandedBonusMultiplier;
        }
    }
}
