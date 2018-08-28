using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TerrainHitData {

    public Terrain terrain;
    public Vector3 physicalHitPoint;
    public Vector2 floorHitPos;
    public float heightAtFloorPos;

}
