using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSwitcher : MonoBehaviour {

    public GameObject godCam;
    public GameObject avatar;

    private Terrain terrain;



    private void OnValidate()
    {
        terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
    }

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

            if(avatar.activeInHierarchy)
            {
                avatar.transform.position = new Vector3(avatar.transform.position.x,
                                                        terrain.SampleHeight(avatar.transform.position),
                                                        avatar.transform.position.z);
            }
        }
    }

}
