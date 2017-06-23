/*** ---------------------------------------------------------------------------
/// CombatController.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>May 15th, 2017</date>
/// ------------------------------------------------------------------------***/

using core.data.to;
using core.data.vo;
using core.units;
using UnityEngine;

namespace core.combat
{
    /// <summary>
    /// Static class that handles the combat calculations
    /// </summary>
    public class CombatUtils
    {
        private const string INDENT = "    ";

        /// <summary>
        /// Handle the attack between 2 combatants
        /// </summary>
        public static void HandleAttack(ref Unit attacker, int atkEqpSlot, ref Unit defender, int defEqpSlot)
        {
            EncounterData eData = CombatEncounterController.GetInstance().CurrentEncounter;

            eData.AddLog("ATTACK");

            ApplyDamage(ref attacker, atkEqpSlot, ref defender, eData);


            // Unit is dead so no counter attack.
            if (defender.hp <= 0)
            {
                return;
            }

            eData.AddLog("COUNTER-ATTACK");
            ApplyDamage(ref defender, defEqpSlot, ref attacker, eData);
        }

        /// <summary>
        /// Applies damage to the defender
        /// </summary>
        private static void ApplyDamage(ref Unit attacker, int atkEqpSlot, ref Unit defender, EncounterData eData)
        {
            Equipment attackerEquipment = attacker.weapons[atkEqpSlot];
            EquipmentVO equipmentVO = attackerEquipment.vo;

            eData.AddLog(INDENT + attacker.vo.uid + "(" + attacker.hp + "/" + 
                attacker.vo.hp + ")" + " uses " + equipmentVO.uid + " on " + 
                defender.vo.uid + " (" + defender.hp + "/" + defender.vo.hp + ")");

            // calculate the damage
            int damage = attacker.GetDamage(atkEqpSlot) - defender.GetDamageReduction();

            // Prevent damage from rolling negative and healing the enemy
            if (damage < 0)
            {
                damage = 0;
            }

            int numShots = equipmentVO.numShotsPerAttack;
            double hitChance = attacker.GetHitChance(atkEqpSlot);

            // do the num shots
            for (int i = 0; i < numShots; i++)
            {
                // expend some ammo
                int ammoUsed = 1;
                attackerEquipment.remainingAmmo -= ammoUsed;
                if (attackerEquipment.remainingAmmo <= 0)
                {
                    eData.AddLog(INDENT + equipmentVO.uid + " OUT OF AMMO!!");
                    break;
                }

                if (!DoesShotHit(hitChance, 0))
                {
                    eData.AddLog(INDENT + "MISS: AMMO REMAINING: " + 
                        attacker.vo.uid + " " + attackerEquipment.remainingAmmo);
                    continue;
                }

                // apply the damage
                defender.hp -= damage;

                eData.AddLog(INDENT + "DAMAGE: " + defender.vo.uid + 
                    " -" + damage + " (" + defender.hp + "/" + defender.vo.hp + " HP)" + 
                    " AMMO REMAINING: " + attacker.vo.uid + "(" + attackerEquipment.remainingAmmo +
                    "/" + attackerEquipment.vo.ammoCapacity + ")");

                // prevent it from going negative.
                if (defender.hp <= 0)
                {
                    defender.hp = 0;

                    eData.AddLog(INDENT + defender.vo.uid + " DESTROYED!!");
                    break;
                }
            }
        }

        /// <summary>
        /// Does the shot hit given the attacker's hit chance and the defender's dodge chance
        /// </summary>
        private static bool DoesShotHit(double hitChance, double dodgeChange)
        {
            // Roll for chance to hit
            float roll = Random.Range(0, 1.0f);

            // If we roll under the threshold then we're good
            return roll <= hitChance - dodgeChange;
        }

    }
}