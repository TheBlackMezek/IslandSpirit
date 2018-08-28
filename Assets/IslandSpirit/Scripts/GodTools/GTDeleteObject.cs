using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GTDeleteObject : GodToolAbstract {

    public override void OnMouseHeld(int button, TerrainHitData data, float dt, float toolRadius, GameObject placablePrefab)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray.origin, ray.direction, out hit, 10000f, LayerMask.GetMask("PlacableObject")))
        {
            Destroy(hit.collider.transform.root.gameObject);
        }
    }

}
