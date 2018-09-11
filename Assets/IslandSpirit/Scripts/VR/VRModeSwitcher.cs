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

        cameraRig.localScale = Vector3.one * avatarModeScale;
    }

}
