using System.Collections;
using UnityEngine;
using UtilsComplements;

namespace DialogueSystem
{

    [CreateAssetMenu(fileName = "New Dialogue Condition", menuName = "DialogueSystem/Conditions/DEBUG Condition")]
    public class DialogueConditionObject : ScriptableObject
    {
        [SerializeField, HideInInspector] private bool DEBUG_testCondition;
        public virtual bool DialogueCondition()
        {
            return DEBUG_testCondition;
        }
    }

    public abstract class DialogueEndActionObject : ScriptableObject
    {
        private const float TIME_DEALYED = 1;
        public void OnDialogueEnd()
        {
            //if (ISingleton<GameManager>.TryGetInstance(out var manager))
            //{
            //    manager.StartCoroutine(DialogueEndCoroutine());
            //}
            //else
            //{
            //}
                OnDialogueEndDelayed();
        }

        public abstract void OnDialogueEndDelayed();

        private IEnumerator DialogueEndCoroutine()
        {
            yield return new WaitForSeconds(TIME_DEALYED);
            OnDialogueEndDelayed();
        }
    }
}
