/*** ---------------------------------------------------------------------------
/// TitleScreen.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>June 4th, 2017</date>
/// ------------------------------------------------------------------------***/

using core.assets;
using core.audio;
using core.constants;
using UnityEngine;
using UnityEngine.UI;

namespace core.ui.screens
{
    public class TitleScreen : BaseScreenComponent
    {
        private const string BTN_NEWGAME = "NewGame";
        private const string BTN_CONTINUE = "Continue";

        private const string TRG_CONTINUE = "continue";
        private const string TRG_RETURN_TO_TITLE = "returnToTitle";

        private Button newGameBtn;
        private Button continueBtn;

        private Animator anim;

        private bool isActive = true;

        // Use this for initialization
        public override void Start()
        {
            base.Start();

            // This guy is not closeable from the Screen Queue since it is the title screen
            base.isCloseable = false;

            ScreenQueueManager.GetInstance().ShowScreenNow(this.gameObject);

            anim = GetComponent<Animator>();

            newGameBtn = UIUtils.GetButtonByName(this.gameObject, BTN_NEWGAME);
            newGameBtn.onClick.AddListener(OnNewGameClicked);

            continueBtn = UIUtils.GetButtonByName(this.gameObject, BTN_CONTINUE);
            continueBtn.onClick.AddListener(OnContinueClicked);

            LoadMusic();
        }

        // Update is called once per frame
        public void Update()
        {

        }

        public void OnEnable()
        {
        }

        private void LoadMusic()
        {
            AssetLoadRequestTO to = new AssetLoadRequestTO();
            to.assetLoadType = AssetLoadType.Music;
            to.path = GameConstants.MUSIC_LOC + GameConstants.SONG_TITLE;
            to.callback = OnMusicLoaded;

            AssetLoader.GetInstance().LoadAsset(to);
        }

        private void OnMusicLoaded(AssetLoadRequestTO to)
        {
            MusicController.GetInstance().TransitionToNewSong(GameConstants.SONG_TITLE);
        }

        private void OnNewGameClicked()
        {
            if (!isActive)
            {
                return;
            }

            // Add it to the title game object
            GameObject MainMenu = GameObject.Find(GameConstants.UI_MAIN_MENU);

            ConfirmScreen confirmScreen =
                UIFactory.CreateScreenAndAddComponent<ConfirmScreen>(UIFactory.SCR_CONFIRM, MainMenu);

            confirmScreen.SetData(UIConstants.NEW_GAME_TITLE, UIConstants.NEW_GAME_DESC, StartNewGame, null);

            ScreenQueueManager sqm = ScreenQueueManager.GetInstance();
            sqm.ShowScreenNow(confirmScreen.gameObject);
            sqm.QueueScreenAsNext(this.gameObject);
        }

        private void StartNewGame()
        {
            Debug.Log("Start New Game");

            // Add it to the title game object
            GameObject MainMenu = GameObject.Find(GameConstants.UI_MAIN_MENU);

            GameObject saveFileScreen = UIFactory.CreateScreen(UIFactory.SCR_SAVE_FILE, MainMenu);

            ScreenQueueManager sqm = ScreenQueueManager.GetInstance();

            sqm.QueueScreenAsNext(saveFileScreen);
            sqm.QueueScreen(this.gameObject);
        }

        private void OnContinueClicked()
        {
            if (!isActive)
            {
                return;
            }

            //SetButtonStatus(false);

            // Add it to the title game object
            GameObject MainMenu = GameObject.Find(GameConstants.UI_MAIN_MENU);

            GameObject loadFileScreen = UIFactory.CreateScreen(UIFactory.SCR_LOAD_FILE, MainMenu);

            ScreenQueueManager sqm = ScreenQueueManager.GetInstance();

            sqm.ShowScreenNow(loadFileScreen);
            sqm.QueueScreenAsNext(this.gameObject);
        }

        private void SetButtonStatus(bool state)
        {
            continueBtn.interactable = state;
            newGameBtn.interactable = state;
        }
    }
}