/*** ---------------------------------------------------------------------------
/// ForcePixelPerfectTextComponent.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>June 4th, 2017</date>
/// ------------------------------------------------------------------------***/

using UnityEngine;
using UnityEngine.UI;

namespace core.ui.util
{
    /// <summary>
    /// Forces the font of first text item it finds to be a point filter.
    /// </summary>
    public class ForcePixelPerfectTextComponent : MonoBehaviour
    {
        [ExecuteInEditMode]
        public void Start()
        {
            GetComponent<Text>().font.material.mainTexture.filterMode = FilterMode.Point;
        }
    }
}