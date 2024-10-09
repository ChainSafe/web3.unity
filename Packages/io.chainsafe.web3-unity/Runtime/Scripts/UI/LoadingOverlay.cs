using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming.UnityPackage.UI
{
    /// <summary>
    /// Loading overlay used for displaying a loading spinner. 
    /// </summary>
    public class LoadingOverlay : MonoBehaviour
    {
        [SerializeField] private TMP_Text loadingText;
        [SerializeField] private RectTransform spinnerTransform;
        [SerializeField] private float spinSpeed = 1f;

        private static LoadingOverlay _instance;
        public static LoadingOverlay Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                _instance = Instantiate(Resources.Load<LoadingOverlay>("LoadingOverlay"));
                DontDestroyOnLoad(_instance.gameObject);
                return _instance;
            }

        }

        public static void ShowLoadingOverlay(string loadingText = "")
        {
            Instance.loadingText.text = loadingText;
            Instance.gameObject.SetActive(true);
        }

        public static void HideLoadingOverlay()
        {
            Instance.gameObject.SetActive(false);
        }

        private void Update()
        {
            spinnerTransform.rotation *= Quaternion.AngleAxis(spinSpeed, Vector3.forward);
        }
    }
}
