/*** ---------------------------------------------------------------------------
/// SoundEffectController.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>May 31st, 2017</date>
/// ------------------------------------------------------------------------***/

using core.assets;
using core.constants;
using core.events;
using System.Collections.Generic;
using UnityEngine;

namespace core.audio
{
    /// <summary>
    /// Handles the preloading and playing of sound effects
    /// </summary>
    public class SoundEffectController
    {
        public const string SND_BUTTON = "sfx_sounds_button6";
        public const string SND_MENU = "sfx_menu_select2";

        private static SoundEffectController instance;

        private Dictionary<string, AudioClip> clipMap;

        private AudioSource source;


        private SoundEffectController()
        {
            clipMap = new Dictionary<string, AudioClip>();
        }

        public static SoundEffectController GetInstance()
        {
            if (instance == null)
            {
                instance = new SoundEffectController();
            }

            return instance;
        }

        public void Initialize()
        {
            GameObject core = GameObject.Find("Core");
            source = core.AddComponent<AudioSource>();

            EventController.GetInstance().RegisterForEvent(EventTypeEnum.ButtonPressed, HandleEvent);
        }
        
        public void PreloadAudio()
        {
            AudioClip[] clipArray = Resources.LoadAll<AudioClip>("Sounds/");

            for (int i = 0, count = clipArray.Length; i < count; i++)
            {
                AudioClip clip = clipArray[i];
                Debug.Log(clip.name);

                clipMap[clip.name] = clip;
            }

            //GameObject core = GameObject.Find("Core");
            //source = core.AddComponent<AudioSource>();
        }

        public void LoadSoundEffectFromTO(AssetLoadRequestTO to)
        {
            AudioClip clip = (AudioClip)to.loadedObject;

            clipMap[clip.name] = clip;
        }

        public void PlaySound(string clipName)
        {
            AudioClip clip = clipMap[clipName];
            source.clip = clip;
            source.Play();
        }

        public void HandleEvent(EventTypeEnum type, object obj)
        {
            switch (type)
            {
                case EventTypeEnum.ButtonPressed:
                    PlaySound(GameConstants.SND_BUTTON);
                    break;
            }
        }
    }
}