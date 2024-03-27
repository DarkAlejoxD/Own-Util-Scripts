using DialogueSystem;
using UnityEngine;

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
