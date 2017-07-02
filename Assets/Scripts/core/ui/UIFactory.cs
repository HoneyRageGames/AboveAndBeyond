/*** ---------------------------------------------------------------------------
/// UIUtils.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>June 4th, 2017</date>
/// ------------------------------------------------------------------------***/

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace core.ui
{
    /// <summary>
    /// A factory class meant to contain static methods to create new instances
    /// of UI Elements. Screens, modules, etc.
    /// </summary>
    public class UIFactory
    {
        private const string PREFAB_SCREEN_PATH = "Prefabs/Screens/";

        public const string SCR_CONVERSATION = "ConversationScreen";

        /// <summary>
        /// The prefab for the load file screen where the user can load their
        /// last saved game. 
        /// </summary>
        public const string SCR_LOAD_FILE = "LoadFileScreen";

        public const string SCR_NAME_INPUT = "NameInputScreen";

        public const string SCR_SAVE_FILE = "SaveFileScreen";

        public const string SCR_CONFIRM = "ConfirmScreen";

        private static List<string> validScreenNames;

        public static T CreateScreenAndAddComponent<T>(string screenName, GameObject parent) where T: MonoBehaviour
        {
            GameObject screen = CreateScreen(screenName, parent);

            T component = null;
            if (screen != null)
            {
                component = screen.AddComponent<T>();
            }

            return component;
        }

        public static GameObject CreateScreen(string screenName, GameObject parent)
        {
            if (!IsScreenDefined(screenName))
            {
                throw new Exception("Cannot create undefined screen: " + screenName);
            }

            // Show the load file screen
            GameObject resourceObj = Resources.Load<GameObject>(PREFAB_SCREEN_PATH + screenName);

            GameObject screen = GameObject.Instantiate(resourceObj);

            screen.name = screenName;

            if (parent != null)
            {
                screen.transform.SetParent(parent.transform, false);
            }

            return screen;
        }

        /// <summary>
        /// Based on the constants defined for the screens in this class, check to make
        /// sure that we're trying to open up a screen that has a constant defined. If
        /// it doesn't exist then the factory doesn't know how to make it.
        /// </summary>
        public static bool IsScreenDefined(string name)
        {
            // If we don't have a list to check then let's initialize the valid list.
            // We don't want to do this too often since this much reflection is quite
            // expensive
            if (validScreenNames == null)
            {
                validScreenNames = new List<string>();

                Type type = typeof(UIFactory);

                FieldInfo[] fieldInfos = type.GetFields(
                    // Gets all public and static fields
                    BindingFlags.Public | BindingFlags.Static |
                    // This tells it to get the fields from all base types as well
                    BindingFlags.FlattenHierarchy);

                // Iterate through all of the fieldInfos
                for (int i = 0, count = fieldInfos.Length; i < count; i++)
                {
                    FieldInfo fi = fieldInfos[i];

                    // Only if it's a const do we try to parse it

                    // IsLiteral determines if its value is written at compile time and not changeable
                    // IsInitOnly determine if the field can be set in the body of the constructor
                    // for C# a field which is read only keyword would have both true 
                    //   but a const field would have only IsLiteral equal to true
                    if (fi.IsLiteral && !fi.IsInitOnly)
                    {
                        string constValue = fi.GetValue(type) as string;

                        if (!string.IsNullOrEmpty(constValue))
                        {
                            Debug.Log("Constant: " + constValue);
                            validScreenNames.Add(constValue);
                        }
                    }
                }
            }

            return validScreenNames.Contains(name);
        }
    }
}