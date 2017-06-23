/*** ---------------------------------------------------------------------------
/// UnitVO.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>May 14th, 2017</date>
/// ------------------------------------------------------------------------***/

using System;
using System.Collections.Generic;

namespace core.data.vo
{
    /// <summary>
    /// The value object data for a unit.
    /// </summary>
    [Serializable]
    public class UnitVO : BaseVO
    {
        /// <summary>
        /// The raw type string. Should enum this later.
        /// </summary>
        public string type;

        /// <summary>
        /// How much base health this unit starts with
        /// </summary>
        public int hp;

        /// <summary>
        /// The amount of base damage this unit can do
        /// </summary>
        public int baseDamage;

        /// <summary>
        /// How much ammunition this unit can carry
        /// </summary>
        public int ammo;

        /// <summary>
        /// The movement speed is how many tiles this unit can move in one turn
        /// </summary>
        public int speed;

        /// <summary>
        /// How many tiles a unit can move before running out of fuel
        /// </summary>
        public int fuel;

        /// <summary>
        /// The number of equipment pieces that the unit can equip
        /// </summary>
        public int equipmentSlots;

        /// <summary>
        /// Space deliminated list of default equipment for this unit
        /// </summary>
        public string equipmentList;

        /// <summary>
        /// Post process set of equipmentUIDs in convenient array form
        /// </summary>
        public string[] equipmentUIDs;

        /// <summary>
        /// The number of weapon pieces this unit is able to carry
        /// </summary>
        public int weaponSlots;

        /// <summary>
        /// Splace deliminated list of default weapons for this unit
        /// </summary>
        public string weaponsList;

        /// <summary>
        /// Post process set of EquipmentVO uids in convenient array form
        /// </summary>
        public string[] weaponsUIDs;


        public override void Process()
        {
            // Only if we have something for the equipmentList do we need to add anything;
            if (!string.IsNullOrEmpty(equipmentList))
            {
                // Split it on the space
                equipmentUIDs = equipmentList.Split(' ');
            }

            if (!string.IsNullOrEmpty(weaponsList))
            {
                weaponsUIDs = weaponsList.Split(' ');
            }

            base.Process();
        }
    }
}