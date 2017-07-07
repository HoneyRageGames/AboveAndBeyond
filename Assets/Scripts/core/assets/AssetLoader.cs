/*** ---------------------------------------------------------------------------
/// AssetLoader.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>June 18th, 2017</date>
/// ------------------------------------------------------------------------***/

using core.audio;
using core.data;
using core.dialog;
using core.events;
using core.tilesys;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace core.assets
{
    public class AssetLoader
    {
        private static AssetLoader instance;

        public int expected = 0;
        public int loaded = 0;

        private DateTime startDate;

        private AssetLoader()
        {
        }

        public static AssetLoader GetInstance()
        {
            if (instance == null)
            {
                instance = new AssetLoader();
            }

            return instance;
        }

        public void LoadAsset(AssetLoadRequestTO to)
        {
            expected += 1;
            loaded += 0;

            startDate = DateTime.Now;

            MainLoop main = MainLoop.GetInstance();
            main.StartCoroutine(LoadAsync(to));
        }

        /// <summary>
        /// Loads up a list of assets
        /// </summary>
        public void LoadAssets(List<AssetLoadRequestTO> assetLoadRequestList)
        {
            Debug.Log("Preparing to Load " + assetLoadRequestList.Count + " assets.");

            startDate = DateTime.Now;

            expected += assetLoadRequestList.Count;

            MainLoop main = MainLoop.GetInstance();

            for (int i = 0, count = assetLoadRequestList.Count; i < count; i++)
            {
                AssetLoadRequestTO to = assetLoadRequestList[i];

                if (to.assetLoadType == AssetLoadType.SpriteSheet)
                {
                    LoadAllObjectsInPath(to);
                }
                else
                {
                    main.StartCoroutine(LoadAsync(to));
                }
                
            }
        }

        private void LoadAllObjectsInPath(AssetLoadRequestTO to)
        {
            to.loadObjectList = Resources.LoadAll(to.path);
            HandleAssetLoaded(to);

            if (to.callback != null)
            {
                to.callback(to);
            }

            EventController.GetInstance().FireEvent(EventTypeEnum.AssetLoadComplete, null);
            LoadFinished();
        }

        private IEnumerator LoadAsync(AssetLoadRequestTO to)
        {
            ResourceRequest req = Resources.LoadAsync(to.path);

            while (!req.isDone)
            {
                Debug.Log("Load Progress: " + req.progress);
                yield return null;
            }
            to.loadedObject = req.asset;
            HandleAssetLoaded(to);

            if (to.callback != null)
            {
                to.callback(to);
            }

            EventController.GetInstance().FireEvent(EventTypeEnum.AssetLoadComplete, null);
            LoadFinished();
        }

        private void LoadFinished()
        {
            loaded++;
            Debug.Log("Loaded " + (loaded) + "/" + expected);

            if (loaded >= expected)
            {
                // FIre off the event to notify others that we've completed our multi-load
                EventController.GetInstance().FireEvent(EventTypeEnum.AssetsLoadMultipleComplete, loaded);
                loaded = expected = 0;

                DateTime endDate = DateTime.Now;

                Debug.Log("Total Loading Time: " +
                    endDate.Subtract(startDate).TotalMilliseconds + " MS");
            }
        }

        /// <summary>
        /// Upon the asset being done loading from resources send it to the correct place to store
        /// </summary>
        /// <param name="to"></param>
        public void HandleAssetLoaded(AssetLoadRequestTO to)
        {
            switch (to.assetLoadType)
            {
                case AssetLoadType.Conversation:
                    DialogController.GetInstance().LoadDialogFromTO(to);
                    DialogController.GetInstance().PreloadPortraits();
                    DialogController.GetInstance().PreloadMusic();
                    break;
                case AssetLoadType.MapData:
                    MapData md = MapData.FromTO(to);
                    MapController.GetInstance().LoadMapData(md);
                    break;
                case AssetLoadType.Metadata:
                    MetaDataManager.GetInstance().LoadMetadataFromTO(to);
                    break;
                case AssetLoadType.Music:
                    MusicController.GetInstance().LoadMusicFromTO(to);
                    break;
                case AssetLoadType.SoundEfect:
                    SoundEffectController.GetInstance().LoadSoundEffectFromTO(to);
                    break;
            }
        }
    }

}