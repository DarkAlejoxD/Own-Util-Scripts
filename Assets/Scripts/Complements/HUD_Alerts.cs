using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UtilsComplements
{
    #region Report
    //Made by DarkAlejoxD, Camilo Londoño

    //Current State: WIP
    //Last checked: March 2024
    //Last modification: March 2024

    //Direct Dependencies:
    //  -   TMPro Package

    //Commentaries:
    //  -   Va medio bug, arreglar
    //  -   Usado para Lideratge

    //TODO: 
    //  -   Arreglar
    //  -   Crear una pool de Textos y hacer que puedan aparecer varios seguidos.
    #endregion

    /// <summary>
    /// Admits showing Alerts in Screen if there is a HUD_Alerts in the map
    /// </summary>
    public class HUD_Alerts : MonoBehaviour, ISingleton<HUD_Alerts>
    {
        [Header("References")]
        [SerializeField] private RectTransform _container;
        [SerializeField] private GameObject _textPrefab;
        private Pool<TMP_Text> _textPool;

        [Header("Time In Screen")]
        [SerializeField, Min(0.1f)] private float _timeInScreen = 0.1f;
        [SerializeField, Min(0.1f)] private float _timeDissapear = 0.1f;

        public ISingleton<HUD_Alerts> Instance => this;
        public HUD_Alerts Value => this;

        #region Static Methods
        public static void WriteText(string text)
        {
            Debug.Log(text);

            if (!ISingleton<HUD_Alerts>.TryGetInstance(out var options))
                return;

            if (options._container == null)
                return;

            options.StartCoroutine(options.WriteTextCoroutine(text));
        }

        public static void ResetText()
        {
            if (!ISingleton<HUD_Alerts>.TryGetInstance(out var options))
                return;

            if (options._container == null)
                return;

        }
        #endregion

        #region UnityLogic
        private void Awake()
        {
            Instance.Instantiate();

            _textPool = new(_textPrefab, defaultObjects: 5,
                                         maxObjects: 5, 
                                         nameOfFolder: "TextPool", 
                                         catchObjectFromActiveList: true,
                                         parent: transform.parent);
        }
        #endregion

        #region Public Methods
        public void Invalidate()
        {
            Destroy(gameObject);
        }
        #endregion

        #region Private Methods
        private IEnumerator WriteTextCoroutine(TMP_Text textReference, string text)
        {            
            textReference.text = text;
            Color textColor = textReference.color;
            textColor.a = 1;
            textReference.color = textColor;

            yield return new WaitForSeconds(_timeInScreen);

            for (float i = 0; i <= _timeDissapear; i += Time.deltaTime)
            {
                textColor.a = 1 - i / _timeDissapear;
                textReference.color = textColor;

                yield return new WaitForSeconds(Time.deltaTime);
            }
            textColor.a = 0;
            textReference.color = textColor;
        }
        #endregion
    }
}