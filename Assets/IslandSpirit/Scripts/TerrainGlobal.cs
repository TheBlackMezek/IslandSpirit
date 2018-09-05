using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGlobal : MonoBehaviour {

	public static Terrain terrain { get; private set; }

    private void Awake()
    {
        terrain = GetComponent<Terrain>();
    }



    public static Vector2 WorldToTerrainPos(Vector3 pos)
    {
        int terrainPosX = (int)(
            ((pos.x - terrain.transform.position.x)
            / terrain.terrainData.size.x) * terrain.terrainData.heightmapWidth);
        int terrainPosY = (int)(
            ((pos.z - terrain.transform.position.z)
            / terrain.terrainData.size.z) * terrain.terrainData.heightmapHeight);

        return new Vector2(terrainPosX, terrainPosY);
    }

}
