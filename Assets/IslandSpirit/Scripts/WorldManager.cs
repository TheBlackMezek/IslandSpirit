using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {

    public static WorldManager Instance { get; private set; }

    private List<Transform>[,] placedObjects;
    private Terrain terrain;



    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        terrain = TerrainGlobal.terrain;

        placedObjects = new List<Transform>[terrain.terrainData.heightmapWidth,
                                            terrain.terrainData.heightmapHeight];
    }

    public Vector2 AddPlacedObject(GameObject obj)
    {
        int terrainPosX = (int)(
            ((obj.transform.position.x - terrain.transform.position.x)
            / terrain.terrainData.size.x) * terrain.terrainData.heightmapWidth);
        int terrainPosY = (int)(
            ((obj.transform.position.z - terrain.transform.position.z)
            / terrain.terrainData.size.z) * terrain.terrainData.heightmapHeight);

        if (terrainPosX < 0 || terrainPosX >= terrain.terrainData.heightmapWidth
        || terrainPosY < 0 || terrainPosY >= terrain.terrainData.heightmapHeight)
        {
            Destroy(obj);
        }
        else
        {
            if (placedObjects[terrainPosX, terrainPosY] == null)
            {
                placedObjects[terrainPosX, terrainPosY] = new List<Transform>();
            }
            placedObjects[terrainPosX, terrainPosY].Add(obj.transform);
        }

        return new Vector2(terrainPosX, terrainPosY);
    }

    public void DeletePlacedObject(GameObject obj)
    {
        int terrainPosX = (int)(
            ((obj.transform.position.x - terrain.transform.position.x)
            / terrain.terrainData.size.x) * terrain.terrainData.heightmapWidth);
        int terrainPosY = (int)(
            ((obj.transform.position.z - terrain.transform.position.z)
            / terrain.terrainData.size.z) * terrain.terrainData.heightmapHeight);

        placedObjects[terrainPosX, terrainPosY].Remove(obj.transform);
        if (placedObjects[terrainPosX, terrainPosY].Count == 0)
        {
            placedObjects[terrainPosX, terrainPosY] = null;
        }
        Destroy(obj);
    }

    public void ClearPlacedObjects(int terrainX, int terrainY)
    {
        if (placedObjects[terrainX, terrainY] != null)
        {
            int count = placedObjects[terrainX, terrainY].Count;
            for (int i = 0; i < count; ++i)
            {
                Destroy(placedObjects[terrainX, terrainY][i].gameObject);
            }
            placedObjects[terrainX, terrainY] = null;
        }
    }

    public void UpdatePlacedObjectPositions(int startX, int startY, int lenX, int lenY)
    {
        Transform t;
        for (int x = startX; x < startX + lenX; ++x)
        {
            for (int y = startY; y < startY + lenY; ++y)
            {
                if (placedObjects[x, y] != null)
                {
                    int count = placedObjects[x, y].Count;
                    for (int i = 0; i < count; ++i)
                    {
                        t = placedObjects[x, y][i];
                        t.position = new Vector3(t.position.x, terrain.SampleHeight(t.position), t.position.z);
                    }
                }
            }
        }
    }

    public Vector2 UpdadePlacedObjectLocation(GameObject obj, int oldX, int oldY)
    {
        placedObjects[oldX, oldY].Remove(obj.transform);
        if (placedObjects[oldX, oldY].Count == 0)
        {
            placedObjects[oldX, oldY] = null;
        }

        return AddPlacedObject(obj);
    }

}
