namespace UtilsComplements
{
    #region Report
    //Last checked: February 2024
    //Last modification: February 2024

    //Direct dependencies of classes if imported file by file:
    //  -   Cheat.cs
    //  -   ISingleton<T>.cs

    //Commentaries:
    //  -   Cheat class implementation
    #endregion

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
            //Put here the command to go to your menu
            //MenuManager.GoToMainMenu();
        }
    }
}