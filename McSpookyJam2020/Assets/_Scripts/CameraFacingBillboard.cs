﻿using System;
using UnityEngine;
using System.Collections;
 
public class CameraFacingBillboard : MonoBehaviour
{
    private Camera mainCamera = null;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
            mainCamera.transform.rotation * Vector3.up);
    }
}