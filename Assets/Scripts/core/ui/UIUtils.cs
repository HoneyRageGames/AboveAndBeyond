/*** ---------------------------------------------------------------------------
/// UIUtils.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>June 4th, 2017</date>
/// ------------------------------------------------------------------------***/

using UnityEngine;
using UnityEngine.UI;

namespace core.ui
{
    /// <summary>
    /// A utility class which contains only static methods around aiding common
    /// UI functionality
    /// </summary>
    public class UIUtils
    {
        /// <summary>
        /// Get a component off of a game object by name. Can return a null.
        /// </summary>
        public static T GetComponentFromGameObjectName<T>(
            GameObject parentObj, string name) where T: Component
        {
            // Try to the find the child transform by name
            Transform transf = parentObj.transform.Find(name);

            // If we couldn't find it then it'll be null
            if (transf == null)
            {
                return null;
            }

            // Now we can safely get the gameobject off of the found tranform
            GameObject obj = transf.gameObject;

            // Return the component
            return obj.GetComponent<T>();
        }

        /// <summary>
        /// Given a GameObject, try to get the Button component by name.
        /// </summary>
        public static Button GetButtonByName(GameObject parentObj, string name)
        {
            return GetComponentFromGameObjectName<Button>(parentObj, name);
        }

        /// <summary>
        /// Searches through the animation clips on an animator's RuntimeAnimatorController and returns
        /// the first clip that matches the name given
        /// </summary>
        public static AnimationClip GetAnimationClipByName(Animator animator, string clipName)
        {
            RuntimeAnimatorController animController = animator.runtimeAnimatorController;
            AnimationClip[] animClips = animController.animationClips;

            // Iterate through all of the clips and check their names
            for (int i = 0, count = animClips.Length; i < count; i++)
            {
                AnimationClip clip = animClips[i];

                if (clip.name == clipName)
                {
                    return clip;
                }
            }

            // We didn't find anything so return null
            return null;
        }
    }
}