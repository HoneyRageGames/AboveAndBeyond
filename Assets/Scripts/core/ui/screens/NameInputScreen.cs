/*** ---------------------------------------------------------------------------
/// NameInputScreen.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>June 11th, 2017</date>
/// ------------------------------------------------------------------------***/

using core.assets;
using core.constants;
using core.dialog;
using core.events;
using core.player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace core.ui.screens
{
    public class NameInputScreen : BaseScreenComponent
    {
        private const string BTN_SUBMIT = "BtnSubmit";
        private const string INPUT_NAME = "NameInputText";

        private Button btnSubmit;
        private InputField nameField;

        public override void Start()
        {
            base.Start();

            btnSubmit = UIUtils.GetButtonByName(this.gameObject, BTN_SUBMIT);
            btnSubmit.onClick.AddListener(OnSubmit);

            nameField = UIUtils.GetComponentFromGameObjectName<InputField>(this.gameObject, INPUT_NAME);
        }

        public void OnSubmit()
        {
            string name = nameField.text;

            Debug.Log("Captain: " + name);

            PlayerAccountManager pam = PlayerAccountManager.GetInstance();

            // Initialize a new player and save it.
            pam.InitializeNewPlayer(name);

            pam.SavePlayer(pam.CurrSaveSlot);

            // Close all the screens and then start loading
            List<AssetLoadRequestTO> requests = new List<AssetLoadRequestTO>();

            AssetLoadRequestTO baseData = 
                AssetLoadRequestTO.CreateMetadataAssetRequest(GameConstants.METADATA_BASE_FILE);
            requests.Add(baseData);

            AssetLoadRequestTO ep1 = AssetLoadRequestTO.CreateConversationRequest(GameConstants.EP01);
            requests.Add(ep1);


            EventController.GetInstance().RegisterForEvent(
                EventTypeEnum.AssetsLoadMultipleComplete, OnLoadCompleteEvent);

            AssetLoader.GetInstance().LoadAssets(requests);
        }

        public void OnLoadCompleteEvent(EventTypeEnum type, object obj)
        {
            EventController.GetInstance().UnregisterForEvent(
                EventTypeEnum.AssetsLoadMultipleComplete, OnLoadCompleteEvent);

            ScreenQueueManager.GetInstance().ClearQueueAndDestroyAllScreens();

            DialogController.GetInstance().StartConversation();
        }

    }
}
