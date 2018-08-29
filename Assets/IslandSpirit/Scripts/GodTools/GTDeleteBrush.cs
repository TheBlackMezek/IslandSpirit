using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GTDeleteBrush : GodToolAbstract {

    public override void OnMouseDown(int button, TerrainHitData data, float dt, float toolRadius, GameObject placablePrefab)
    {
        int centerX = (int)data.terrainHitPos.x;
        int centerY = (int)data.terrainHitPos.y;
        int radiusInt = (int)Mathf.Ceil(toolRadius);
        int diameter = radiusInt * 2 + 1;
        int lenX = diameter;
        int lenY = diameter;

        int gridX = centerX - radiusInt;
        if (gridX < 0)
        {
            lenX += gridX;
            gridX = 0;
        }
        if (gridX + lenX >= data.terrain.terrainData.heightmapWidth)
        {
            lenX = data.terrain.terrainData.heightmapWidth - gridX - 1;
        }
        int gridY = centerY - radiusInt;
        if (gridY < 0)
        {
            lenY += gridY;
            gridY = 0;
        }
        if (gridY + lenY >= data.terrain.terrainData.heightmapHeight)
        {
            lenY = data.terrain.terrainData.heightmapHeight - gridY - 1;
        }

        Vector2 loopCenter = new Vector2(centerX, centerY);



        float[,] heights = data.terrain.terrainData.GetHeights(gridX, gridY, lenX, lenY);

        Vector2 loopPos;
        for (int x = gridX; x < gridX + lenX; ++x)
        {
            loopPos.x = x;
            for (int y = gridY; y < gridY + lenY; ++y)
            {
                loopPos.y = y;
                float dist = Vector2.Distance(loopPos, loopCenter);
                if (dist <= toolRadius)
                {
                    gc.ClearPlacedObjects(x, y);
                }
            }
        }
    }

}
