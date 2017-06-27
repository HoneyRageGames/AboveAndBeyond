/*** ---------------------------------------------------------------------------
/// ConversationScreen.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>June 25th, 2017</date>
/// ------------------------------------------------------------------------***/

using core.constants;
using core.dialog;
using core.events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace core.ui.screens
{
    public class ConversationScreen : BaseScreenComponent
    {
        private const int NUM_CHOICES = 3;

        private const string IMG_PORTRAIT = "portrait";
        private const string TXT_DIALOG = "dialogText";
        private const string GRID_CHOICES = "choicesGrid";

        private Image portraitImg;
        private Text dialogText;
        private GridLayoutGroup choicesGrid;
        private List<Button> choices;
        private List<Text> choicesText;

        private ConversationNode currNode;

        private bool initialized;

        public void DisplayConversationNode(ConversationNode node)
        {
            currNode = node;

            if (!initialized)
            {
                return;
            }

            HideChoices();

            // Load the correct image
            portraitImg.sprite = DialogController.GetInstance().GetPortraitSprite(currNode.spriteName);

            // Display the dialog text
            dialogText.text = currNode.displayBody;

            // If it's the last node then we can assume there's no choice to be made and 
            // all that's necessary is a continue
            if (currNode.tags.Contains(Conversation.TAG_END))
            {
                // If it's the end of the conversation or there are no choices continue;
                choices[0].gameObject.SetActive(true);
                choicesText[0].text = UIConstants.CONV_CONTINUE;
                return;
            }

            // Iterate through the choices and set up the choice buttons
            for (int i = 0; i < currNode.choices.Count; i++)
            {
                ConversationChoice choice = currNode.choices[i];

                Button choiceBtn = choices[i];

                // Re-enable it to make it visible.
                choiceBtn.gameObject.SetActive(true);

                Text choiceTxt = choicesText[i];
                choiceTxt.text = i + ") " + choice.text;
            }
        }

        // Use this for initialization
        public override void Start()
        {
            base.Start();

            portraitImg = UIUtils.GetComponentFromGameObjectName<Image>(this.gameObject, "portrait");
            dialogText = UIUtils.GetComponentFromGameObjectName<Text>(this.gameObject, "dialogText");
            choicesGrid = UIUtils.GetComponentFromGameObjectName<GridLayoutGroup>(this.gameObject, "choicesGrid");

            choices = new List<Button>();
            choicesText = new List<Text>();

            for (int i = 0; i < NUM_CHOICES; i++)
            {
                Button btn = UIUtils.GetComponentFromGameObjectName<Button>(choicesGrid.gameObject, "choice" + i);
                choices.Add(btn);

                Text txt = btn.GetComponentInChildren<Text>();
                txt.text = "Choice " + (i + 1);

                choicesText.Add(txt);

                btn.onClick.AddListener(delegate
                    {
                        OnChoiceSelected(btn);
                    });
            }

            // Register to listen for events from the DialogController
            EventController.GetInstance().RegisterForEvent(EventTypeEnum.ShowNewConversationNode, OnNewConversationNode);

            initialized = true;

            DisplayConversationNode(currNode);
        }

        public override void OnDestroy()
        {
            EventController.GetInstance().UnregisterForEvent(EventTypeEnum.ShowNewConversationNode, OnNewConversationNode);
            base.OnDestroy();
        }

        private void OnNewConversationNode(EventTypeEnum type, object obj)
        {
            currNode = (ConversationNode)obj;
            DisplayConversationNode(currNode);
        }

        private void OnChoiceSelected(Button button)
        {
            int index = choices.IndexOf(button);

            Debug.Log("Choice " + index + " selected");

            // If we're at the end then kill this screen
            if (currNode.tags.Contains(Conversation.TAG_END))
            {
                GameObject.Destroy(this.gameObject);
                DialogController.GetInstance().EndConversation();
                return;
            }

            DialogController.GetInstance().SelectChoice(index);
        }

        /// <summary>
        /// Visibly hides all of the choice buttons.
        /// </summary>
        private void HideChoices()
        {
            for (int i = 0, count = choices.Count; i < count; i++)
            {
                choices[i].gameObject.SetActive(false);
            }
        }
    }
}