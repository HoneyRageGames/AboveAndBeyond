/*** ---------------------------------------------------------------------------
/// BaseScreenComponent.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>June 4th, 2017</date>
/// ------------------------------------------------------------------------***/

using core.audio;
using core.constants;
using UnityEngine;
using UnityEngine.UI;

namespace core.ui.screens
{
    public class BaseScreenComponent : MonoBehaviour
    {
        /// <summary>
        /// Is this screeen closable by the ScreenQueueManager? Usually flagged true
        /// and only set to false for special screens such as the TitleScreen
        /// </summary>
        public bool isCloseable = true;

        public virtual void Start()
        {
            Debug.Log("initialize buttons");
            // Hook up buttons
            Button[] buttons = GetComponentsInChildren<Button>();

            for (int i = 0, count = buttons.Length; i < count; i++)
            {
                Button btnComp = buttons[i];
                btnComp.onClick.AddListener(
                    delegate 
                    {
                        OnAnyButtonClicked(btnComp.name);
                    });
            }
        }

        public virtual void OnDestroy()
        {
            ScreenQueueManager.GetInstance().ShowNextScreen();
        }

        private void OnAnyButtonClicked(string name)
        {
            Debug.Log("Button: " + name + " clicked");
            SoundEffectController.GetInstance().PlaySound(GameConstants.SND_BUTTON);
        }
    }
}
