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
        [SerializeField] private TMP_Text _textReference;
        [SerializeField, Min(0.1f)] private float _timeInScreen = 0.1f;
        [SerializeField, Min(0.1f)] private float _timeDissapear = 0.1f;
        private Queue<string> _textToAppear;
        private bool _isAppearing;

        public ISingleton<HUD_Alerts> Instance => this;
        public HUD_Alerts Value => this;


        public static void WriteText(string text)
        {
            Debug.Log(text);
            if (!ISingleton<HUD_Alerts>.TryGetInstance(out var options))
                return;

            if (options._textReference == null)
                return;

            if (!options._isAppearing)
                options.StartCoroutine(options.WriteTextCoroutine(text));

            else
                options._textToAppear.Enqueue(text);
        }

        public static void ResetText()
        {
            if (!ISingleton<HUD_Alerts>.TryGetInstance(out var options))
                return;

            if (options._textReference == null)
                return;
            options._textToAppear.Clear();
        }

        private void Awake()
        {
            Instance.Instantiate();
            Color color = _textReference.color;
            color.a = 0;
            _textReference.color = color;
            _textToAppear = new();
            _isAppearing = false;
        }
        public void Invalidate()
        {
            Destroy(gameObject);
        }

        private IEnumerator WriteTextCoroutine(string text)
        {
            _isAppearing = true;
            _textReference.text = text;
            Color textColor = _textReference.color;
            textColor.a = 1;
            _textReference.color = textColor;

            yield return new WaitForSeconds(_timeInScreen);

            for (float i = 0; i <= _timeDissapear; i += Time.deltaTime)
            {
                textColor.a = 1 - i / _timeDissapear;
                _textReference.color = textColor;

                yield return new WaitForSeconds(Time.deltaTime);
            }
            textColor.a = 0;
            _textReference.color = textColor;

            _isAppearing = false;
            //if (_textToAppear.Count <= 0)
            //else
            //{
            //    yield return new WaitForSeconds(1);
            //    string newText = _textToAppear.Dequeue();
            //    StartCoroutine(WriteTextCoroutine(newText));
            //}
        }  
    }
}