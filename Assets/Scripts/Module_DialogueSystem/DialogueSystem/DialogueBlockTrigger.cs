using DialogueSystem;
using DialogueSystem.Conditions;
using UnityEngine;
using UtilsComplements;

[RequireComponent(typeof(Collider))]
public class DialogueBlockTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DialogueConditionObject _canPassConditionObject;
    [SerializeField] private DialogueObject _blockDialogue;
    [SerializeField] private Collider _block;

    [Header("Not operator")]
    [Tooltip("If False -> Will block until the condition is true.\n" +
             "\tEx: You didn't talked to your abuela, you can't pass. (Normal behaviour) \n\n" +
             "If true -> will block if the condition is true. \n" +
             "\tEx: You already talked to your abuela, you can't pass.(Inversed behaviour)")]
    [SerializeField] private bool _ifNot = false;

    private void Start()
    {
        _block.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        bool canPass = _canPassConditionObject.DialogueCondition();
        if (_ifNot) canPass = !canPass;

        if (!canPass)
        {
            _block.enabled = true;
            if (ISingleton<DialogueManager>.TryGetInstance(out DialogueManager manager))
                manager.SetNewStory(_blockDialogue);
        }
        else { _block.enabled = false; }
    }
}
