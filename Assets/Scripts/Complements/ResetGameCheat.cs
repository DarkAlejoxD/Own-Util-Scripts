﻿using UtilsComplements;

namespace UtilsComplements
{
    public class ResetGameCheat : Cheat, ISingleton<ResetGameCheat>
    {
        private const string MENU_CHEAT = "GOTOMENU";

        public ISingleton<ResetGameCheat> Instance => this;

        public ResetGameCheat Value => this;

        protected override string CHEAT_NAME => MENU_CHEAT;

        protected override void Awake()
        {
            base.Awake();
            Instance.Instantiate();
        }

        private void OnDestroy()
        {
            Instance.RemoveInstance();
        }

        public void Invalidate()
        {
            Destroy(this);
        }

        protected override void OnCheat()
        {
            //MenuManager.GoToMainMenu();
        }
    }
}