using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] menus;

    private int current;



    private void Awake()
    {
        menus[0].SetActive(true);
        current = 0;
        for(int i = 1; i < menus.Length; ++i)
        {
            menus[i].SetActive(false);
        }
    }

    public void SetMenu(int idx)
    {
        menus[current].SetActive(false);

        current = idx;
        menus[current].SetActive(true);
    }

}
