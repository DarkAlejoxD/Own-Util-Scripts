using DialogueSystem;
using DialogueSystem.Conditions;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class DialogueEventTrigger : MonoBehaviour
{
    [SerializeField] private DialogueConditionObject _canTriggerConditionObject;
    [SerializeField] private UnityEvent _onTriggerActivates;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (_canTriggerConditionObject != null)
        {
            Debug.Log("Reaches");
            if (!_canTriggerConditionObject.DialogueCondition())
                return;
        }

        Debug.Log("Activates");
        _onTriggerActivates?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerEnter(other);
    }
}