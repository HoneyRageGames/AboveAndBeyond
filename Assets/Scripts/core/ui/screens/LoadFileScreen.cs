/*** ---------------------------------------------------------------------------
/// LoadFileScreen.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>June 4th, 2017</date>
/// ------------------------------------------------------------------------***/

using core.constants;
using core.player;
using core.ui.modules;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace core.ui.screens
{
    /// <summary>
    /// Handles the functionality of the load file selection screen.
    /// </summary>
    public class LoadFileScreen : BaseScreenComponent
    {
        private GameObject cachedSaveFileAsset;


        // Use this for initialization
        public override void Start()
        {
            base.Start();

            //// add all the save slots
            for (int i = 0; i < GameConstants.NUM_SAVE_SLOTS; i++)
            {
                // Try loading up a save file
                SaveData save = PlayerAccountManager.GetInstance().LoadSaveFile(PlayerAccountManager.SAVE_FILE_PREFIX + i);

                // If we didn't find a save file in that slot then make an empty one
                if (save == null)
                {
                    save = new SaveData(null);
                    
                }

                GetNewSaveFileButton(save, i);
            }
        }

        public void Close()
        {
            Debug.Log("Destroy LoadFileScreen");

            Destroy(this.gameObject);
        }

        private GameObject GetNewSaveFileButton(SaveData saveData, int slot)
        {
            if (cachedSaveFileAsset == null)
            {
                // Show the load file screen
                cachedSaveFileAsset = Resources.Load<GameObject>("Prefabs/SaveButton");
            }

            GameObject saveFileButton = GameObject.Instantiate(cachedSaveFileAsset);
            saveFileButton.name = "saveFile";

            // Add it to the title game object
            GameObject grid = GameObject.Find("SaveFileGridPanel");

            saveFileButton.transform.SetParent(grid.transform, false);
            LoadFileButtonModule module = saveFileButton.AddComponent<LoadFileButtonModule>();
            module.SetData(saveData.CreationDateTime, saveData.LastSaveDateTime, slot, saveData.PlayerName);

            return saveFileButton;
        }
    }
}
