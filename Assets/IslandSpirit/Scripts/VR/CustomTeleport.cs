using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTeleport : MonoBehaviour {

    [SerializeField]
    private VRTK.VRTK_HeightAdjustTeleport teleporter;
    [SerializeField]
    private LayerMask layerMask;

    private VRTK.VRTK_ControllerEvents controllerEvents;



    private void OnValidate()
    {
        controllerEvents = GetComponent<VRTK.VRTK_ControllerEvents>();
    }

    private void Awake()
    {
        controllerEvents.GripClicked += new VRTK.ControllerInteractionEventHandler(Teleport);
    }

    private void Teleport(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 10000f, layerMask))
        {
            teleporter.Teleport(hit.transform, hit.point);
        }
    }

}
