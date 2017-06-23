/*** ---------------------------------------------------------------------------
/// CombatEncounterController.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>May 19th, 2017</date>
/// ------------------------------------------------------------------------***/

using core.data.to;
using core.units;
using System.Collections.Generic;

namespace core.combat
{
    /// <summary>
    /// Handles the logic around combat for each combat encounter between ships
    /// </summary>
    public class CombatEncounterController
    {
        private static CombatEncounterController instance;

        /// <summary>
        /// The current turn that we're on
        /// </summary>
        public int CurrentTurn { get; private set; }

        /// <summary>
        /// The current faction that is allowed to move/attack for this turn
        /// </summary>
        public CombatantFactionEnum CurrentFaction = CombatantFactionEnum.Neutral;

        /// <summary>
        /// The data pertaining to the current combat encounter
        /// </summary>
        public EncounterData CurrentEncounter { get; private set; }

        private CombatEncounterController()
        {

        }

        public static CombatEncounterController GetInstance()
        {
            if (instance == null)
            {
                instance = new CombatEncounterController();
            }

            return instance;
        }

        public void StartEncounter(List<Unit> playerUnits, List<Unit> enemyUnits, List<Unit> neutralUnits)
        {
            // Initialize a new encounter
            CurrentEncounter = new EncounterData();
            CurrentEncounter.PlayerUnits = playerUnits;
            CurrentEncounter.EnemyUnits = enemyUnits;
            CurrentEncounter.NeutralUnits = neutralUnits;

            // Set the current turn to -1
            CurrentTurn = -1;

            // Set the player to move first since the player always moves first
            CurrentFaction = CombatantFactionEnum.Neutral;
            GoToNextStep();
        }

        /// <summary>
        /// Goes to the next faction in the sequence. 
        /// 
        /// Player --> Enemy --> Neutral
        /// 
        /// After the neutral faction is done then the turn is done
        /// </summary>
        public void GoToNextStep()
        {
            // TODO: Make an FSM to handle this
            switch (CurrentFaction)
            {
                case CombatantFactionEnum.Player:
                    CurrentFaction = CombatantFactionEnum.Enemy;
                    break;
                case CombatantFactionEnum.Enemy:
                    CurrentFaction = CombatantFactionEnum.Neutral;
                    break;
                case CombatantFactionEnum.Neutral:
                    CurrentFaction = CombatantFactionEnum.Player;
                    CurrentTurn++;

                    CurrentEncounter.AddLog("TURN: " + CurrentTurn);
                    break;
            }

            CurrentEncounter.BeginFactionTurn(CurrentFaction);
        }

        /// <summary>
        /// Ends the encounter;
        /// </summary>
        public void EndEncounter()
        {
            CurrentEncounter.Destroy();
            CurrentEncounter = null;
        }

        public void AttackUnit(Unit attacker, int equipmentSlot, Unit defender)
        {
            CombatUtils.HandleAttack(ref attacker, 0, ref defender, 0);

            if (defender.hp == 0)
            {
                CurrentEncounter.DestroyUnit(defender);
            }

            if (attacker.hp == 0)
            {
                CurrentEncounter.DestroyUnit(attacker);
            }

            // Remove it from the pending list
            CurrentEncounter.SpendAction(attacker);

            if (IsEncounterOver())
            {
                EndEncounter();
                return;
            }

            if (CurrentEncounter.IsOutofActions())
            {
                GoToNextStep();
            }
        }

        public bool IsEncounterOver()
        {
            if (CurrentEncounter == null)
            {
                return true;
            }

            // if all the player units are dead then it's over
            if (CurrentEncounter.PlayerUnits.Count == 0)
            {
                return true;
            }

            // if all the enemy units are dead then it's over
            if (CurrentEncounter.EnemyUnits.Count == 0)
            {
                return true;
            }

            return false;
        }
    }
}