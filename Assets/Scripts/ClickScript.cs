using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

public class DetectTapExample : MonoBehaviour
{

    public void Start()
    {
        PointerHandler pointerHandler = gameObject.AddComponent<PointerHandler>();
        pointerHandler.OnPointerClicked.AddListener((evt) => Debug.Log("Tap Detected " + Time.time));
        // Make this a global input handler, otherwise this object will only receive events when it has input focus
        CoreServices.InputSystem.RegisterHandler<IMixedRealityPointerHandler>(pointerHandler);
    }
}