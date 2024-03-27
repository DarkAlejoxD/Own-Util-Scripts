using System;
using UnityEngine;

namespace DialogueSystem
{
    [Serializable]
    public class DialogueNode
    {
        /// <summary>
        /// NODEID has only influence in DEBUG terms
        /// it should not provoke any error or influence in Runtime
        /// </summary>
        [HideInInspector] public string NodeID = "0";
        [TextArea(1, 2)] public string Text = "\n\n";
        public EntityEnum WhoTalks;
        public string EntityName = "Player";

        public virtual bool CanPassNextNode()
        {
            return true;
        }
    }
}