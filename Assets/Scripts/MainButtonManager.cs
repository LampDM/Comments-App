using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Examples.Demos;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VuforiaManager;

public class MainButtonManager : MonoBehaviour
    {
        [SerializeField]
        private DisableVuforia disabler = null;

    [SerializeField]
        private Interactable m_Button1;
        [SerializeField]
        private Interactable m_Button2;
        [SerializeField]
        private Interactable m_Button3;
        [SerializeField]
        private Interactable m_Button4;
        [SerializeField]
        private Interactable m_Button5;
        [SerializeField]
        private Interactable m_Button6;
        [SerializeField]
        private Interactable m_Button7;
        [SerializeField]
        private Interactable m_Button8;
        [SerializeField]
        private Interactable m_Button9;
        [SerializeField]
        private Interactable m_Button10;
        [SerializeField]
        private Interactable m_Button11;

    private void Start()
    {
        disabler.StopVuforia();
    }
    public void ButtonClicked(int id)
        {
            switch (id)
            {
                case 1:
                    break;
                case 2:
                    //Start Assembly Instruction WP2
                    break;

                case 3:
                    //Start Assembly Instruction WP3
                    break;

                case 4:
                    //Start Assembly Instruction WP4
                    break;

                case 5:
                    //Start Assembly Instruction WP5
                    break;

                case 6:
                    //View Model from WP1

                    break;

                case 7:
                    //View Model from WP2
                    break;

                case 8:
                    //View Model from WP3
                    break;

                case 9:
                    //View Model from WP4
                    break;

                case 10:
                    //View Model from WP5
                    break;

                case 11:
                    //View Scooter Model
                    break;
            }
        }

    }