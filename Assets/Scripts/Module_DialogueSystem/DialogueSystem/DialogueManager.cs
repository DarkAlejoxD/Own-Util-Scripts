using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UtilsComplements;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour, ISingleton<DialogueManager>
    {
        private enum DialogueSystemStates
        {
            None, InGame, StartDialogue, TextAppearing, TextAppeared
        }

        public static Action OnDialogueStart;
        public static Action OnDialogueEnd;

        [Foldout("References", styled = true)]
        [SerializeField] private TMP_Text _entityText;
        [SerializeField] private TMP_Text _textCanvas;
        [SerializeField] private Canvas _dialogueCanvas;
        [SerializeField] private string _playerName;
        private bool _ended;

        [Foldout("Animations", styled = true)]
        [SerializeField] private AnimationClip _dialogueOnInAnim;
        [SerializeField] private AnimationClip _dialogueInStayAnim;
        [SerializeField] private AnimationClip _dialogueOnOutAnim;
        [SerializeField] private AnimationClip _dialogueOutStayAnim;
        [SerializeField] private Animation _animationController;

        [Foldout("Char Attributes", styled = true)]
        [SerializeField] private float _characterAppearTime = 0.01f;
        private bool _skipCharacterAnimation = false;
        private bool _animatingCharacters = false;

        [Foldout("DEBUG", styled = true)]
        [SerializeField] private DialogueObject DEBUG_dialogueObject;
        [SerializeField] private TMP_Text DEBUG_textIDCanvas;
        [SerializeField, Min(0.1f)] private float DEBUG_cooldownToTest = 1;
        [SerializeField] private bool DEBUG_debug = false;

        private Action _onDialogueClosed;
        public ISingleton<DialogueManager> Instance => this;
        public DialogueManager Value => this;
        private DialogueDecoder _dialogueDecoder;

        #region Unity Logic

        private void Awake()
        {
            Instance.Instantiate();
            _animationController.CrossFade(_dialogueOutStayAnim.name);
            _dialogueCanvas.gameObject.SetActive(false);
            //DEBUG_debug = true;
        }

        //private void OnEnable()
        //{
        //    InputManager.OnDialogueInput += DialogueInput;
        //}

        //private void OnDisable()
        //{
        //    InputManager.OnDialogueInput -= DialogueInput;
        //}

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.L) && DEBUG_debug)
            {
                SetNewStory(DEBUG_dialogueObject);
                StartCoroutine(DEBUG_CooldownCoroutine());
            }

            if (Input.GetKeyDown(KeyCode.H) && DEBUG_debug)
                DialogueInput();
#endif
        }

        private void DialogueInput()
        {
            if (_animatingCharacters)
            {
                _skipCharacterAnimation = true;
            }
            else
            {
                SetNextText();
            }
        }

        private void OnDestroy()
        {
            Instance.RemoveInstance();
        }

        #endregion

        #region Public Methods

        public void Invalidate()
        {
            Destroy(this.gameObject);
        }

        /// <summary> </summary>
        /// <param name="dialogueObject"></param>
        /// <returns>
        /// Return if the new Story got succesfully started.
        /// </returns>
        public bool SetNewStory(DialogueObject dialogueObject)
        {
            _ended = false;
            _dialogueDecoder = new(dialogueObject, _playerName);
            OnDialogueStart?.Invoke();

            OpenDialogue();
            SetNextText();
            return true;
        }

        public bool SetNewStory(DialogueObject dialogue, Action onClose)
        {
            _onDialogueClosed = onClose;
            return SetNewStory(dialogue);
        }

        public void AddDialogueEndAction(Action onEnd)
        {
            _onDialogueClosed += onEnd;
        }
        #endregion

        #region Private Methods

        private void SetNextText()
        {
            string text;
            string entity;
            string nodeID;

            _dialogueDecoder.GetNextText(out text, out entity, out nodeID, out _ended);

            DEBUG_textIDCanvas.text = "";//nodeID + " Ended: " + _ended;

            if (_ended)
            {
                CloseDialogue();
                _dialogueDecoder.DialogueEnded();
                return;
            }

            StartCoroutine(DisplayText(text));
            _entityText.text = entity;
        }

        #region Animations

        private void OpenDialogue()
        {
            _dialogueCanvas.gameObject.SetActive(true);
            _animationController.CrossFade(_dialogueOnInAnim.name);
            _animationController.CrossFadeQueued(_dialogueInStayAnim.name);
        }

        private void CloseDialogue()
        {
            StartCoroutine(EndingDialogueCoroutine());
            _onDialogueClosed?.Invoke();
            _onDialogueClosed = null;
        }

        #endregion

        #region Coroutines

        private IEnumerator EndingDialogueCoroutine()
        {
            float animDuration = _dialogueOnOutAnim.averageDuration;

            _animationController.CrossFade(_dialogueOnOutAnim.name);
            _animationController.CrossFadeQueued(_dialogueOutStayAnim.name);

            yield return new WaitForSeconds(animDuration);
            _dialogueCanvas.gameObject.SetActive(false);
            OnDialogueEnd?.Invoke();
        }

        private IEnumerator DisplayText(string text)
        {
            int characterIndex = 0;
            _textCanvas.text = "";
            WaitForSeconds wait = new WaitForSeconds(_characterAppearTime);
            _animatingCharacters = true;

            while (characterIndex < text.Length)
            {
                if (_skipCharacterAnimation)
                {
                    _skipCharacterAnimation = false;
                    break;
                }

                _textCanvas.text += text[characterIndex];
                //yield return StartCoroutine(CharacterEnterAnim(characterIndex));

                characterIndex++;
                yield return wait;
            }

            _textCanvas.text = text;
            _animatingCharacters = false;
        }

        #endregion

        #endregion

        #region DEBUG
#if UNITY_EDITOR
        private IEnumerator DEBUG_CooldownCoroutine()
        {
            DEBUG_debug = false;
            yield return new WaitForSeconds(DEBUG_cooldownToTest);
            DEBUG_debug = true;
        }
#endif
        #endregion
    }
}