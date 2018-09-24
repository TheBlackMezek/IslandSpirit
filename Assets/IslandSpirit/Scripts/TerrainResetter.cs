using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainResetter : MonoBehaviour {

    public float baseHeight;

    [SerializeField]
    [HideInInspector]
    private Terrain terrain;



    private void OnValidate()
    {
        terrain = GetComponent<Terrain>();
    }

    private void Awake()
    {
        ResetHeights();
        ResetAlphamap();
    }

    private void OnApplicationQuit()
    {
        ResetHeights();
        ResetAlphamap();
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

    private void ResetAlphamap()
    {
        float[,,] splats = terrain.terrainData.GetAlphamaps(0, 0, terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight);
        for (int x = 0; x < terrain.terrainData.alphamapWidth; ++x)
        {
            for (int y = 0; y < terrain.terrainData.alphamapHeight; ++y)
            {
                for(int i = 0; i < terrain.terrainData.alphamapLayers; ++i)
                {
                    if(i == 0)
                    {
                        splats[x, y, i] = 1;
                    }
                    else
                    {
                        splats[x, y, i] = 0;
                    }
                }
            }
        }
        terrain.terrainData.SetAlphamaps(0, 0, splats);
    }

}
