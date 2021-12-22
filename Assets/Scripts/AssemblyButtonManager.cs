using Microsoft.MixedReality.Toolkit.SceneSystem;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loading;
using UnityEngine.SceneManagement;
using VuforiaManager;
using TMPro;
using Microsoft.MixedReality.Toolkit.Extensions.SceneTransitions;

public class AssemblyButtonManager : MonoBehaviour
{
    [SerializeField]
    private Interactable m_ButtonResetTracking;
    [SerializeField]
    private Interactable m_ButtonPrevious;
    [SerializeField]
    private Interactable m_ButtonPlay;
    [SerializeField]
    private Interactable m_ButtonNext;
    [SerializeField]
    private Interactable m_ButtonHome;

    [SerializeField]
    private Material animationFinishedMaterial = null;
    [SerializeField]
    private LoadContentScene loadingScene = null;

    [SerializeField]
    private LoadContentScene mainScene = null;

    [SerializeField]
    private AnimationManager animationManager = null;

    private void Start()
    {
        animationManager.UpdateVisibleParts();
        ChangeStateNameInUI();
        UpdateInstructionsInUI();
    }
	
    public void ButtonClicked(int id)
    {
		//Check if animation is being played

        switch (id) 
        {
            case 1:
                //Debug.Log("---------------------Load previous animation step---------------------");
				animationManager.animationFinished = false;
                animationManager.PlayPreviousAnimationClip();
                animationManager.UpdateVisibleParts();
                UpdateInstructionsInUI();
                ChangeStateNameInUI();
                break;

            case 2:
                //Debug.Log("---------------------Play animation---------------------");
                animationManager.PlayCurrentAnimationClip();
                break;

            case 3:
                //Debug.Log("---------------------Loading next animation step---------------------");
                
				//Go back to main menu if we are in last state
                if (animationManager.animationFinished)
                {
                    mainScene.LoadContent();
                    animationManager.animationFinished = false;
                    break;
                }
                
                animationManager.PlayNextAnimationClip();
                animationManager.UpdateVisibleParts();
                UpdateInstructionsInUI();
                ChangeStateNameInUI();
                if (animationManager.currentState == animationManager.GetLastStripName())
                {
                        animationManager.animationFinished = true;
                        UpdateNextButtonIcon();
                }
                
                break;

            case 4:
                //Debug.Log("Reset Tracking...");
                loadingScene.LoadContent();
                break;

            case 5:
                //Debug.Log("Loading Start Menu...");
                break;

            
        }
    }

    private void ChangeButtonName(string bt_name, Interactable button)
    {
        button.transform.Find("Text").GetComponent<TextMeshPro>().text = bt_name;
    }

    private void ChangeStateNameInUI()
    {
        //ToDo: Get Current State Name
        string stateName = animationManager.GetCurrentStateName();
        GameObject animationState = GameObject.Find("ImageTarget/UI-Assembly/AnimationState");
        animationState.GetComponent<TextMeshPro>().text = stateName;
    }


    private void UpdateInstructionsInUI()
    {
        //ToDo: Get Instructions from json 
        string instructionText = animationManager.GetCurrentInstructionText();
        GameObject instructions = GameObject.Find("ImageTarget/UI-Assembly/Instructions");
        instructions.GetComponent<TextMeshPro>().text = instructionText;

    }

    private void UpdateNextButtonIcon()
    {
        string newButtonText = "Finish";
        GameObject ButtonText = GameObject.Find("ImageTarget/UI-Assembly/ButtonNext/Text");
        ButtonText.GetComponent<TextMeshPro>().text = newButtonText;
        GameObject buttonIcon = GameObject.Find("ImageTarget/UI-Assembly/ButtonNext/UIButtonSquareIcon");
        buttonIcon.GetComponent<Renderer>().material = animationFinishedMaterial;
    }
}
