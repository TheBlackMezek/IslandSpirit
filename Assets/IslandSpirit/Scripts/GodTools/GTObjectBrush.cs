using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GTObjectBrush : GodToolAbstract
{

    public float objectDensity;

    public string ghostLayer;

    private GameObject ghost;



    public override void OnToolSelect(GameObject placablePrefab)
    {
        ghost = Instantiate(placablePrefab);
        RecursiveChangeLayer(ghost);
    }

    private void RecursiveChangeLayer(GameObject obj)
    {
        obj.layer = LayerMask.NameToLayer(ghostLayer);
        for (int i = 0; i < obj.transform.childCount; ++i)
        {
            RecursiveChangeLayer(obj.transform.GetChild(i).gameObject);
        }
    }

    public override void OnToolDeselect()
    {
        Destroy(ghost);
    }

    public override void ToolUpdate(TerrainHitData data, float dt)
    {
        ghost.transform.position = data.physicalHitPoint;
    }

    public override void OnSlectedPlacableObjectChange(GameObject placablePrefab)
    {
        Destroy(ghost);
        ghost = Instantiate(placablePrefab);
        RecursiveChangeLayer(ghost);
    }

    public override void OnMouseDown(int button, TerrainHitData data, float dt, float toolRadius, GameObject placablePrefab)
    {
        if (button == 0)
        {
            float area = Mathf.PI * (toolRadius * toolRadius);
            int objCount = Mathf.RoundToInt(objectDensity * area);

            for(int i = 0; i < objCount; ++i)
            {
                GameObject obj = Instantiate(placablePrefab);
                obj.transform.position = ghost.transform.position;
                obj.transform.eulerAngles = Vector3.up * Random.Range(0f, 360f);
                obj.transform.position += obj.transform.forward * Random.Range(0f, toolRadius);
                gc.AddPlacedObject(obj);
            }
        }
    }

}
