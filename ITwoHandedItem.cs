using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortensKomeback2
{
    /// <summary>
    /// Interface for "Two-Handed" Items that currently only adds a "Statboost" function that doubles a specified int, designed for the "damageBonus" stat
    /// </summary>
    internal interface ITwoHandedItem
    {
        /// <summary>
        /// Function to multiply a stat
        /// </summary>
        /// <param name="damageBonus"></param>
        /// <returns>Supplied int multiplied by 2</returns>
        int StatBoost(int damageBonus)
        {
            int twoHandedBonusMultiplier = 2;
            return damageBonus * twoHandedBonusMultiplier;
        }
    }
}
