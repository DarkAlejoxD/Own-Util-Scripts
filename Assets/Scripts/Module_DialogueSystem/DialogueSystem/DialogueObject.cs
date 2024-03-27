using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "DialogueSystem/New Dialogue", order = 1)]
    public class DialogueObject : ScriptableObject
    {
        [Header("NPC Values")]
        [HideInInspector] public Branch DialogueBranch;

        [HideInInspector] public Sprite NpcIcon;
        [HideInInspector] public string NpcName; //Dont know if necesary

        public Branch GetBranch()
        {
            DialogueBranch ??= new();
            return DialogueBranch;
        }
    }
}