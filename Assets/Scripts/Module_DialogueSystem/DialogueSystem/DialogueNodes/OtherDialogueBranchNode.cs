using System;
using UnityEngine;

namespace DialogueSystem
{
    [Serializable]
    public class OtherDialogueBranchNode : DialogueNode, ISubBranchDialogueNode
    {
        public bool IgnoreEndNode = false;
        public bool ReturnMainDialogue;
        public DialogueObject OtherBranch;
        private Branch _evaluatedBranch;

        public DialogueNode CurrentNode { get; private set; }

        public bool BranchEnded { get; private set; }

        public bool AlreadyEvaluating { get; private set; }

        public bool FoundEndNode { get; private set; }

        bool ISubBranchDialogueNode.IgnoreEndNode => IgnoreEndNode;

        /// <summary>
        /// Before this Node you should ask if return Main Dialogue
        /// </summary>
        /// <returns></returns>
        public override bool CanPassNextNode()
        {
            if (FoundEndNode)
            {
                if (IgnoreEndNode)
                    return true;

                else
                    return false;
            }

            if (BranchEnded)
                return true;

            return false;
        }

        public DialogueNode GetNextNode()
        {
            if (!AlreadyEvaluating)
            {
                _evaluatedBranch.ResetBranch();
                AlreadyEvaluating = true;
                CurrentNode = _evaluatedBranch.GetNextNode();
                FoundEndNode = _evaluatedBranch.EndNodeFound;
                BranchEnded = _evaluatedBranch.BranchEnded;
                return CurrentNode;
            }
            else
            {
                if (BranchEnded)                
                    return null;                

                CurrentNode = _evaluatedBranch.GetNextNode();

                FoundEndNode = _evaluatedBranch.EndNodeFound;
                BranchEnded = _evaluatedBranch.BranchEnded;
                return CurrentNode;
            }
        }

        public void Reset()
        {
            _evaluatedBranch = OtherBranch.GetBranch();
            _evaluatedBranch.ResetBranch();
            AlreadyEvaluating = false;
            FoundEndNode = false;
            BranchEnded = false;
        }
    }
}