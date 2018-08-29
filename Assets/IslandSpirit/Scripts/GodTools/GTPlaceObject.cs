using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GTPlaceObject : GodToolAbstract {

    public string ghostLayer;

    private GameObject ghost;



    public override void OnToolSelect(GameObject placablePrefab)
    {
        MakeGhost(placablePrefab);
    }

    private void RecursiveChangeLayer(GameObject obj)
    {
        obj.layer = LayerMask.NameToLayer(ghostLayer);

        MonoBehaviour[] script = obj.GetComponents<MonoBehaviour>();
        for(int i = 0; i < script.Length; ++i)
        {
            script[i].enabled = false;
        }

        for(int i = 0; i < obj.transform.childCount; ++i)
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
        MakeGhost(placablePrefab);
    }

    private void MakeGhost(GameObject placablePrefab)
    {
        ghost = Instantiate(placablePrefab);
        RecursiveChangeLayer(ghost);
    }

    public override void OnMouseDown(int button, TerrainHitData data, float dt, float toolRadius, GameObject placablePrefab)
    {
        if(button == 0)
        {
            GameObject obj = Instantiate(placablePrefab);
            obj.transform.position = ghost.transform.position;
            obj.transform.eulerAngles = new Vector3(0, Random.Range(0f, 360f), 0);
            gc.AddPlacedObject(obj);
            particleSystem.transform.position = ghost.transform.position;
            particleSystem.Play();
        }
    }

}
