using Microsoft.MixedReality.Toolkit.Extensions.SceneTransitions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsynchronSceneLoader : MonoBehaviour
{
    [SerializeField]
    private float sceneLoadDelay = 2.0F;
    [SerializeField]
    private LoadContentScene content = null;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadNextSceneAfter(sceneLoadDelay));
    }

    private IEnumerator LoadNextSceneAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        content.LoadContent();
    }
}
