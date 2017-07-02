/*** ---------------------------------------------------------------------------
/// SaveData.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>April 24th, 2017</date>
/// ------------------------------------------------------------------------***/

using core.util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace core.player
{
    [Serializable]
    public class SaveData : ISerializationCallbackReceiver
    {
        public string PlayerName;
        public List<ScalarSaveIntEntry> int_scalars;
        public List<ScalarSaveStrEntry> str_scalars;
        public long LastSaveDate;
        public long CreationDate;

        public DateTime LastSaveDateTime;
        public DateTime CreationDateTime;

        public SaveData(Player player)
        {
            if (player == null)
            {
                return;
            }
            PlayerName = player.PlayerName;

            LastSaveDateTime = player.LastSaveDate;
            CreationDateTime = player.CreationDate;

            LastSaveDate = DateUtils.GetEpochFromDateTime(LastSaveDateTime);
            CreationDate = DateUtils.GetEpochFromDateTime(CreationDateTime);

            Dictionary<string, object> scalarMap = player.GetScalarMap();

            foreach (KeyValuePair<string, object> entry in scalarMap)
            {
                if (entry.Value is int)
                {
                    if (int_scalars == null)
                    {
                        int_scalars = new List<ScalarSaveIntEntry>();
                    }

                    int_scalars.Add(new ScalarSaveIntEntry(entry.Key, entry.Value));

                    continue;
                }

                if (entry.Value is string)
                {
                    if (str_scalars == null)
                    {
                        str_scalars = new List<ScalarSaveStrEntry>();
                    }

                    str_scalars.Add(new ScalarSaveStrEntry(entry.Key, entry.Value));
                }
            }
        }

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {
            Init();
        }

        private void Init()
        {
            LastSaveDateTime = DateUtils.GetDateTimeFromEpoch(LastSaveDate);
            CreationDateTime = DateUtils.GetDateTimeFromEpoch(CreationDate);
        }

        
    }
}