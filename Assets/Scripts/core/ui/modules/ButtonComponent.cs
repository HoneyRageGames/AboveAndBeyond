/*** ---------------------------------------------------------------------------
/// SaveFileButtonModule.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>June 4th, 2017</date>
/// ------------------------------------------------------------------------***/

using UnityEngine;
using UnityEngine.UI;

namespace core.ui.modules
{
    /// <summary>
    /// This component is there to forward button behavior over to
    /// the UIInputController.
    /// </summary>
    public class ButtonComponent : MonoBehaviour
    {
        public Button button;


        public virtual void Start()
        {
            button = this.gameObject.GetComponent<Button>();
            button.onClick.AddListener(OnClicked);
        }

        protected virtual void OnClicked()
        {
        }
    }
}