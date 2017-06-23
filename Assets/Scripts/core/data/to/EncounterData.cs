/*** ---------------------------------------------------------------------------
/// EncounterData.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>May 19th, 2017</date>
/// ------------------------------------------------------------------------***/

using core.combat;
using core.units;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace core.data.to
{
    /// <summary>
    /// Contains data for the encounter that can be passed back and forth
    /// </summary>
    public class EncounterData
    {
        /// <summary>
        /// The list of units controlled by the player
        /// </summary>
        public List<Unit> PlayerUnits;

        /// <summary>
        /// The list of units controlled by the enemy
        /// </summary>
        public List<Unit> EnemyUnits;

        /// <summary>
        /// Units which are neutral to faction
        /// </summary>
        public List<Unit> NeutralUnits;

        /// <summary>
        /// The limit of turns before the battle is forced to end.
        /// 
        /// -1 indicates there is no end.
        /// </summary>
        public int MaxTurns = -1;

        /// <summary>
        /// The list of units which have yet to take action;
        /// </summary>
        public List<Unit> UnitsPendingAction;

        public StringBuilder CombatLog;

        public EncounterData ()
        {
            UnitsPendingAction = new List<Unit>();
            CombatLog = new StringBuilder("COMBAT LOG: \n" );
        }

        /// <summary>
        /// Are we done moving things for this faction turn
        /// </summary>
        public bool IsOutofActions()
        {
            return UnitsPendingAction.Count == 0;
        }

        public void BeginFactionTurn(CombatantFactionEnum faction)
        {
            // At the beginning of the turn copy over the remaining units to the pending list

            UnitsPendingAction.Clear();
            
            switch (faction)
            {
                case CombatantFactionEnum.Player:
                    UnitsPendingAction.AddRange(PlayerUnits);
                    break;
                case CombatantFactionEnum.Enemy:
                    UnitsPendingAction.AddRange(EnemyUnits);
                    break;
                case CombatantFactionEnum.Neutral:
                    UnitsPendingAction.AddRange(EnemyUnits);
                    break;
            }

            AddLog(faction.ToString() + " Time to ACT!");
        }

        /// <summary>
        /// Removes the unit from the list of units pending an action
        /// </summary>
        public void SpendAction(Unit unit)
        {
            // This unit is no longer pending an action and thus remove it from the list
            if (UnitsPendingAction.Contains(unit))
            {
                UnitsPendingAction.Remove(unit);
            }
        }

        /// <summary>
        /// Removes the unit from the list of available units and pending
        /// </summary>
        public void DestroyUnit(Unit unit)
        {
            if (UnitsPendingAction.Contains(unit))
            {
                UnitsPendingAction.Remove(unit);
            }

            switch (unit.faction)
            {
                case CombatantFactionEnum.Player:
                    PlayerUnits.Remove(unit);
                    break;
                case CombatantFactionEnum.Enemy:
                    EnemyUnits.Remove(unit);
                    break;
                case CombatantFactionEnum.Neutral:
                    NeutralUnits.Remove(unit);
                    break;
            }
        }

        public void AddLog(string log)
        {
            CombatLog.Append(log);
            CombatLog.Append("\n");
        }

        /// <summary>
        /// Destroy the data and null out everything to be very sure everything gets 
        /// garbage collected
        /// </summary>
        public void Destroy()
        {
            AddLog("END OF ENCOUNTER");

            Debug.Log(CombatLog.ToString());

            UnitsPendingAction.Clear();
            PlayerUnits.Clear();
            EnemyUnits.Clear();
            NeutralUnits.Clear();

            UnitsPendingAction = null;
            PlayerUnits = null;
            EnemyUnits = null;
            NeutralUnits = null;
            CombatLog = null;
        }
    }
}
