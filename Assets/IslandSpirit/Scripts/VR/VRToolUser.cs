using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRToolUser : MonoBehaviour {

    [SerializeField]
    private float targetCircleFloatHeight;
    [SerializeField]
    private float targetCylinderExtraHeight;
    [SerializeField]
    private float radiusIncrementMultiplier;
    [SerializeField]
    private GameObject[] placeableObjects;
    [SerializeField]
    private GodToolAbstract[] tools;
    [SerializeField]
    private float toolRadius;
    [SerializeField]
    private Transform targetCircle;
    [SerializeField]
    private Transform targetCylinder;
    [SerializeField]
    private VRTK.VRTK_ControllerEvents controllerEvents;
    [SerializeField]
    private VRTK.VRTK_Pointer pointer;

    private Terrain terrain;
    private GameObject placeablePrefab;

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

        if (tools.Length > 0)
        {
            SetTool(0);
        }

        SetPlaceableObject(0);
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

            if(tool.usesTargetCylinder)
            {
                targetCircle.gameObject.SetActive(true);
                tHeight = terrain.SampleHeight(hit.point);
                targetCircle.position = new Vector3(hit.point.x, tHeight + targetCircleFloatHeight, hit.point.z);
                targetCircle.eulerAngles = Vector3.zero;
                targetCircle.localScale = new Vector3(toolRadius * 2, 1, toolRadius * 2);
                float cylinderHeight = tHeight / 2f + targetCylinderExtraHeight;
                targetCylinder.position = new Vector3(hit.point.x, cylinderHeight - targetCylinderExtraHeight / 2f, hit.point.z);
                targetCylinder.localScale = new Vector3(1, cylinderHeight, 1);
                if (toolRadius != prevBrushRadius)
                {
                    prevBrushRadius = toolRadius;
                    tool.OnBrushRadiusChange(toolRadius);
                }
            }
        }
        else
        {
            targetCircle.gameObject.SetActive(false);
        }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit2;
        bool terrainSurfHit = Physics.Raycast(ray.origin, ray.direction, out hit2, 10000f, LayerMask.GetMask("Terrain"));
        hitdat.physicalHitPoint = terrainSurfHit ? hit2.point : Vector3.zero;
        hitdat.terrainHitPos = terrainPos;
        hitdat.heightAtFloorPos = tHeight;
        hitdat.floorHitPos = floorHitPos;
        hitdat.floorHitPlusTHeight = new Vector3(floorHitPos.x, terrain.SampleHeight(floorHitPos), floorHitPos.z);



        if(tool != null)
        {
            tool.ToolUpdate(hitdat, lastdt);
            if(triggerDown)
            {
                tool.OnMouseHeld(0, hitdat, lastdt, toolRadius, placeablePrefab);
            }
        }
    }

    public void Deactivate()
    {
        if(tool != null)
        {
            tool.OnToolDeselect();
        }
        targetCircle.gameObject.SetActive(false);
    }

    public void Activate()
    {
        if(tool != null)
        {
            tool.OnToolSelect(placeablePrefab);
        }
        targetCircle.gameObject.SetActive(true);
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
        if(tool != null)
        {
            tool.OnMouseUp(0, hitdat, lastdt, toolRadius, placeablePrefab);
        }
    }

    public void SetTool(int idx)
    {
        if (tool != null)
        {
            tool.OnToolDeselect();
        }

        tool = tools[idx];
        tool.OnToolSelect(placeablePrefab);
        tool.OnBrushRadiusChange(toolRadius);

        if(tool.usesTargetCylinder)
        {
            targetCircle.gameObject.SetActive(true);
        }
        else
        {
            targetCircle.gameObject.SetActive(false);
        }
    }

    public void IncrementBrushRadius(int dir)
    {
        toolRadius += toolRadius * dir * radiusIncrementMultiplier;
        if(tool != null)
        {
            tool.OnBrushRadiusChange(toolRadius);
            tool.OnMouseScroll(dir * radiusIncrementMultiplier, hitdat, lastdt, toolRadius, placeablePrefab);
        }
    }

    public void SetPlaceableObject(int idx)
    {
        placeablePrefab = WorldManager.Instance.GetPlaceablePrefab(idx);
        tool.OnSlectedPlacableObjectChange(placeablePrefab);
    }

}
