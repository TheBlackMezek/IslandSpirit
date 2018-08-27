using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodController : MonoBehaviour {

    public float lookSensitivity;
    public float moveSpeed;

    public float terrainRaiseSpeed;
    public float terrainRaiseRadius;

    public float targetCircleFloatHeight;

    private float xRotOffset = 0;
    private Terrain terrain;
    private Transform targetCircle;



    private void OnValidate()
    {
        terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        targetCircle = transform.Find("TargetCircle");
    }

    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool impact = Physics.Raycast(ray.origin, ray.direction, out hit, 10000f, LayerMask.GetMask("TerrainEdit"));

        if (impact)
        {
            Vector3 v3pos = hit.point - terrain.transform.position;
            v3pos.x /= terrain.terrainData.size.x;
            v3pos.y /= terrain.terrainData.size.y;
            v3pos.z /= terrain.terrainData.size.z;
            Vector2 terrainPos = new Vector2(v3pos.x * terrain.terrainData.heightmapWidth,
                                             v3pos.z * terrain.terrainData.heightmapHeight);

            targetCircle.gameObject.SetActive(true);
            float tHeight = terrain.SampleHeight(hit.point);
            targetCircle.position = new Vector3(hit.point.x, tHeight + targetCircleFloatHeight, hit.point.z);
            targetCircle.eulerAngles = Vector3.zero;
            targetCircle.localScale = new Vector3(terrainRaiseRadius, 1, terrainRaiseRadius);

            if (Input.GetMouseButton(0))
            {
                RaiseTerrainCircleLerpBrush((int)terrainPos.x, (int)terrainPos.y, terrainRaiseRadius,
                                            Time.deltaTime * terrainRaiseSpeed);
            }
        }
        else
        {
            targetCircle.gameObject.SetActive(false);
        }

        //if (Input.GetMouseButton(0))
        //{
        //    if(impact)
        //    {
        //        Vector3 v3pos = hit.point - terrain.transform.position;
        //        v3pos.x /= terrain.terrainData.size.x;
        //        v3pos.y /= terrain.terrainData.size.y;
        //        v3pos.z /= terrain.terrainData.size.z;
        //        Vector2 terrainPos = new Vector2(v3pos.x * terrain.terrainData.heightmapWidth,
        //                                         v3pos.z * terrain.terrainData.heightmapHeight);
        //        RaiseTerrainCircleLerpBrush((int)terrainPos.x, (int)terrainPos.y, terrainRaiseRadius,
        //                                    Time.deltaTime * terrainRaiseSpeed);
        //    }
        //}

        if(Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            transform.eulerAngles += Vector3.up * Input.GetAxis("Mouse X")
                                   * lookSensitivity * Time.deltaTime;
            xRotOffset += -Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;
            Mathf.Clamp(xRotOffset, -90f, 90f);
            transform.eulerAngles = new Vector3(xRotOffset, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        transform.position += transform.forward * Input.GetAxis("ForeBack") * Time.deltaTime * moveSpeed
                            + transform.right * Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed
                            + transform.up * Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
    }

    private void RaiseTerrainCircleLerpBrush(int centerX, int centerY, float radius, float raiseAmt)
    {
        int radiusInt = (int)Mathf.Ceil(radius);
        int diameter = radiusInt * 2 + 1;
        int lenX = diameter;
        int lenY = diameter;

        int gridX = centerX - radiusInt;
        if(gridX < 0)
        {
            lenX += gridX;
            gridX = 0;
        }
        if(gridX + lenX >= terrain.terrainData.heightmapWidth)
        {
            lenX = terrain.terrainData.heightmapWidth - gridX - 1;
        }
        int gridY = centerY - radiusInt;
        if (gridY < 0)
        {
            lenY += gridY;
            gridY = 0;
        }
        if (gridY + lenY >= terrain.terrainData.heightmapHeight)
        {
            lenY = terrain.terrainData.heightmapHeight - gridY - 1;
        }
        
        Vector2 loopCenter = new Vector2(centerX, centerY);



        float[,] heights = terrain.terrainData.GetHeights(gridX, gridY, lenX, lenY);
        
        Vector2 loopPos;
        for(int x = gridX; x < gridX + lenX; ++x)
        {
            loopPos.x = x;
            for (int y = gridY; y < gridY + lenY; ++y)
            {
                loopPos.y = y;
                float dist = Vector2.Distance(loopPos, loopCenter);
                if (dist <= radius)
                {
                    heights[y - gridY, x - gridX] += Mathf.Lerp(0, raiseAmt, Mathf.InverseLerp(radius, 0, dist));
                }
            }
        }

        terrain.terrainData.SetHeights(gridX, gridY, heights);
    }

}
