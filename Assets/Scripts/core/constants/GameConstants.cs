/*** ---------------------------------------------------------------------------
/// GameConstants.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>June 4th, 2017</date>
/// ------------------------------------------------------------------------***/

namespace core.constants
{
    public class GameConstants
    {
        public const string CONVERSATION_LOC = "JSON/Episodes/";
        public const string EP01 = "ep01";

        public const string METADATA_LOC = "JSON/Metadata/";
        public const string METADATA_BASE_FILE = "metadata";

        public const string MUSIC_LOC = "Music/";
        public const string SONG_1 = "Factory-On-Mercury_Looping";
        public const string SONG_TITLE = "Retro-Sci-Fi-Planet_Looping";

        public const string SOUND_LOC = "Sounds/";
        public const string SND_BUTTON = "sfx_sounds_button6";
        public const string SND_MENU = "sfx_menu_select2";

        /// <summary>
        /// How long we want the splash screen to display for in seconds
        /// </summary>
        public const int SPLASH_DUR_SEC = 5;

        /// <summary>
        /// How many save slots are available to the user.
        /// </summary>
        public const int NUM_SAVE_SLOTS = 4;

        public const string UI_MAIN_MENU = "MainMenu";
    }
}