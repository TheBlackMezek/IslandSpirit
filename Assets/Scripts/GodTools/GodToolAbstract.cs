using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GodToolAbstract : MonoBehaviour
{

    public string toolName;

    public virtual void OnMouseDown(int button, TerrainHitData data, float dt)
    {

    }

    public virtual void OnMouseHeld(int button, TerrainHitData data, float dt)
    {
        
    }

    public virtual void OnMouseUp(int button, TerrainHitData data, float dt)
    {

    }

    public virtual void OnMouseScroll(float amt, TerrainHitData data, float dt)
    {

    }

}
