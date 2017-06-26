/*** ---------------------------------------------------------------------------
/// EventTypeEnum.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>June 4th, 2017</date>
/// ------------------------------------------------------------------------***/

namespace core.events
{ 
    /// <summary>
    /// An enum containing all of the types of events we want to listen for
    /// </summary>
    public enum EventTypeEnum
    {
        /// <summary>
        /// The user pressed any button
        /// </summary>
        ButtonPressed,

        /// <summary>
        /// The pressed the escape button specifically
        /// </summary>
        EscapePressed,

        AssetsLoadMultipleComplete,

        AssetLoadComplete,

        ShowNewConversationNode,
    }
}