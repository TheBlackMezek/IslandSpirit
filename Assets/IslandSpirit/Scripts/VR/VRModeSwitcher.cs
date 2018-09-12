using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRModeSwitcher : MonoBehaviour {

    [SerializeField]
    private Transform cameraRig;

    [SerializeField]
    private GameObject[] godModeObjects;
    [SerializeField]
    private GameObject[] avatarModeObjects;
    [SerializeField]
    private VRToolUser[] toolUserScripts;
    [SerializeField]
    private VRTK.VRTK_StraightPointerRenderer[] pointerRenderers;

    [SerializeField]
    private bool startInGodMode = true;
    [SerializeField]
    private float godModeScale;
    [SerializeField]
    private float avatarModeScale = 1f;

    [SerializeField]
    private float godModePointerLineSize;
    [SerializeField]
    private float avatarModePointerLineSize;
    [SerializeField]
    private VRTK.VRTK_CustomRaycast godModePointerRaycast;
    [SerializeField]
    private VRTK.VRTK_CustomRaycast avatarModePointerRaycast;



    private void Awake()
    {
        if(startInGodMode)
        {
            SwitchToGodMode();
        }
        else
        {
            SwitchToAvatarMode();
        }
    }

    public void SwitchToGodMode()
    {
        for(int i = 0; i < avatarModeObjects.Length; ++i)
        {
            avatarModeObjects[i].SetActive(false);
        }
        for (int i = 0; i < godModeObjects.Length; ++i)
        {
            godModeObjects[i].SetActive(true);
        }
        for(int i = 0; i < toolUserScripts.Length; ++i)
        {
            toolUserScripts[i].enabled = true;
            toolUserScripts[i].Activate();
        }
        for (int i = 0; i < pointerRenderers.Length; ++i)
        {
            pointerRenderers[i].customRaycast = godModePointerRaycast;
            pointerRenderers[i].scaleFactor = godModePointerLineSize;
        }

        cameraRig.localScale = Vector3.one * godModeScale;
    }

    public void SwitchToAvatarMode()
    {
        for (int i = 0; i < godModeObjects.Length; ++i)
        {
            godModeObjects[i].SetActive(false);
        }
        for (int i = 0; i < avatarModeObjects.Length; ++i)
        {
            avatarModeObjects[i].SetActive(true);
        }
        for (int i = 0; i < toolUserScripts.Length; ++i)
        {
            toolUserScripts[i].Deactivate();
            toolUserScripts[i].enabled = false;
        }
        for (int i = 0; i < pointerRenderers.Length; ++i)
        {
            pointerRenderers[i].customRaycast = avatarModePointerRaycast;
            pointerRenderers[i].scaleFactor = avatarModePointerLineSize;
        }

        cameraRig.localScale = Vector3.one * avatarModeScale;
    }

}
