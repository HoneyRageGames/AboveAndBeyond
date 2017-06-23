/*** ---------------------------------------------------------------------------
/// Unit.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>May 15th, 2017</date>
/// ------------------------------------------------------------------------***/

using core.combat;
using core.data.vo;
using System.Collections.Generic;
using System.Text;

namespace core.units
{
    /// <summary>
    /// Represents the unit's state and has reference to the original metadata
    /// </summary>
    public class Unit
    {
        public UnitVO vo;

        public CharacterVO pilot;

        /// <summary>
        /// Indicates which side this unit belongs to.
        /// </summary>
        public CombatantFactionEnum faction;

        /// <summary>
        /// The unit's current remaining HP
        /// </summary>
        public int hp;

        /// <summary>
        /// The unit's remaining ammo
        /// </summary>
        public int ammo;

        /// <summary>
        /// The unit's remaining fuel
        /// </summary>
        public int fuel;

        public List<Equipment> weapons;
        public List<Equipment> equipment;

        public Unit(
            UnitVO vo, 
            CharacterVO pilot, 
            CombatantFactionEnum faction, 
            List<Equipment> equipment, 
            List<Equipment> weapons)
        {
            this.vo = vo;

            this.pilot = pilot;
            this.faction = faction;

            // Initialize the unit with max supplies
            hp = vo.hp;
            ammo = vo.ammo;
            fuel = vo.fuel;

            this.equipment = equipment;
            this.weapons = weapons;
        }

        public int GetDamage(int weaponSlot)
        {
            int damage = weapons[weaponSlot].vo.baseDamage;

            // Add up all the equipment damage bonuses
            for (int i = 0, count = equipment.Count; i < count; i++)
            {
                EquipmentVO eqpVO = equipment[i].vo;
                damage += eqpVO.damageBonus;
            }

            return damage;
        }

        public double GetHitChance(int weaponSlot)
        {
            double chanceToHit = weapons[weaponSlot].vo.hitChance;

            // Add up all the equipment hit chance bonuses
            for (int i = 0, count = equipment.Count; i < count; i++)
            {
                EquipmentVO eqpVO = equipment[i].vo;
                chanceToHit += eqpVO.hitChance;
            }

            // Apply character bonuses
            chanceToHit += pilot.hitChance;

            return chanceToHit;
        }

        public double GetDodgeChance()
        {
            double chanceToDodge = 0;

            // Add up all the dodge bonuses for all the pieces of equipment
            for (int i = 0, count = equipment.Count; i < count; i++)
            {
                EquipmentVO eqpVO = equipment[i].vo;
                chanceToDodge += eqpVO.dodgeChance;
            }

            // Apply character bonuses
            chanceToDodge += pilot.dodgeChance;

            return chanceToDodge;
        }

        public int GetDamageReduction()
        {
            int reduction = 0;

            // Add up all the dodge bonuses for all the pieces of equipment
            for (int i = 0, count = equipment.Count; i < count; i++)
            {
                EquipmentVO eqpVO = equipment[i].vo;
                reduction += eqpVO.damageReduction;
            }

            reduction += pilot.damageReduction;

            return reduction;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(vo.uid);
            sb.Append(": \n");
            sb.Append("HP: " + hp + "\n");
            sb.Append("Fuel: " + fuel + "\n");

            if (weapons != null)
            {
                for (int i = 0, count = weapons.Count; i < count; i++)
                {
                    Equipment eqp = weapons[i];

                    sb.Append(eqp.vo.uid + ":" + eqp.remainingAmmo + " \n");
                }
            }

            return sb.ToString();
        }
    }
}