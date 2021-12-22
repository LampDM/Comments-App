using Microsoft.MixedReality.Toolkit.SceneSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Extensions.SceneTransitions;
using Microsoft.MixedReality.Toolkit;

namespace Loading
{
    public class SceneLoaderHelper : MonoBehaviour
    {
        public void LoadContent(SceneInfo contentScene, LoadSceneMode loadSceneMode)
        {
            ISceneTransitionService transitions = MixedRealityToolkit.Instance.GetService<ISceneTransitionService>();
            if (transitions.TransitionInProgress)
            {
                return;
            }
            transitions.DoSceneTransition(() => CoreServices.SceneSystem.LoadContent(contentScene.Name, loadSceneMode));
        }
    }
}

