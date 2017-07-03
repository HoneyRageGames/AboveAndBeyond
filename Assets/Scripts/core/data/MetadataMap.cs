/*** ---------------------------------------------------------------------------
/// MetadataMap.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>May 14th, 2017</date>
/// ------------------------------------------------------------------------***/

using core.data.vo;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace core.data
{
    /// <summary>
    /// The map containing all of the metadata for the game.
    /// </summary>
    [Serializable]
    public class MetadataMap : ISerializationCallbackReceiver
    {
        /// <summary>
        /// A list of VOs for the unit data.
        /// </summary>
        public List<UnitVO> Units;

        /// <summary>
        /// A list of VOs for equipment data
        /// </summary>
        public List<EquipmentVO> Equipment;

        public List<CharacterVO> Characters;

        public Dictionary<object, Dictionary<string, object>> metadata;

        public MetadataMap()
        {
            metadata = new Dictionary<object, Dictionary<string, object>>();
        }

        public void OnBeforeSerialize()
        {
            // Nothing to do prior to serialization
        }

        /// <summary>
        /// Processes the lists of VOs and adds them to the mapping by class type
        /// and then from uid to VO.
        /// </summary>
        public void OnAfterDeserialize()
        {
            SaveListToMetadataMap<UnitVO>(Units);
            SaveListToMetadataMap<EquipmentVO>(Equipment);
            SaveListToMetadataMap<CharacterVO>(Characters);

            Debug.Log("Done Processing");
        }

        /// <summary>
        /// Given the raw VO list, map them all by UID for quicker lookup later.
        /// </summary>
        private void SaveListToMetadataMap<T>(List<T> voList) where T : BaseVO
        {
            for (int i = 0, count = voList.Count; i < count; i++)
            {
                BaseVO vo = voList[i];
                Type type = typeof(T);

                // See if we have the mapping yet.
                if (!metadata.ContainsKey(type))
                {
                    // nope then let's make the dictionary.
                    metadata[type] = new Dictionary<string, object>();
                }

                Dictionary<string, object> voMap = metadata[type];
                voMap[vo.uid] = vo;
            }
        }

        public T GetVO<T>(string uid) where T:BaseVO
        {
            Type type = typeof(T);

            if (!metadata.ContainsKey(type))
            {
                return null;
            }

            Dictionary<string, object> voMap = metadata[type];
            if (!voMap.ContainsKey(uid))
            {
                return null;
            }

            return (T)voMap[uid];
        }

        /// <summary>
        /// Returns a full breakdown of uids grouped by data type
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<object, Dictionary<string, object>> entry in metadata)
            {
                sb.Append("\n");
                sb.Append("Type: " + entry.Key.ToString());

                Dictionary<string, object> sheet = entry.Value;

                foreach (string uid in sheet.Keys)
                {
                    sb.Append("\n");
                    sb.Append("    ");
                    sb.Append("uid: " + uid);
                }
            }

            return sb.ToString();
        }
    }
}