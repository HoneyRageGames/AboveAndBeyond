/*** ---------------------------------------------------------------------------
/// SaveFileButtonModule.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>June 11th, 2017</date>
/// ------------------------------------------------------------------------***/

using core.audio;
using core.constants;
using core.player;
using core.ui.screens;
using UnityEngine;

namespace core.ui.modules
{
    public class SaveFileButtonModule : LoadFileButtonModule
    {
        // Use this for initialization
        public override void Start()
        {
            base.Start();
            button.interactable = true;
        }

        protected override void OnClicked()
        {
            Debug.Log("Save File Button Clicked");

            GameObject MainMenu = GameObject.Find(GameConstants.UI_MAIN_MENU);
            ConfirmScreen screenComp =
                UIFactory.CreateScreenAndAddComponent<ConfirmScreen>(UIFactory.SCR_CONFIRM, MainMenu);

            ScreenQueueManager sqm = ScreenQueueManager.GetInstance();
            sqm.ShowScreenNow(screenComp.gameObject);
            sqm.QueueScreenAsNext(parentScreen);

            screenComp.SetData(UIConstants.SAVE_FILE_TITLE, UIConstants.SAVE_FILE_DESC, OnYes, OnNo);

            SoundEffectController.GetInstance().PlaySound(GameConstants.SND_BUTTON);
        }

        protected override void OnYes()
        {
            // Set the current save slot
            PlayerAccountManager.GetInstance().CurrSaveSlot = SlotNum;

            Debug.Log("SaveFile Yes");

            // Pop the name input screen
            GameObject MainMenu = GameObject.Find(GameConstants.UI_MAIN_MENU);
            GameObject nameInputScreen = UIFactory.CreateScreen(UIFactory.SCR_NAME_INPUT, MainMenu);

            ScreenQueueManager sqm = ScreenQueueManager.GetInstance();
            sqm.QueueScreenAsNext(nameInputScreen);
            sqm.QueueScreen(parentScreen);
        }

        protected override void OnNo()
        {
        }

        protected override GameObject GetParent()
        {
            return GameObject.Find(UIFactory.SCR_SAVE_FILE);
        }
    }
}
