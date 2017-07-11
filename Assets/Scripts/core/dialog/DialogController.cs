/*** ---------------------------------------------------------------------------
/// DialogController.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>April 22nd, 2017</date>
/// ------------------------------------------------------------------------***/

using core.assets;
using core.audio;
using core.constants;
using core.events;
using core.player;
using core.tilesys;
using core.ui;
using core.ui.screens;
using core.util;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace core.dialog
{
    /// <summary>
    /// The main controller for handling dialog and conversations.
    /// </summary>
    public class DialogController
    {
        /// <summary>
        /// The parameter modifier that indicates that we want to
        /// open a screen. 
        /// </summary>
        private const string PMOD_SCREEN_QUEUE = "ScreenQueue";

        private const string PMOD_MAP_PRELOAD = "MapPreload";
        private const string PMOD_MAP_SHOW = "MapShow";

        private static DialogController instance;

        private Conversation currConv;
        public ConversationNode currNode { get; private set; }

        //private Text bodyTextField;
        //private Image portrait;
        private Animator animPortrait;

        private Dictionary<string, Sprite> portraitSprites;
        private List<GameObject> buttonGameObjs;

        private DialogController()
        {
            portraitSprites = new Dictionary<string, Sprite>();
            buttonGameObjs = new List<GameObject>();
        }

        public static DialogController GetInstance()
        {
            if (instance == null)
            {
                instance = new DialogController();
            }

            return instance;
        }

        /// <summary>
        /// Loads the given dialog JSON file and processes all of the data.
        /// </summary>
        /// <param name="path"></param>
        public void LoadDialogJSON(string path)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(path);

            StringBuilder sb = new StringBuilder();

            sb.Append("{ ");
            sb.Append("\"nodes\":");
            sb.Append(textAsset.text);
            sb.Append("}");

            Conversation conv = JsonUtility.FromJson<Conversation>(sb.ToString());
            conv.uid = textAsset.name;

            // Set this new conversation to the current.
            currConv = conv;
        }

        public void LoadDialogFromTO(AssetLoadRequestTO to)
        {
            TextAsset textAsset = (TextAsset)to.loadedObject;

            StringBuilder sb = new StringBuilder();

            sb.Append("{ ");
            sb.Append("\"nodes\":");
            sb.Append(textAsset.text);
            sb.Append("}");

            Conversation conv = JsonUtility.FromJson<Conversation>(sb.ToString());
            conv.uid = textAsset.name;

            // Set this new conversation to the current.
            currConv = conv;
        }

        /// <summary>
        /// Begins a conversation and opens up the ConversationScreen.
        /// </summary>
        public void StartConversation()
        {
            if (currConv == null)
            {
                Debug.LogError("Cannot start a conversation if one was not loaded.");
                return;
            }

            Debug.Log("Start Conversation: " + currConv.uid);

            GameObject MainMenu = GameObject.Find(GameConstants.UI_MAIN_MENU);

            GameObject convScreenObj = UIFactory.CreateScreen(UIFactory.SCR_CONVERSATION, MainMenu);
            currNode = currConv.nodeMap[currConv.startNodeTitle];

            // Set the node to be displayed upon loading to the starting node
            ApplyParamModifiers(currNode);

            ScreenQueueManager sqm = ScreenQueueManager.GetInstance();
            sqm.ShowScreenNow(convScreenObj);
        }

        public void EndConversation()
        {
            SaveConversationComplete(currConv);
        }

        /// <summary>
        /// Select a choice at the given index
        /// </summary>
        /// <param name="choiceIndex"></param>
        public void SelectChoice(int choiceIndex)
        {
            ConversationChoice choice = currNode.choices[choiceIndex];

            // Save your choice
            SaveDecision(currNode, choiceIndex);

            currNode = currConv.nodeMap[choice.nextNodeTitle];

            EventController.GetInstance().FireEvent(EventTypeEnum.ShowNewConversationNode, currNode);

            // Apply the parameter modifiers upon viewing the choice
            ApplyParamModifiers(currNode);
        }

        /// <summary>
        /// We save a conversation is complete by saving the starting node id.
        /// </summary>
        /// <param name="conv"></param>
        private void SaveConversationComplete(Conversation conv)
        {
            ConversationNode startNode = conv.nodeMap[conv.startNodeTitle];

            // Flag that whole conversation tree as complete
            PlayerAccountManager.GetInstance().GetPlayer().SetValue<int>(startNode.treeID, 1);

            // Auto save the player
            PlayerAccountManager pm = PlayerAccountManager.GetInstance();
            pm.AutoSavePlayerToCurrentSlot();
        }

        private void SaveDecision(ConversationNode node, int choice)
        {
            PlayerAccountManager.GetInstance().GetPlayer().SetValue<int>(node.title, choice);
        }

        private void ApplyParamModifiers(ConversationNode node)
        {
            if (node.paramMods == null || node.paramMods.Count < 1)
            {
                return;
            }

            PlayerAccountManager pm = PlayerAccountManager.GetInstance();
            Player player = pm.GetPlayer();

            for (int i = 0, count = node.paramMods.Count; i < count; i++)
            {
                ConversationParamModifier mod = node.paramMods[i];

                // If it's a music parameter then play the transition
                if (ParameterModifierUtils.IsMusicParameter(mod.paramName))
                {
                    if (mod.paramName == MusicController.DIALOG_PARAM_MUSIC_FADEIN)
                    {
                        MusicController.GetInstance().TransitionToNewSong(mod.strValue);
                    }

                    if (mod.paramName == MusicController.DIALOG_PARAM_MUSIC_FADEOUT)
                    {
                        MusicController.GetInstance().FadeOutCurrentSong();
                    }

                    continue;
                }

                if (ParameterModifierUtils.IsScreenParameter(mod.paramName))
                {
                    if (mod.paramName == PMOD_SCREEN_QUEUE)
                    {
                        GameObject MainMenu = GameObject.Find(GameConstants.UI_MAIN_MENU);
                        GameObject screen = UIFactory.CreateScreen(mod.strValue, MainMenu);
                        ScreenQueueManager.GetInstance().QueueScreen(screen);
                    }
                    continue;
                }

                if (ParameterModifierUtils.IsMapParameter(mod.paramName))
                {
                    // Preload a map
                    if (mod.paramName == PMOD_MAP_PRELOAD)
                    {
                        MapController.GetInstance().LoadMapByUID(mod.strValue);
                    }

                    if (mod.paramName == PMOD_MAP_SHOW)
                    {
                        MapController.GetInstance().ShowMap();
                    }

                    continue;
                }

                // Set the string or integer to the given value.
                if (mod.action == ConversationParamModifier.ModifierActionType.Set)
                {
                    if (mod.type == ConversationParamModifier.ModifierType.Integer)
                    {
                        player.SetValue<int>(mod.paramName, mod.intValue);
                    }
                    else
                    {
                        player.SetValue<string>(mod.paramName, mod.strValue);
                    }
                    continue;
                }

                if (mod.action == ConversationParamModifier.ModifierActionType.Increment)
                {
                    player.IncrementValue(mod.paramName, mod.intValue);
                    continue;
                }

                if (mod.action == ConversationParamModifier.ModifierActionType.Decrement)
                {
                    player.IncrementValue(mod.paramName, -mod.intValue);
                    continue;
                }
            }

            // Save the player progress at this point. 
            pm.AutoSavePlayerToCurrentSlot();
        }

        public void PreloadPortraits()
        {
            portraitSprites.Clear();
            // Unload the unnecessary assets
            Resources.UnloadUnusedAssets();

            List<AssetLoadRequestTO> requests = new List<AssetLoadRequestTO>();

            // Go through all of the nodes and make a list of sheets we need to load.
            List<ConversationNode> nodes = currConv.nodes;
            List<string> sheetsToLoad = new List<string>();
            for (int i = 0, count = nodes.Count; i < count; i++)
            {
                ConversationNode node = nodes[i];
                string sheetName = node.spriteSheet;

                if (!sheetsToLoad.Contains(sheetName))
                {
                    sheetsToLoad.Add(sheetName);

                    AssetLoadRequestTO to = AssetLoadRequestTO.CreateSpriteSheetAssetRequest(sheetName);
                    to.callback = OnSpriteSheetLoaded;

                    requests.Add(to);
                }
            }

            sheetsToLoad.Clear();
            sheetsToLoad = null;

            AssetLoader.GetInstance().LoadAssets(requests);
        }

        public void PreloadMusic()
        {
            List<AssetLoadRequestTO> requests = new List<AssetLoadRequestTO>();

            List<string> songsToLoad = new List<string>();

            List<ConversationNode> nodes = currConv.nodes;

            // Iterate through the conversation nodes and check to see if we need to
            // preload any new songs
            for (int i = 0, count = nodes.Count; i < count; i++)
            {
                ConversationNode node = nodes[i];
                List<ConversationParamModifier> paramMods = node.paramMods;

                // if there are no param modifiers then no need to preload any music
                if (paramMods == null)
                {
                    continue;
                }

                // iterate through the param mods and check for songs to preload

                for (int j = 0, jCount = paramMods.Count; j < jCount; j++)
                {
                    ConversationParamModifier mod = paramMods[j];

                    // Not a music parameter then skip it
                    if (!ParameterModifierUtils.IsMusicParameter(mod.paramName))
                    {
                        continue;
                    }

                    string songName = mod.strValue;

                    // If it's an empty fade out music param then skip
                    if (string.IsNullOrEmpty(songName))
                    {
                        continue;
                    }

                    // If we already queued it up to load then no need to double add it.
                    if (songsToLoad.Contains(songName))
                    {
                        continue;
                    }

                    // We got a new song to load so create a new TO
                    requests.Add(AssetLoadRequestTO.CreateMusicAssetRequest(songName));
                    songsToLoad.Add(songName);
                }
            }

            songsToLoad.Clear();
            songsToLoad = null;

            AssetLoader.GetInstance().LoadAssets(requests);
        }
        public Sprite GetPortraitSprite(string name)
        {
            if (portraitSprites.ContainsKey(name))
            {
                return portraitSprites[name];
            }

            return null;
        }

        /// <summary>
        /// Upon the sprite sheet being loaded, load all the portrait sprites into
        /// </summary>
        /// <param name="to"></param>
        private void OnSpriteSheetLoaded(AssetLoadRequestTO to)
        {
            StringBuilder log = new StringBuilder();
            log.Append("Portraits Loaded: \n");

            Object[] sprites = to.loadObjectList;

            for (int i = 0, count = sprites.Length; i < count; i++)
            {
                Sprite portrait = sprites[i] as Sprite;

                if (portrait == null)
                {
                    continue;
                }

                portraitSprites[portrait.name] = portrait;

                log.Append(portrait.name + "\n");
            }

            Debug.Log(log.ToString());
        }

        /// <summary>
        /// Visibly hides all of the choice buttons.
        /// </summary>
        private void HideChoices()
        {
            for (int i = 0, count = buttonGameObjs.Count; i < count; i++)
            {
                buttonGameObjs[i].SetActive(false);
            }
        }

        /// <summary>
        /// Display a given choice node
        /// </summary>
        private void ShowChoice(ConversationChoice choice, int index)
        {
            GameObject gameObj = buttonGameObjs[index];
            gameObj.SetActive(true);
            Button btn = gameObj.GetComponent<Button>();
            Text textField = btn.GetComponentInChildren<Text>();
            textField.text = choice.text;
        }
    }
}