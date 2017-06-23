/*** ---------------------------------------------------------------------------
/// PlayerAccountManager.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>April 24th, 2017</date>
/// ------------------------------------------------------------------------***/

using System;
using System.IO;
using UnityEngine;

namespace core.player
{
    /// <summary>
    /// Handles anything related to the player account.
    /// </summary>
    public class PlayerAccountManager
    {
        public const string SAVE_FILE_PREFIX = "savefile";

        private const string SAVE_DIR = "Saves/";
        private const string SAVE_FILE = "savefile0.sav";
        private const string SAVE_FILE_EXT = ".sav";

        /// <summary>
        /// The current save slot the player is referring to
        /// </summary>
        public int CurrSaveSlot = -1;

        private static PlayerAccountManager instance;

        private Player currentPlayer;

        private PlayerAccountManager()
        {
            currentPlayer = new Player();
        }

        public static PlayerAccountManager GetInstance()
        {
            if (instance == null)
            {
                instance = new PlayerAccountManager();
            }

            return instance;
        }

        public Player GetPlayer()
        {
            return currentPlayer;
        }

        public Player InitializeNewPlayer(string name)
        {
            currentPlayer = new Player();

            currentPlayer.PlayerName = name;

            // TODO: All the initialization logic here
            currentPlayer.CreationDate = DateTime.Now;

            return currentPlayer;
        }

        public void LoadPlayer(int slot)
        {
            string pullPath = SAVE_DIR + SAVE_FILE_PREFIX + slot + SAVE_FILE_EXT;
            SaveData save = LoadSaveFile(pullPath);

            if (save == null)
            {
                Debug.LogError("Could not find save file: " + pullPath);
                return;
            }

            CurrSaveSlot = slot;

            currentPlayer.LoadFromSaveData(save);
        }

        public SaveData LoadSaveFile(string fileName)
        {
            string fullPath = SAVE_DIR + fileName + SAVE_FILE_EXT;

            SaveData save = null;

            // Only if it exists do we try to load the file
            if (File.Exists(fullPath))
            {
                string rawSaveText = File.ReadAllText(fullPath);

                save = JsonUtility.FromJson<SaveData>(rawSaveText);
            }
            
            return save;
        }

        public void AutoSavePlayerToCurrentSlot()
        {
            if (CurrSaveSlot == -1)
            {
                Debug.LogError("Cannot save to undefined save slot!");
                return;
            }

            SavePlayer(CurrSaveSlot);
        }

        public void SavePlayer(int slot)
        {
            Debug.Log("Starting Save to Slot: " + slot);

            string fullPath = SAVE_DIR + SAVE_FILE_PREFIX + slot + SAVE_FILE_EXT;

            if (!Directory.Exists(SAVE_DIR))
            {
                Directory.CreateDirectory(SAVE_DIR);
            }

            DateTime startDate = DateTime.Now;

            currentPlayer.LastSaveDate = DateTime.Now;

            SaveData save = new SaveData(currentPlayer);

            string json = JsonUtility.ToJson(save, true);

            File.WriteAllText(fullPath, json);

            save = null;
            json = null;

            DateTime endDate = DateTime.Now;

            Debug.Log("Total Saving Time: " +
                endDate.Subtract(startDate).TotalMilliseconds + " MS");
        }
    }
}