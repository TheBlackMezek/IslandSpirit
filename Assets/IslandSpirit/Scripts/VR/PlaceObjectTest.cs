using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjectTest : MonoBehaviour {

    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private VRTK.VRTK_HeightAdjustTeleport teleporter;
    [SerializeField]
    private Transform camRig;

    private VRTK.VRTK_Pointer pointer;
    private VRTK.VRTK_ControllerEvents controllerEvents;



    private void OnValidate()
    {
        pointer = GetComponent<VRTK.VRTK_Pointer>();
        controllerEvents = GetComponent<VRTK.VRTK_ControllerEvents>();
    }

    private void Start()
    {
        pointer.SelectionButtonPressed += OnPlaceButtonPressed;
        controllerEvents.GripClicked += new VRTK.ControllerInteractionEventHandler(OnTeleportButtonPressed);
    }

    private void OnPlaceButtonPressed(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        Debug.Log("Is state valid? " + pointer.IsStateValid());
        if(pointer.IsStateValid())
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.position = pointer.pointerRenderer.GetDestinationHit().point;
            obj.transform.eulerAngles = Vector3.up * Random.Range(0f, 360f);
            Debug.Log("Created object & set position to:" + obj.transform.position);
        }
    }

    private void OnTeleportButtonPressed(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        Debug.Log("Is state valid? " + pointer.IsStateValid());
        if (pointer.IsStateValid())
        {
            Debug.Log("Attempting to teleport");
            camRig.position = pointer.pointerRenderer.GetDestinationHit().point;
            //teleporter.Teleport(null, pointer.pointerRenderer.GetDestinationHit().point);
            Debug.Log("Teleported to:" + camRig.position);
        }
    }

}
