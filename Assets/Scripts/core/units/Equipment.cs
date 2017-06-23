/*** ---------------------------------------------------------------------------
/// Equipment.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>May 15th, 2017</date>
/// ------------------------------------------------------------------------***/

using core.data.vo;

namespace core.units
{
    /// <summary>
    /// Handles the state of a unit's equipment. 
    /// </summary>
    public class Equipment
    {
        public EquipmentVO vo { get; private set; }

        /// <summary>
        /// How much ammunition does this unit have left on this piece of equipment
        /// </summary>
        public int remainingAmmo;

        public Equipment(EquipmentVO vo)
        {
            this.vo = vo;

            remainingAmmo = vo.ammoCapacity;
        }
    }
}