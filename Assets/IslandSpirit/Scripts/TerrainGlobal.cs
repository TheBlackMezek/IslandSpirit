using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGlobal : MonoBehaviour {

	public static Terrain terrain { get; private set; }

    private void Awake()
    {
        terrain = GetComponent<Terrain>();
    }

}
