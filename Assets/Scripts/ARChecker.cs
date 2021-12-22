using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ARChecker : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GameObject mainCamera = this.gameObject;
        if (mainCamera.GetComponent<VuforiaBehaviour>().enabled == true)
        {
            mainCamera.GetComponent<VuforiaBehaviour>().enabled = false;
            //mainCamera.GetComponent<VuforiaBehaviour>().enabled = false;
        }
        else
        {
            Debug.Log("NO SECOND AR CAMERA");
        }
    }

    // Update is called once per frame
    void Update()
    {
     
  
    }
}
