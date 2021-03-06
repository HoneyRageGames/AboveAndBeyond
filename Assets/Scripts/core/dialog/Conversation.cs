﻿/*** ---------------------------------------------------------------------------
/// Conversation.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>April 22nd, 2017</date>
/// ------------------------------------------------------------------------***/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace core.dialog
{
    /// <summary>
    /// Contains the raw nodes for the full conversation.
    /// </summary>
    [Serializable]
    public class Conversation : ISerializationCallbackReceiver
    {
        public static string TAG_START = "start";
        public static string TAG_END = "end";

        /// <summary>
        /// The unique identifier for this conversation
        /// </summary>
        public string uid;

        /// <summary>
        /// The list of conversation nodes. 
        /// </summary>
        public List<ConversationNode> nodes;

        /// <summary>
        /// A mapping of uid to a ConversationNode for quicker lookups
        /// </summary>
        public Dictionary<string, ConversationNode> nodeMap;

        /// <summary>
        /// A maping of tags to the list of ConversationNodes that have
        /// that tag applied to them.
        /// </summary>
        public Dictionary<string, List<ConversationNode>> nodesByTag;

        public string startNodeTitle;
        public string endNodeTitle;

        private bool alreadyProcessed = false;

        public Conversation()
        {
            nodeMap = new Dictionary<string, ConversationNode>();
            nodesByTag = new Dictionary<string, List<ConversationNode>>();
        }

        public void OnAfterDeserialize()
        {
            if (alreadyProcessed)
            {
                Debug.LogError("Already Processed cannot process twice: " + uid);
                return;
            }

            if (nodes == null || nodes.Count < 1)
            {
                Debug.LogError("Conversation does not have any nodes: " + uid);
                return;
            }

            // Go through all of the nodes and process them properly.
            for (int i = 0, count = nodes.Count; i < count; i++)
            {
                ConversationNode node = nodes[i];
                string key = node.title;

                // Add the node to the map of nodes by title
                nodeMap[key] = node;

                // Add the node to the tag mappings
                AddNodeToTags(node);
            }

            // Flag this conversation as processed so we don't go through the 
            // expensive task of processing all this data.
            alreadyProcessed = true;
        }

        public void OnBeforeSerialize()
        {
            // Nothing to do prior to serialization
        }

        /// <summary>
        /// Adds a node to the appropriate tags in the nodesByTag map.
        /// </summary>
        private void AddNodeToTags(ConversationNode node)
        {
            if (node.tagArray == null || node.tagArray.Length < 1)
            {
                return;
            }

            // Iterate through all the tags for the node
            for (int i = 0, count = node.tagArray.Length; i < count; i++)
            {
                string tag = node.tagArray[i];

                if (!nodesByTag.ContainsKey(tag))
                {
                    nodesByTag.Add(tag, new List<ConversationNode>());
                }

                List<ConversationNode> nodes = nodesByTag[tag];
                nodes.Add(node);

                // Save the title id to the starting node
                if (tag == TAG_START)
                {
                    startNodeTitle = node.title;
                }

                // Save the title id to the ending node
                if (tag == TAG_END)
                {
                    endNodeTitle = node.title;
                }
            }
        }
    }
}