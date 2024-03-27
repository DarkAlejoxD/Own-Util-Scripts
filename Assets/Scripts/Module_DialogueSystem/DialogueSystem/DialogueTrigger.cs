using DialogueSystem;
using UnityEngine;
using UtilsComplements;

[RequireComponent(typeof(Collider))]
public class DialogueTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DialogueObject _dialogue;
    [SerializeField] private DialogueConditionObject _condition;
    private bool _alreadyUsed;

    private void Awake()
    {
        _alreadyUsed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_alreadyUsed)
            return;

        if (_condition != null)
        {
            bool canTrigger = _condition.DialogueCondition();
            if (!canTrigger)
                return;
        }

        if (other.CompareTag("Player"))
        {
            if (ISingleton<DialogueManager>.TryGetInstance(out DialogueManager manager))
            {
                manager.SetNewStory(_dialogue);
                _alreadyUsed = true;
            }
        }
    }
}
