/*** ---------------------------------------------------------------------------
/// MetaDataManager.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>June 14th, 2017</date>
/// ------------------------------------------------------------------------***/

using core.assets;
using UnityEngine;

namespace core.data
{
    /// <summary>
    /// Contains all of the metadata parsed in from JSON
    /// </summary>
    public class MetaDataManager
    {
        private static MetaDataManager instance;

        private MetadataMap map;

        private MetaDataManager()
        {

        }

        public static MetaDataManager GetInstance()
        {
            if (instance == null)
            {
                instance = new MetaDataManager();
            }

            return instance;
        }

        public void LoadMetadataFromTO(AssetLoadRequestTO to)
        {
            TextAsset textAsset = to.loadedObject as TextAsset;

            map = JsonUtility.FromJson<MetadataMap>(textAsset.text);

            // Debug out all the units parsed into the metadata map
            Debug.Log(map.ToString());
        }
    }
}