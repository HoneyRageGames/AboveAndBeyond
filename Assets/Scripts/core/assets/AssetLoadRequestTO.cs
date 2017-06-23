/*** ---------------------------------------------------------------------------
/// AssetLoadType.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>June 18th, 2017</date>
/// ------------------------------------------------------------------------***/

using core.constants;

namespace core.assets
{
    public enum AssetLoadType
    {
        Metadata,
        SoundEfect,
        Music,
        Conversation,
        SaveData,
    }

    public class AssetLoadRequestTO
    {
        public delegate void OnLoadedCallback(AssetLoadRequestTO to);

        public string path;
        public AssetLoadType assetLoadType;

        public UnityEngine.Object loadedObject;

        public OnLoadedCallback callback;

        /// <summary>
        /// Creates an asset request for a sound effect
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static AssetLoadRequestTO CreateSoundEffectAssetRequest(string fileName)
        {
            AssetLoadRequestTO to = new AssetLoadRequestTO();
            to.assetLoadType = AssetLoadType.SoundEfect;
            to.path = GameConstants.SOUND_LOC + fileName;

            return to;
        }
    }
}
