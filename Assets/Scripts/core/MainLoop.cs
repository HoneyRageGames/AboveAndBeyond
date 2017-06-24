/*** ---------------------------------------------------------------------------
/// MainLoop.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>June 4th, 2017</date>
/// ------------------------------------------------------------------------***/

using core.assets;
using core.audio;
using core.constants;
using core.input;
using core.ui;
using System.Collections.Generic;
using UnityEngine;

namespace core
{
    public class MainLoop : MonoBehaviour
    {
        public InputController inputController;
        public MusicController musicController;

        private static MainLoop instance;

        private MainLoop()
        {
            inputController = InputController.GetInstance();
            musicController = MusicController.GetInstance();
            instance = this;
        }

        public static MainLoop GetInstance()
        {
            if (instance == null)
            {
                instance = new MainLoop();
            }

            return instance;
        }

        // Use this for initialization
        public void Start()
        {
            MusicController.GetInstance().Initialize();
            SoundEffectController.GetInstance().Initialize();

            List<AssetLoadRequestTO> preloadAssets = new List<AssetLoadRequestTO>();

            // Preload some button sound effects
            AssetLoadRequestTO btnSFX = 
                AssetLoadRequestTO.CreateSoundEffectAssetRequest(GameConstants.SND_BUTTON);

            preloadAssets.Add(btnSFX);

            AssetLoadRequestTO escSFX =
                AssetLoadRequestTO.CreateSoundEffectAssetRequest(GameConstants.SND_MENU);
            preloadAssets.Add(escSFX);

            AssetLoader.GetInstance().LoadAssets(preloadAssets);
        }

        public void OnApplicationQuit()
        {
            ScreenQueueManager.GetInstance().ClearQueueAndDestroyAllScreens();
        }

        // Update is called once per frame
        public void Update()
        {
            if (inputController != null)
            {
                inputController.Update();
            }

            if (musicController != null)
            {
                musicController.Update();
            }
        }
    }
}