using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using Microsoft.MixedReality.Toolkit.UI;
using VuforiaManager;
using Loading;
using Vuforia;
using TMPro;
using Microsoft.MixedReality.Toolkit.Extensions.SceneTransitions;

public class ModelButtonManager : MonoBehaviour
{
    public Interactable m_button1;
    [SerializeField]
    private EnableVuforia enabler = null;
    [SerializeField]
    private DisableVuforia disabler = null;

    [SerializeField]
    private LoadContentScene content = null;

    private bool vuforiaEnabled = true;
    public void ButtonClicked(int id)
    {
        GameObject vuforiaState = GameObject.Find("UI-Model/VuforiaToggle/Text");
        switch (id)
        {
            case 1:

                break;
        }
    }
}