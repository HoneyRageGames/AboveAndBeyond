/// ---------------------------------------------------------------------------
/// MapData.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>March 6th, 2017</date>
/// ---------------------------------------------------------------------------

using core.assets;
using System;
using UnityEngine;

namespace core.data
{
    /// <summary>
    /// Contains the raw tile data for a map
    /// </summary>
    public class MapData
    {
        private string[,] tileData;

        private MapData(string[,] data)
        {
            tileData = data;

            Debug.Log("Width: " + GetWidth().ToString());
            Debug.Log("Height: " + GetHeight().ToString());
        }

        public string GetTileAt(int x, int y)
        {
            return tileData[x, y];
        }

        public int GetHeight()
        {
            return tileData.GetLength(1);
        }

        public int GetWidth()
        {
            return tileData.GetLength(0);
        }

        /// <summary>
        /// From the TO return a fresh MapData
        /// </summary>
        public static MapData FromTO(AssetLoadRequestTO to)
        {
            TextAsset textAsset = (TextAsset)to.loadedObject;

            string[,] tileMapLayout = RawToStringArray(textAsset.text);

            return new MapData(tileMapLayout);
        }

        /// <summary>
        /// From the raw text from the map data csv file we parse it out
        /// into a two-dimensional array.
        /// </summary>
        private static string[,] RawToStringArray(string rawData)
        {
            // Create the new line splitter
            string[] newLineSplit = new string[] { System.Environment.NewLine };

            // Since this is a grid, split it by the new line first
            string[] rows = rawData.Split(newLineSplit, StringSplitOptions.None);

            // The height of the tile map. We need to subtract 1 since Tiled
            // tends to add an extra new line in there at the end. 
            int tileHeight = rows.Length - 1;

            string[,] tileMapLayout = null;

            // Go through from the bottom up since Unity likes a right up coordinate
            // system and tile prefers a right down we gotta compensate for that. 
            for (int y = tileHeight - 1; y >= 0; y--)
            {
                // Used to handle an empty row due to the extra new line
                // added by Tiled
                if (rows[y].Length == 0)
                {
                    continue;
                }

                // Separate out each tile out by the comma
                string[] columns = rows[y].Split(',');

                // If we haven't made the map yet we can do it now since at this 
                // point we know the full dimensions of the map.
                if (tileMapLayout == null)
                {
                    int tileWidth = columns.Length;

                    tileMapLayout = new string[tileWidth, tileHeight];

                }

                // Now go through the row, column by column, and add their tile
                // to the two dimensional array. 
                for (int x = 0, hCount = columns.Length; x < hCount; x++)
                {
                    tileMapLayout[x, tileHeight - y - 1] = columns[x];
                }
            }

            return tileMapLayout;
        }
    }
}