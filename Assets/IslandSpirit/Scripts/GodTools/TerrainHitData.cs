using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TerrainHitData {

    public Terrain terrain;
    public Vector3 physicalHitPoint;
    public Vector3 floorHitPos;
    public Vector3 floorHitPlusTHeight;
    public Vector2 terrainHitPos;
    public float heightAtFloorPos;

}
