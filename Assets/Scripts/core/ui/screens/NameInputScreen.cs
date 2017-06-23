/*** ---------------------------------------------------------------------------
/// NameInputScreen.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>June 11th, 2017</date>
/// ------------------------------------------------------------------------***/

using core.assets;
using core.constants;
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

            AssetLoadRequestTO to = new AssetLoadRequestTO();
            to.assetLoadType = AssetLoadType.Metadata;
            to.path = GameConstants.METADATA_LOC + GameConstants.METADATA_BASE_FILE;

            requests.Add(to);

            AssetLoadRequestTO ep1 = new AssetLoadRequestTO();
            ep1.assetLoadType = AssetLoadType.Conversation;
            ep1.path = GameConstants.CONVERSATION_LOC + GameConstants.EP01;
            requests.Add(ep1);


            AssetLoader.GetInstance().LoadAssets(requests);
        }
    }
}
