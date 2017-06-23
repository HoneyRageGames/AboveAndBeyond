/*** ---------------------------------------------------------------------------
/// GameEvent.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>June 4th, 2017</date>
/// ------------------------------------------------------------------------***/

using UnityEngine.Events;

namespace core.events
{
    /// <summary>
    /// A wrapper for the UnityEvent that takes a parameter of type object
    /// </summary>
    public class GameEvent : UnityEvent<object>
    {
    }
}