/*** ---------------------------------------------------------------------------
/// SaveFileScreen.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>June 4th, 2017</date>
/// ------------------------------------------------------------------------***/

using core.player;
using core.ui.modules;
using UnityEngine;

namespace core.ui.screens
{
    public class SaveFileScreen : BaseScreenComponent
    {
        private GameObject cachedSaveFileAsset;

        // Use this for initialization
        public override void Start()
        {
            base.Start();

            //// add all the save slots
            for (int i = 0; i < 4; i++)
            {
                // Try loading up a save file
                SaveData save = PlayerAccountManager.GetInstance().LoadSaveFile(PlayerAccountManager.SAVE_FILE_PREFIX + i);

                // If we didn't find a save file in that slot then make an empty one
                if (save == null)
                {
                    save = new SaveData(null);

                }

                AddNewSaveFileButton(save, i);
            }
        }

        private GameObject AddNewSaveFileButton(SaveData saveData, int slot)
        {
            if (cachedSaveFileAsset == null)
            {
                // Show the load file screen
                cachedSaveFileAsset = Resources.Load<GameObject>("Prefabs/Modules/SaveButton");
            }

            GameObject saveFileButton = GameObject.Instantiate(cachedSaveFileAsset);
            saveFileButton.name = "saveFile";

            // Add it to the title game object
            GameObject grid = GameObject.Find("SaveFileGridPanel");

            saveFileButton.transform.SetParent(grid.transform, false);
            SaveFileButtonModule module = saveFileButton.AddComponent<SaveFileButtonModule>();
            module.SetData(saveData.CreationDateTime, saveData.LastSaveDateTime, slot, saveData.PlayerName);

            return saveFileButton;
        }
    }
}
