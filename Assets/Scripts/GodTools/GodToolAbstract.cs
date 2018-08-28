using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GodToolAbstract : MonoBehaviour
{

    public string toolName;

    protected GodController gc;



    protected virtual void OnValidate()
    {
        gc = FindObjectOfType<GodController>();
    }



    public virtual void OnToolSelect(GameObject placablePrefab)
    {

    }

    public virtual void OnToolDeselect()
    {

    }

    public virtual void ToolUpdate(TerrainHitData data, float dt)
    {

    }

    public virtual void OnMouseDown(int button, TerrainHitData data, float dt, float toolRadius, GameObject placablePrefab)
    {

    }

    public virtual void OnMouseHeld(int button, TerrainHitData data, float dt, float toolRadius, GameObject placablePrefab)
    {
        
    }

    public virtual void OnMouseUp(int button, TerrainHitData data, float dt, float toolRadius, GameObject placablePrefab)
    {

    }

    public virtual void OnMouseScroll(float amt, TerrainHitData data, float dt, float toolRadius, GameObject placablePrefab)
    {

    }

    public virtual void OnSlectedPlacableObjectChange(GameObject placablePrefab)
    {

    }

}
