using System;
using UnityEngine;

namespace DialogueSystem
{
    [Serializable]
    public class EndNodeAction : EndNode
    {
        [SerializeField] public DialogueEndActionObject DialogueEndReference;
    }
}