using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSwitcher : MonoBehaviour {

    public GameObject godCam;
    public GameObject avatar;



    private void Awake()
    {
        godCam.SetActive(true);
        avatar.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            godCam.SetActive(!godCam.activeInHierarchy);
            avatar.SetActive(!avatar.activeInHierarchy);
        }
    }

}
