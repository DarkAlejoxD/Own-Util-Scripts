using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace DialogueSystem
{
    [Serializable]
    public class ConditionedDialogueNode : DialogueNode, ISubBranchDialogueNode
    {
        public List<ConditionAndDialoguePair> ConditionedBranch;
        [SerializeReference] public Branch DefaultBranch;
        private const int MAX_CONDITIONS = 7;
        private const bool IGNORE_END = false;
        private Branch _evaluatedBranch;

        public DialogueNode CurrentNode { get; private set; }
        public bool AlreadyEvaluating { get; private set; }
        public bool BranchEnded { get; private set; }
        public bool FoundEndNode { get; private set; }
        public bool IgnoreEndNode => IGNORE_END;

        public ConditionedDialogueNode() : base()
        {
            ConditionedBranch ??= new(MAX_CONDITIONS);
            DefaultBranch ??= new();
            _evaluatedBranch = DefaultBranch;
        }

        /// <summary>
        /// Function intended to EditorMode
        /// </summary>
        /// <returns></returns>
        public bool CanAddCondition()
        {
            return ConditionedBranch.Count < MAX_CONDITIONS;
        }

        /// <summary>
        /// Function intended to EditorMode
        /// </summary>
        public void AddCondition()
        {
            ConditionedBranch ??= new List<ConditionAndDialoguePair>();
            DefaultBranch ??= new Branch();

            ConditionedBranch.Add(new ConditionAndDialoguePair());
        }

        public override bool CanPassNextNode()
        {
            if (FoundEndNode)
            {
                if (IgnoreEndNode)
                    return true;

                else
                    return false;
            }

            if (_evaluatedBranch.BranchEnded)
                return true;

            return false;
        }

        public void Reset()
        {
            AlreadyEvaluating = false;
            FoundEndNode = false;
            BranchEnded = false;
            DefaultBranch.ResetBranch();
            foreach (var item in ConditionedBranch)
            {
                item.ConditionedBranch.ResetBranch();
            }
        }

        public DialogueNode GetNextNode()
        {
            if (!AlreadyEvaluating)
            {
                bool conditionTrue = false;
                if (ConditionedBranch.Count > 0)
                {
                    for (int i = 0; i < ConditionedBranch.Count; i++)
                    {
                        if (ConditionedBranch.ElementAt(i).Condition.DialogueCondition())
                        {
                            _evaluatedBranch = null;
                            _evaluatedBranch = ConditionedBranch.ElementAt(i).ConditionedBranch;
                            conditionTrue = true;
                            break;
                        }
                    }
                }
                if (!conditionTrue)
                    _evaluatedBranch = DefaultBranch;

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
                {
                    return null;
                }

                CurrentNode = _evaluatedBranch.GetNextNode();

                FoundEndNode = _evaluatedBranch.EndNodeFound;
                BranchEnded = _evaluatedBranch.BranchEnded;
                return CurrentNode;
            }
        }
    }
}