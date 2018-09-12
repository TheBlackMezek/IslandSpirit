﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DisableVR : MonoBehaviour {

    private void Awake()
    {
        XRSettings.enabled = false;
    }

}