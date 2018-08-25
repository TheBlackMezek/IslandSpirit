using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainResetter : MonoBehaviour {

    public float baseHeight;

    private Terrain terrain;



    private void OnValidate()
    {
        terrain = GetComponent<Terrain>();
    }

    private void Awake()
    {
        ResetHeights();
    }

    private void OnApplicationQuit()
    {
        ResetHeights();
    }

    private void ResetHeights()
    {
        float[,] heights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight);
        for (int x = 0; x < terrain.terrainData.heightmapWidth; ++x)
        {
            for (int y = 0; y < terrain.terrainData.heightmapHeight; ++y)
            {
                heights[x, y] = baseHeight;
            }
        }
        terrain.terrainData.SetHeights(0, 0, heights);
    }

}
