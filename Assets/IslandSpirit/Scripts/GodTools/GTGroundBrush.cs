using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GTGroundBrush : GodToolAbstract {

    [Range(0f, 1f), Tooltip("The percentage of the radius out from center in which tex alpha will be set to 1")]
    public float solidTexRadPercent;

    private int texture = 1;

    private float[] alphas;



    public override void OnMouseHeld(int button, TerrainHitData data, float dt, float toolRadius, GameObject placablePrefab)
    {
        if(button != 0)
        {
            return;
        }


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
        if (gridX + lenX >= data.terrain.terrainData.alphamapWidth)
        {
            lenX = data.terrain.terrainData.alphamapWidth - gridX - 1;
        }
        int gridY = centerY - radiusInt;
        if (gridY < 0)
        {
            lenY += gridY;
            gridY = 0;
        }
        if (gridY + lenY >= data.terrain.terrainData.alphamapHeight)
        {
            lenY = data.terrain.terrainData.alphamapHeight - gridY - 1;
        }

        Vector2 loopCenter = new Vector2(centerX, centerY);



        float[,,] splats = data.terrain.terrainData.GetAlphamaps(gridX, gridY, lenX, lenY);
        int layers = data.terrain.terrainData.alphamapLayers;

        if (alphas == null || alphas.Length != layers)
        {
            alphas = new float[layers];
        }

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
                    float texAlpha = splats[y - gridY, x - gridX, texture];
                    float leftoverAlpha = 1f - texAlpha;

                    //Here alphas is used to store ratios
                    alphas[texture] = 0;
                    for(int i = 0; i < layers; ++i)
                    {
                        if(i != texture)
                        {
                            alphas[i] = splats[y - gridY, x - gridX, i] / leftoverAlpha;
                        }
                    }

                    if (dist < toolRadius * solidTexRadPercent)
                    {
                        texAlpha = 1f;
                    }
                    else
                    {
                        texAlpha = Mathf.Lerp(0f, 1f,
                            Mathf.InverseLerp(toolRadius, toolRadius * solidTexRadPercent, dist));
                    }

                    //texAlpha = Mathf.Lerp(0, 1, Mathf.InverseLerp(toolRadius + (toolRadius * solidTexRadPercent), 0, dist));
                    if(splats[y - gridY, x - gridX, texture] < texAlpha)
                    {
                        leftoverAlpha = 1f - texAlpha;
                        splats[y - gridY, x - gridX, texture] = texAlpha;

                        for (int i = 0; i < layers; ++i)
                        {
                            if (i != texture)
                            {
                                splats[y - gridY, x - gridX, i] = leftoverAlpha * alphas[i];
                            }
                        }
                    }
                }
            }
        }

        data.terrain.terrainData.SetAlphamaps(gridX, gridY, splats);
    }

    public void SetTexture(int id)
    {
        texture = id;
    }

}
