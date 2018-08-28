using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GTCircleLowerTerrain : GodToolAbstract
{
    public float speed;

    public float particleToRadiusRatio;
    public float particleSizeToRadiusRatio;
    public float particleMinSize;
    public float particleMaxSize;



    public override void OnMouseHeld(int button, TerrainHitData data, float dt, float toolRadius, GameObject placablePrefab)
    {
        if (button == 0)
        {
            LowerTerrainCircleLerpBrush(data, dt, toolRadius);
            if (!particleSystem.isPlaying)
            {
                particleSystem.Play();
            }
        }
    }

    public override void OnMouseUp(int button, TerrainHitData data, float dt, float toolRadius, GameObject placablePrefab)
    {
        if (button == 0)
        {
            particleSystem.Stop();
            particleSystem.Clear();
        }
    }

    public override void OnBrushRadiusChange(float newRadius)
    {
        ParticleSystem.ShapeModule shape = particleSystem.shape;
        shape.radius = newRadius;
        ParticleSystem.EmissionModule em = particleSystem.emission;
        em.rateOverTime = newRadius * particleToRadiusRatio;
        ParticleSystem.MainModule main = particleSystem.main;
        main.startSize = Mathf.Clamp(newRadius * particleSizeToRadiusRatio, particleMinSize, particleMaxSize);
    }

    private void LowerTerrainCircleLerpBrush(TerrainHitData data, float dt, float radius)
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
                    heights[y - gridY, x - gridX] -= Mathf.Lerp(0, speed * dt, Mathf.InverseLerp(radius, 0, dist));
                }
            }
        }

        data.terrain.terrainData.SetHeights(gridX, gridY, heights);

        gc.UpdatePlacedObjectPositions(gridX, gridY, lenX, lenY);
    }

}
