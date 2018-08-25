using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodController : MonoBehaviour {

    public float lookSensitivity;
    public float moveSpeed;

    public float terrainRaiseSpeed;

    private float xRotOffset = 0;
    private Terrain terrain;



    private void OnValidate()
    {
        terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray.origin, ray.direction, out hit, 10000f, LayerMask.GetMask("TerrainEdit")))
            {
                Vector3 v3pos = hit.point - terrain.transform.position;
                v3pos.x /= terrain.terrainData.size.x;
                v3pos.y /= terrain.terrainData.size.y;
                v3pos.z /= terrain.terrainData.size.z;
                Vector2 terrainPos = new Vector2(v3pos.x * terrain.terrainData.heightmapWidth,
                                                 v3pos.z * terrain.terrainData.heightmapHeight);
                float[,] heights = terrain.terrainData.GetHeights((int)terrainPos.x, (int)terrainPos.y, 1, 1);
                heights[0, 0] += terrainRaiseSpeed * Time.deltaTime;
                terrain.terrainData.SetHeights((int)terrainPos.x, (int)terrainPos.y, heights);
            }
        }

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

}
