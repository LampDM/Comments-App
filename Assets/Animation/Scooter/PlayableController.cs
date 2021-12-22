using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEditor;
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
using System;
using System.Threading;

#if UNITY_EDITOR
[CustomEditor(typeof(PlayableController))]
[CanEditMultipleObjects]
public class PlayableControllerEditor : Editor
{
    PlayableController script;
	
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        script = (PlayableController)target;

        GUILayout.BeginHorizontal();

        if (script.IsPlaying())
        {
            if (GUILayout.Button("Pause ||"))
            {
                script.PlayPause();
            }
        }
        else
        {
            if (GUILayout.Button("Play >"))
            {
                script.PlayPause();
            }
        }
        
        if (GUILayout.Button("Stop []"))
        {
            script.Stop();
        }

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Backwards <<"))
        {
            script.JumpBackward();
        }
        if (GUILayout.Button("Forward >>"))
        {
            script.JumpForward();
        }

        GUILayout.EndHorizontal();
    }
}
#endif

public enum PlayMode { PlayPause, PlayAll}

[System.Serializable]
public class HiddenObject
{
    public GameObject[] obj;
    public float visibleFrom = 0;
}

[ExecuteInEditMode]
public class PlayableController : MonoBehaviour
{	
	//Buttons that connect to the old system
	[SerializeField]
	private Interactable testButton1;
	[SerializeField]
	private Interactable testButton2;
	
	Int32 lastTime = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
	
    [SerializeField] private PlayableDirector director;
    [SerializeField] private PlayMode playMode;
    [SerializeField] private float animationSpeed = 1f;

    [Space]
    [SerializeField] private int framePerAnim = 30;
    [SerializeField] private int fps = 60;

    [Space]
    [SerializeField] private bool shouldStartAtZero = true;
    [SerializeField] private double timeNow;
    [SerializeField] private int totalAnimations = 50;

    [Space]
    [SerializeField] private HiddenObject[] hideObjects;
    
    private bool autoPausing = false;
    private float next;
    private bool isPlayingNow = false;


	private Int32 currentTime(){
		return (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
	}

    private void Awake()
    {
		testButton1 = GameObject.Find("ButtonTest1").GetComponent<Interactable>();
		testButton2 = GameObject.Find("ButtonTest2").GetComponent<Interactable>();
		
        if (!director)
        {
            director = GetComponent<PlayableDirector>();
        }

        if (!director) { return; }

        if (shouldStartAtZero)
        {
            director.Play();
            Stop();
        }
    }
    private void Update()
    {
        if (!director) { return; }
        timeNow = director.time;

        if (autoPausing)
        {
            if (timeNow >= next)
            {
                director.time = next;
                autoPausing = false;
                GoPause();
            }
        }

        UpdateVisibleObjects((float)director.time);
    }

    private void UpdateVisibleObjects(float time)
    {
        foreach(HiddenObject o in hideObjects)
        {
            foreach(GameObject g in o.obj)
            {
                if (time >= o.visibleFrom)
                {
                    g.SetActive(true);
                }
                else
                {
                    g.SetActive(false);
                }
            }
        }
    }

    // Is playing check
    public bool IsPlaying()
    {
        if (!director) { return false; }

        return isPlayingNow;
    }

    public void PlayPause()
    {
        if (!director) { return; }

		//Button can only be clicked every 3 seconds
		if((currentTime()-lastTime)>4){
			if (isPlayingNow)
			{
				GoPause();
			}
			else
			{
				GoPlay();
			}
			lastTime = currentTime();
		}
		

    }
    private void GoPause()
    {
        SetSpeed(0);
        //director.Pause();
        director.Evaluate();//*/
        isPlayingNow = false;
    }
    private void GoPlay()
    {
        SetSpeed(animationSpeed);
        switch (playMode)
        {
            case PlayMode.PlayAll:
                director.Play();
                break;
            case PlayMode.PlayPause:
                director.time = GetPrevius((float)director.time);
                ExecuteStep();
                break;
        }

        isPlayingNow = true;
        director.Evaluate();
    }

    private void ShouldPause()
    {
        next = GetNext((float)director.time);
        autoPausing = true;
    }

    public void Stop()
    {
        director.time = 0;
        SetSpeed(0);
        director.Evaluate();
    }
    public void JumpForward()
    {
		
        if (!director) { return; }

		//Button can only be clicked every 3 seconds
		if((currentTime()-lastTime)>3){
			ExecuteStep();
			testButton1.TriggerOnClick();
			lastTime = currentTime();
		}
    }
    public void JumpBackward()
    {
		
        if (!director) { return; }
		
		//Button can only be clicked every 3 seconds
		if((currentTime()-lastTime)>3){
			director.time = GetPreviusTwice((float)director.time);
			ExecuteStep();
			testButton2.TriggerOnClick();
			lastTime = currentTime();
		}
    }
    public void ExecuteStep()
    {
        SetSpeed(animationSpeed);
        director.Play();
        ShouldPause();
        isPlayingNow = true;
        director.Evaluate();
    }
    public void SetSpeed(float speed)
    {
        director.RebuildGraph();
        director.playableGraph.GetRootPlayable(0).SetSpeed(speed);
    }
    public float GetNext(float value)
    {
        float interval = (float)framePerAnim / (float)fps;

        for (int i = 0; i < totalAnimations; i++)
        {
            if (interval * i > value)
            {
                interval *= i;
                interval = Mathf.Repeat(interval, (float)director.duration);
                return interval;
            }
        }

        return 0;
    }
    public float GetPrevius(float value)
    {
        float interval = (float)framePerAnim / (float)fps;
        float finalPrev = GetNext(value) - interval;

        if (finalPrev == timeNow) { finalPrev -= interval; }

        finalPrev = Mathf.Repeat(finalPrev, (float)director.duration);
        return finalPrev;
    }
    public float GetPreviusTwice(float value)
    {
        float interval = (float)framePerAnim / (float)fps;
        float finalPrev = GetNext(value) - interval;

        if (finalPrev == timeNow) { finalPrev -= interval; }

        finalPrev -= interval;
        finalPrev = Mathf.Clamp(finalPrev, 0, (float)director.duration);
        return finalPrev;
    }
}
