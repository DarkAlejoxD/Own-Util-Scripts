using System;
using UnityEngine;
using DialogueSystem.Conditions;

namespace DialogueSystem
{
    [Serializable]
    public class ConditionAndDialoguePair
    {
        [SerializeReference] public DialogueConditionObject Condition;
        [SerializeReference] public Branch ConditionedBranch;

        public ConditionAndDialoguePair()
        {
            ConditionedBranch ??= new();
        }
    }
}