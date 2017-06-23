/*** ---------------------------------------------------------------------------
/// UnitFactory.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>May 15th, 2017</date>
/// ------------------------------------------------------------------------***/

using core.combat;
using core.data;
using core.data.vo;
using System.Collections.Generic;

namespace core.units
{
    /// <summary>
    /// The factory that creates new units given a UID
    /// </summary>
    public class UnitFactory
    {
        private static UnitFactory instance;

        private MetadataMap metadataMap;

        private UnitFactory()
        {

        }

        public static UnitFactory GetInstance()
        {
            if (instance == null)
            {
                instance = new UnitFactory();
            }

            return instance;
        }

        public void SetMetadata(MetadataMap metadataMap)
        {
            this.metadataMap = metadataMap;
        }

        /// <summary>
        /// Create a new Unit and initialize default equipment and weapons
        /// </summary>
        public Unit CreateNewUnit(string unitUID, string characterUID, CombatantFactionEnum faction)
        {
            UnitVO unitVO = metadataMap.GetVO<UnitVO>(unitUID);
            CharacterVO charVO = metadataMap.GetVO<CharacterVO>(characterUID);

            List<Equipment> defaultEquipment = GetEquipmentListFromUIDs(unitVO.equipmentUIDs);

            List<Equipment> defaultWeapons = GetEquipmentListFromUIDs(unitVO.weaponsUIDs);

            Unit unit = new Unit(unitVO, charVO, faction, defaultEquipment, defaultWeapons);
            return unit;
        }

        /// <summary>
        /// Given a string array of EquipmentVO UIDs return a list of Equipment
        /// </summary>
        private List<Equipment> GetEquipmentListFromUIDs(string[] equipmentUIDs)
        {
            List<Equipment> list = new List<Equipment>();

            if (equipmentUIDs != null && equipmentUIDs.Length > 0)
            {
                for (int i = 0, count = equipmentUIDs.Length; i < count; i++)
                {
                    EquipmentVO eqp = metadataMap.GetVO<EquipmentVO>(equipmentUIDs[i]);
                    list.Add(new Equipment(eqp));
                }
            }

            return list;
        }
    }
}