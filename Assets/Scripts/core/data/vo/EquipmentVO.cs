/*** ---------------------------------------------------------------------------
/// EquipmentVO.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>May 15th, 2017</date>
/// ------------------------------------------------------------------------***/

using System;

namespace core.data.vo
{
    /// <summary>
    /// Items that can be equiped onto Units to modify attributes
    /// </summary>
    [Serializable]
    public class EquipmentVO : BaseVO
    {
        /// <summary>
        /// The amount of base damage this unit can do
        /// </summary>
        public int baseDamage;

        /// <summary>
        /// The type of equipment this is for example, weapon, armor, etc
        /// </summary>
        public string type;

        /// <summary>
        /// The subtype of this equipment such as cannon, missle, armor
        /// </summary>
        public string subtype;

        /// <summary>
        /// The probability of each shot to hit
        /// </summary>
        public double hitChance;

        /// <summary>
        /// The probability of dodging a shot
        /// </summary>
        public double dodgeChance;

        /// <summary>
        /// The number of shots per attack for this weapon
        /// </summary>
        public int numShotsPerAttack;

        /// <summary>
        /// The amount of ammo capacity for this type of equipment.
        /// </summary>
        public int ammoCapacity;

        /// <summary>
        /// The amount of additional damage this equipment adds
        /// </summary>
        public int damageBonus;

        /// <summary>
        /// How much additional health this unit can receive
        /// </summary>
        public int healthBonus;

        /// <summary>
        /// How much damage this piece of equipment helps with
        /// </summary>
        public int damageReduction;

        /// <summary>
        /// The range of the attack
        /// </summary>
        public int range;
    }
}