/*** ---------------------------------------------------------------------------
/// CharacterVO.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>May 18th, 2017</date>
/// ------------------------------------------------------------------------***/

using System;

namespace core.data.vo
{
    /// <summary>
    /// Contains the data pertaining to a character. 
    /// </summary>
    [Serializable]
    public class CharacterVO : BaseVO
    {
        public int damageReduction;
        public double dodgeChance;
        public double hitChance;
    }
}