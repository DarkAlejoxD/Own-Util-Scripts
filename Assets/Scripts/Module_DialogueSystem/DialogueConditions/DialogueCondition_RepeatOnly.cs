using UnityEngine;

namespace DialogueSystem.Conditions
{
    #region Report
    //Made by DarkAlejoxD

    //Current State: Archived
    //Last checked: February 2024
    //Last modification: February 2024

    //Direct dependencies of classes if imported file by file:
    //  -   All the DialogueSystem Folder

    //Commentaries:
    //  -   Helps to create a dialogue that shows once or limited times
    #endregion

    [CreateAssetMenu(fileName = "New RepeatOnlyX Condition", menuName = "DialogueSystem/Conditions/new RepeatOnlyX", order = 1)]
    public class DialogueCondition_RepeatOnly : DialogueConditionObject
    {
        [SerializeField] private int _timesToRepeat;
        private int _repeatCount;

        private void OnEnable()
        {
            _repeatCount = 0;
        }

        public override bool DialogueCondition()
        {
            if (CanRepeat())
            {
                _repeatCount++;
                return true;
            }

            return false;
        }

        private bool CanRepeat()
        {
            return _repeatCount < _timesToRepeat;
        }
    }
}