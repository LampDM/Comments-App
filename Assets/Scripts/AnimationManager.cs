using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField]
    private Transform model = null;
    
    [SerializeField]
    private TextAsset animationJsonFile = null;

    public Animator animator;
    private AnimationClip[] animationClips = null;
    List<string> states;
    public string currentState = "";
    private AnimationJson anim = null;

    public bool animationFinished = false;
    //Use Awake since it is first called and like a constructor. It is needed because ReadAnimationJson() must be called first 
    void Awake()
    {
        ReadAnimationJson();
        currentState = GetFirstStripName();
        animator = model.GetComponent<Animator>();
        /*
        RuntimeAnimatorController animatorController = animator.runtimeAnimatorController;
        animationClips = animatorController.animationClips;
        */
        states = GetStateNames(animationClips);
    }
    private void Start()
    {
        HideAllParts();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    #region AnimationEvents
    public void PlayPreviousAnimationClip()
    {
        //UpdateVisibleParts();
        JumpToPreviousState();
        Debug.Log(currentState);
        animator.Play(currentState, -1, 0f);
    }
    public void PlayCurrentAnimationClip()
    {
        if(animator.speed == 0f)
        {
            animator.speed = 1f;
        }
        Debug.Log(currentState);
        animator.Play(currentState, -1, 0f);
        
    }
    public void PauseAnimationClip()
    {
        if (animator.speed == 1f)
        {
            Debug.Log("Pause Animation");
            animator.speed = 0f;
        }
        else if (animator.speed == 0f)
        {
            Debug.Log("Play Animation");
            animator.speed = 1f;
        }

    }
    
    public void PlayNextAnimationClip()
    {
        
        JumpToNextState();
       
        Debug.Log(currentState);
        //UpdateVisibleParts();
        
        animator.Play(currentState, -1, 0f);
        
    }
    #endregion

    #region AnimationHelpers

    private void HideAllParts()
    {
        Transform[] childarray = model.GetComponentsInChildren<Transform>();
        foreach (Transform part in childarray)
        {
            if (part.GetComponent<MeshRenderer>() == null)
            {
                Debug.Log("Part without mesh renderer:" + part.name);
                continue;
            }

            Debug.Log("Part with Mesh Renderer:" + part.name);
            part.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void UpdateVisibleParts()
    {
        Transform[] childarray = model.GetComponentsInChildren<Transform>();
        int stepSize = 1;
        HideAllParts();
        for (int i = 0; i < anim.AnimationClips.Length + 1; i++)
        {
            //If last clip then display all
            if (currentState == GetLastStripName())
            {
                foreach (Transform part in childarray)
                {
                    if (part.GetComponent<MeshRenderer>() != null && part.name != "MountingFixture")
                    {
                        part.GetComponent<MeshRenderer>().enabled = true;
                    }

                }
            }
            else
            {
                if (anim.AnimationClips[i].clipname == states[states.IndexOf(currentState) + stepSize])
                {
                    break;
                }
                else
                {
                    foreach (Transform part in childarray)
                    {
                        if (anim.AnimationClips[i].animated_parts.Contains(part.name) && part.GetComponent<MeshRenderer>() != null)
                        {
                            Debug.Log("UNHIDE:" + part.name);
                            part.GetComponent<MeshRenderer>().enabled = true;
                        }
                    }
                }
            }

        }
    }

    private void JumpToNextState()
    {
        for (int i = 0; i < states.Count; i++)
        {

            if (currentState == states[i] && currentState != GetLastStripName())
            {
                currentState = states[i + 1];
                break;
            }
            else if (currentState == GetLastStripName())
            {
                Debug.Log("Last State of Animation!\n");
            }
        }
    }

    private void JumpToPreviousState()
    {
        for (int i = 0; i < states.Count; i++)
        {
            if (currentState == states[i] && currentState != GetFirstStripName())
            {
                currentState = states[i - 1];
            }
            else if (currentState == GetFirstStripName())
            {
                Debug.Log("First State of Animation!\nThere is no previous State!");
                break;
            }
        }
    }

    private void ReadAnimationJson()
    {
        string data = animationJsonFile.ToString();
        anim = JsonUtility.FromJson<AnimationJson>(data);
    }
    #endregion

    #region Getter-Methods
    public string GetCurrentStateName()
    {
        return currentState;
    }

    public string GetCurrentInstructionText()
    {
        for (int i = 0; i < anim.AnimationClips.Length + 1; i++)
        {
            if (anim.AnimationClips[i].clipname == currentState)
            {
                Debug.Log(anim.AnimationClips[i].instruction);
                return anim.AnimationClips[i].instruction;
            }

        }
        return "";
    }

    public string GetLastStripName()
    {
        return anim.AnimationClips.Last().clipname;
    }

    private string GetFirstStripName()
    {
        Debug.Log(anim.AnimationClips.First().clipname);
        return anim.AnimationClips.First().clipname;
    }

    private List<string> GetStateNames(AnimationClip[] animationClips)
    {
        states = new List<string>();
        Debug.Log(anim.AnimationClips.Length);
        for (int i = 0; i < anim.AnimationClips.Length; i++)
        {
            states.Add(anim.AnimationClips[i].clipname);
        }
        return states;
    }
    #endregion

}

