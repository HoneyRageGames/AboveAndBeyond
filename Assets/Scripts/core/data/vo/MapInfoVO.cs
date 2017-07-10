/// MapInfoVO.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>July 9th, 2017</date>
/// ------------------------------------------------------------------------

using System;

namespace core.data.vo
{
    [Serializable]
    public class MapInfoVO : BaseVO
    {
        /// <summary>
        /// The name of the tiled CSV file for this map
        /// </summary>
        public string tileMapCSV;

        /// <summary>
        /// The name of the material needed for the tile map
        /// </summary>
        public string material;
    }
}