/*** ---------------------------------------------------------------------------
/// ParameterModifierUtils.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>July 10th, 2017</date>
/// ------------------------------------------------------------------------***/

using core.audio;

namespace core.dialog
{
    public class ParameterModifierUtils
    {
        private const string PMOD_SCREEN_QUEUE = "ScreenQueue";

        private const string PMOD_MAP_PRELOAD = "MapPreload";
        private const string PMOD_MAP_SHOW = "MapShow";

        /// <summary>
        /// Is this PMOD related to something that needs to be done to the map?
        /// </summary>
        public static bool IsMapParameter(string paramName)
        {
            if (paramName == PMOD_MAP_PRELOAD ||
                paramName == PMOD_MAP_SHOW)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Is this conversation parameter modifier meant to trigger music?
        /// </summary>
        public static bool IsMusicParameter(string paramName)
        {
            if (paramName == MusicController.DIALOG_PARAM_MUSIC_FADEIN ||
                paramName == MusicController.DIALOG_PARAM_MUSIC_FADEOUT)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines if we have a screen specific parametere modifier that
        /// is meant to trigger.
        /// </summary>
        public static bool IsScreenParameter(string paramName)
        {
            if (paramName == PMOD_SCREEN_QUEUE)
            {
                return true;
            }

            return false;
        }
    }
}