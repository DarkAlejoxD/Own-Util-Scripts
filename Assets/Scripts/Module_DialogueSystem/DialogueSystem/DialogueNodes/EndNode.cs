using System;

namespace DialogueSystem
{
    [Serializable]
    public class EndNode : DialogueNode
    {
        public override bool CanPassNextNode()
        {
            return false;
        }
    }
}