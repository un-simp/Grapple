using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class XRManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DisableXR();
            
    }

    public void EnableXR()
    {
        XRSettings.enabled = true;
    }

    public void DisableXR()
    {
        XRSettings.enabled = false;
    }
}
