using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming.UnityPackage.UI
{
    /// <summary>
    /// Loading overlay used for displaying a loading spinner. 
    /// </summary>
    public class LoadingOverlay : MonoBehaviour
    {
        [SerializeField] private RectTransform spinnerTransform;
        [SerializeField] private float spinSpeed = 1f;

        private void Update()
        {
            spinnerTransform.rotation *= Quaternion.AngleAxis(spinSpeed, Vector3.forward);
        }
    }
}
