using UnityEngine;
using UnityEngine.UI;

namespace UtilsComplements
{
    #region Report
    //Made by DarkAlejoxD

    //Current State: WIP
    //Last checked: March 2024
    //Last modification: March 2024

    //Commentaries:
    //  -   Rewrite to fit in your project
    #endregion

    public class HUD_Pointer : MonoBehaviour, ISingleton<HUD_Pointer>
    {
        public enum Icons_enum
        {
            DEFAULT, INSPECT, PUZZLE
        }

        private const Icons_enum DEFAULT_ICON = Icons_enum.DEFAULT;

        [Header("Icons references")]
        [SerializeField] private Image _defaultIcon;
        [SerializeField] private Image _inspectIcon;
        [SerializeField] private Image _puzzleIcon;
        private bool _blocked;

        [Header("Time delay")]
        private bool _active = false;

        public ISingleton<HUD_Pointer> Instance => this;
        public HUD_Pointer Value => this;

        public static void ShowIcon(Icons_enum iconType)
        {
            if (ISingleton<HUD_Pointer>.TryGetInstance(out var hud))
            {
                hud.ShowImage(iconType);
            }
        }

        private void Awake()
        {
            Instance.Instantiate();
            DisableAll();
            _blocked = false;
        }

        //private void OnEnable()
        //{
        //    GameManager.OnBlockInput += BlockIcon;
        //    GameManager.OnUnblockInput += UnBlock;
        //}

        //private void OnDisable()
        //{
        //    GameManager.OnBlockInput -= BlockIcon;
        //    GameManager.OnUnblockInput -= UnBlock;
        //}

        private void Update()
        {
            if (_blocked)
                return;

            if (_active)
                DisableAll();
        }

        private void OnDestroy()
        {
            Instance.RemoveInstance();
        }

        public static void ShowIcon()
        {
            ShowIcon(DEFAULT_ICON);
        }        

        public void Invalidate()
        {
            Destroy(gameObject);
        }

        public void ShowImage()
        {
            ShowImage(DEFAULT_ICON);
        }

        public void ShowImage(Icons_enum iconType)
        {
            _defaultIcon.enabled = false;
            switch (iconType)
            {
                case Icons_enum.INSPECT:
                    _inspectIcon.enabled = (true);
                    break;
                case Icons_enum.PUZZLE:
                    _puzzleIcon.enabled = (true);
                    break;
                default:
                    _defaultIcon.enabled = true;
                    break;
            }

            _active = true;
        }

        private void DisableAll()
        {
            _active = false;
            _inspectIcon.enabled = (false);
            _puzzleIcon.enabled = (false);
            _defaultIcon.enabled = true;
        }

        private void BlockIcon()
        {
            DisableAll();
            _defaultIcon.enabled = false;
            _blocked = true;
        }

        private void UnBlock()
        {
            DisableAll();
            _blocked = false;
        }
    }
}