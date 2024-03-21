using System;
using UnityEngine;
//using DialogueSystem;

namespace UtilsComplements
{
    #region Report
    //Last checked: February 2024
    //Last modification: February 2024

    //Commentaries:
    //  -   Cheat class implementation
    #endregion

    /// <summary> 
    /// Allows play with the Time Scale, mantain in your code if you want to test in build 
    /// </summary>
    public class TimeScaleControllerCheat : Cheat, ISingleton<TimeScaleControllerCheat>
    {
        private const string TIME_CHEAT = "CONTROL";
        [SerializeField, Range(0, 5)] protected float _timeScaled = 1;
        private float _previousTimeScaled;

        //[Header("Specific action\nDelete when import to another project")]
        //[SerializeField] private DialogueObject _cheatDialogue;

        protected override string CHEAT_NAME => TIME_CHEAT;

        public ISingleton<TimeScaleControllerCheat> Instance => this;
        public TimeScaleControllerCheat Value => this;

        protected override void Awake()
        {
            base.Awake();
            Instance.Instantiate();
        }

        private void Start()
        {
            _previousTimeScaled = _timeScaled;
            Time.timeScale = _timeScaled;
            Time.timeScale = 1;
        }

        protected override void Update()
        {
            base.Update();
            CheckIfChanged();

            if (_cheatActivated)
                InputChange();
        }

        private void InputChange()
        {
            if (Input.GetKeyUp(KeyCode.Alpha1)) _timeScaled = 1;
            if (Input.GetKeyUp(KeyCode.Alpha2)) _timeScaled = 2;
            if (Input.GetKeyUp(KeyCode.Alpha3)) _timeScaled = 3;
            if (Input.GetKeyUp(KeyCode.Alpha4)) _timeScaled = 4;
            if (Input.GetKeyUp(KeyCode.Alpha5)) _timeScaled = 5;
        }

        private void CheckIfChanged()
        {
            if (_previousTimeScaled != _timeScaled)
            {
                _timeScaled = Mathf.Clamp(_timeScaled, 0, 100);
                _previousTimeScaled = _timeScaled;
                Time.timeScale = _previousTimeScaled;
            }
        }

        private void OnDestroy()
        {
            Instance.RemoveInstance();
        }

        protected override void OnCheat()
        {
            //if (ISingleton<DialogueManager>.TryGetInstance(out var dialogue))
            //{
            //    if (_cheatDialogue)
            //        dialogue.SetNewStory(_cheatDialogue);
            //}
        }

        public void Invalidate()
        {
            Destroy(this);
        }
    }
}