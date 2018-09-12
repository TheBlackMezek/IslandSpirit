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
    private VRTK.VRTK_Pointer[] pointers;

    [SerializeField]
    private bool startInGodMode = true;
    [SerializeField]
    private float godModeScale;
    [SerializeField]
    private float avatarModeScale = 1f;



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
        }
        for (int i = 0; i < pointers.Length; ++i)
        {
            pointers[i].enabled = true;
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
            toolUserScripts[i].enabled = false;
        }
        for (int i = 0; i < pointers.Length; ++i)
        {
            pointers[i].enabled = false;
        }

        cameraRig.localScale = Vector3.one * avatarModeScale;
    }

}
