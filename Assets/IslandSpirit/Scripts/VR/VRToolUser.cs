using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRToolUser : MonoBehaviour {

    [SerializeField]
    private GodController godController;
    [SerializeField]
    private GameObject placeablePrefab;
    [SerializeField]
    private float toolRadius;
    [SerializeField]
    private Transform targetCircle;
    [SerializeField]
    private VRTK.VRTK_ControllerEvents controllerEvents;
    [SerializeField]
    private VRTK.VRTK_Pointer pointer;

    private Terrain terrain;

    private GodToolAbstract tool;
    private TerrainHitData hitdat = new TerrainHitData();
    private float lastdt = 0;
    private bool pointerValid = false;
    private float prevBrushRadius = float.MinValue;
    private bool triggerDown = false;





    #region Setup
    //private void OnValidate()
    //{
    //    pointer = GetComponent<VRTK.VRTK_Pointer>();
    //}

    private void Awake()
    {
        controllerEvents.TriggerClicked += new VRTK.ControllerInteractionEventHandler(OnTriggerClicked);
        controllerEvents.TriggerUnclicked += new VRTK.ControllerInteractionEventHandler(OnTriggerUnclicked);
    }

    private void Start()
    {
        terrain = TerrainGlobal.terrain;

        hitdat.terrain = terrain;

        if (godController.tools.Length > 0)
        {
            SetTool(0);
        }
    }
    #endregion

    

    private void Update()
    {
        lastdt = Time.deltaTime;
        pointerValid = pointer.IsStateValid();

        RaycastHit hit = pointer.pointerRenderer.GetDestinationHit();

        Vector2 terrainPos = Vector2.zero;
        float tHeight = 0;
        Vector3 floorHitPos = Vector3.zero;

        if(pointerValid)
        {
            floorHitPos = hit.point;
            Vector3 v3pos = hit.point - terrain.transform.position;
            v3pos.x /= terrain.terrainData.size.x;
            v3pos.y /= terrain.terrainData.size.y;
            v3pos.z /= terrain.terrainData.size.z;
            terrainPos = new Vector2(v3pos.x * terrain.terrainData.heightmapWidth,
                                             v3pos.z * terrain.terrainData.heightmapHeight);

            targetCircle.gameObject.SetActive(true);
            tHeight = terrain.SampleHeight(hit.point);
            targetCircle.position = new Vector3(hit.point.x, tHeight + godController.targetCircleFloatHeight, hit.point.z);
            targetCircle.eulerAngles = Vector3.zero;
            targetCircle.localScale = new Vector3(toolRadius * 2, 1, toolRadius * 2);
            if (toolRadius != prevBrushRadius)
            {
                prevBrushRadius = toolRadius;
                tool.OnBrushRadiusChange(toolRadius);
            }
        }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit2;
        bool terrainSurfHit = Physics.Raycast(ray.origin, ray.direction, out hit2, 10000f, LayerMask.GetMask("Terrain"));
        hitdat.physicalHitPoint = terrainSurfHit ? hit2.point : Vector3.zero;
        hitdat.terrainHitPos = terrainPos;
        hitdat.heightAtFloorPos = tHeight;
        hitdat.floorHitPos = floorHitPos;
        hitdat.floorHitPlusTHeight = new Vector3(floorHitPos.x, terrain.SampleHeight(floorHitPos), floorHitPos.z);



        if(triggerDown)
        {
            tool.OnMouseHeld(0, hitdat, lastdt, toolRadius, placeablePrefab);
        }
    }

    private void OnTriggerClicked(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        triggerDown = true;
        if(tool != null)
        {
            tool.OnMouseDown(0, hitdat, lastdt, toolRadius, placeablePrefab);
        }
    }

    private void OnTriggerUnclicked(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        triggerDown = false;
        tool.OnMouseDown(0, hitdat, lastdt, toolRadius, placeablePrefab);
    }

    private void SetTool(int idx)
    {
        if (tool != null)
        {
            tool.OnToolDeselect();
        }

        tool = godController.tools[idx];
        tool.OnToolSelect(placeablePrefab);
        tool.OnBrushRadiusChange(toolRadius);
    }

}
