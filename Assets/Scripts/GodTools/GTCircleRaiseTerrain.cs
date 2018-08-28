using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GTCircleRaiseTerrain : GodToolAbstract {
    
    public float speed;



    public override void OnMouseHeld(int button, TerrainHitData data, float dt, float toolRadius)
    {
        if(button == 0)
        {
            RaiseTerrainCircleLerpBrush(data, dt, toolRadius);
        }
    }

    private void RaiseTerrainCircleLerpBrush(TerrainHitData data, float dt, float radius)
    {
        int centerX = (int)data.floorHitPos.x;
        int centerY = (int)data.floorHitPos.y;
        int radiusInt = (int)Mathf.Ceil(radius);
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
                if (dist <= radius)
                {
                    heights[y - gridY, x - gridX] += Mathf.Lerp(0, speed * dt, Mathf.InverseLerp(radius, 0, dist));
                }
            }
        }

        data.terrain.terrainData.SetHeights(gridX, gridY, heights);
    }

}
