using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class GodController : MonoBehaviour {

    public float lookSensitivity;
    public float moveSpeed;

    public float terrainRaiseSpeed;
    public float terrainRaiseRadius;

    public float targetCircleFloatHeight;
    
    public GodToolAbstract[] tools;

    public Text selectedToolText;

    private float xRotOffset = 0;
    private Terrain terrain;
    private Transform targetCircle;
    private GodToolAbstract activeTool;



    private void OnValidate()
    {
        terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        targetCircle = transform.Find("TargetCircle");
        if(tools.Length > 0)
        {
            SetTool(0);
        }
    }

    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool impact = Physics.Raycast(ray.origin, ray.direction, out hit, 10000f, LayerMask.GetMask("TerrainFloor"));
        Vector2 terrainPos = Vector2.zero;
        float tHeight = 0;

        if (impact)
        {
            Vector3 v3pos = hit.point - terrain.transform.position;
            v3pos.x /= terrain.terrainData.size.x;
            v3pos.y /= terrain.terrainData.size.y;
            v3pos.z /= terrain.terrainData.size.z;
            terrainPos = new Vector2(v3pos.x * terrain.terrainData.heightmapWidth,
                                             v3pos.z * terrain.terrainData.heightmapHeight);

            targetCircle.gameObject.SetActive(true);
            tHeight = terrain.SampleHeight(hit.point);
            targetCircle.position = new Vector3(hit.point.x, tHeight + targetCircleFloatHeight, hit.point.z);
            targetCircle.eulerAngles = Vector3.zero;
            targetCircle.localScale = new Vector3(terrainRaiseRadius, 1, terrainRaiseRadius);
        }
        else
        {
            targetCircle.gameObject.SetActive(false);
        }

        

        impact = Physics.Raycast(ray.origin, ray.direction, out hit, 10000f, LayerMask.GetMask("Terrain"));
        Vector3 terrainHitPoint = impact ? hit.point : Vector3.zero;

        if (activeTool != null && !EventSystem.current.IsPointerOverGameObject())
        {
            TerrainHitData data;
            data.terrain = terrain;
            data.floorHitPos = terrainPos;
            data.heightAtFloorPos = tHeight;
            data.physicalHitPoint = terrainHitPoint;

            float dt = Time.deltaTime;

            #region MouseButtonCalls
            if (Input.GetMouseButton(0))
            {
                activeTool.OnMouseHeld(0, data, dt);
            }
            else if(Input.GetMouseButtonDown(0))
            {
                activeTool.OnMouseDown(0, data, dt);
            }
            else if(Input.GetMouseButtonUp(0))
            {
                activeTool.OnMouseUp(0, data, dt);
            }

            if (Input.GetMouseButton(1))
            {
                activeTool.OnMouseHeld(1, data, dt);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                activeTool.OnMouseDown(1, data, dt);
            }
            else if (Input.GetMouseButtonUp(1))
            {
                activeTool.OnMouseUp(1, data, dt);
            }

            if (Input.GetMouseButton(2))
            {
                activeTool.OnMouseHeld(2, data, dt);
            }
            else if (Input.GetMouseButtonDown(2))
            {
                activeTool.OnMouseDown(2, data, dt);
            }
            else if (Input.GetMouseButtonUp(2))
            {
                activeTool.OnMouseUp(2, data, dt);
            }
            #endregion

            float scrollAmt = Input.GetAxis("Mouse ScrollWheel");
            if(scrollAmt != 0)
            {
                activeTool.OnMouseScroll(scrollAmt, data, dt);
            }
        }

        if (Input.GetMouseButton(1))
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

    public void SetTool(int idx)
    {
        activeTool = tools[idx];
        selectedToolText.text = "SELECTED:\n" + activeTool.toolName;
    }

}
