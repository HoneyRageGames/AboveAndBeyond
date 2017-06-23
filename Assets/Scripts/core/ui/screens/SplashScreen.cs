/*** ---------------------------------------------------------------------------
/// SplashScreen.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>June 4th, 2017</date>
/// ------------------------------------------------------------------------***/

using core.constants;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace core.ui.screens
{
    /// <summary>
    /// The splash screen that appears on initial load of the game. Handles the displaying and
    /// then fading out of the logo. After the fade out we transition to the title screen.
    /// </summary>
    public class SplashScreen : MonoBehaviour
    {
        private const string ANIM_FADEOUT = "logoFadeOutAnim";
        private const string TRG_FADEOUT = "fadeOut";

        private Animator anim;
        private RuntimeAnimatorController fadeOutController;

        public void Start()
        {
            anim = GetComponent<Animator>();
            AnimationClip clip = UIUtils.GetAnimationClipByName(anim, ANIM_FADEOUT);

            // Create the animation event to fire after the fadeout is complete
            AnimationEvent evt = new AnimationEvent();
            evt.time = clip.length;
            evt.functionName = "LoadTitleScene";
            clip.AddEvent(evt);
            
            // Wait N-amount of seconds and then trigger the animation
            Invoke("StartFadeOut", GameConstants.SPLASH_DUR_SEC);
        }

        /// <summary>
        /// Fires the animation trigger to start the fading out of the logo and text
        /// </summary>
        private void StartFadeOut()
        {
            anim.SetTrigger(TRG_FADEOUT);
        }

        /// <summary>
        /// After the fade out is done, it fires 
        /// </summary>
        public void LoadTitleScene()
        {
            Debug.Log("Begin Loading of Title Screen");
            SceneManager.LoadScene("TitleScreen");
        }
    }
}