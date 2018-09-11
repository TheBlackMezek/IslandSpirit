using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjectTest : MonoBehaviour {

    [SerializeField]
    private GameObject prefab;

    private VRTK.VRTK_Pointer pointer;



    private void OnValidate()
    {
        pointer = GetComponent<VRTK.VRTK_Pointer>();
    }

    private void Awake()
    {
        pointer.SelectionButtonPressed += OnSelectPressed;
        Debug.Log("Subscribed to event");
    }

    private void OnSelectPressed(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        Debug.Log("Is state valid? " + pointer.IsStateValid());
        if(pointer.IsStateValid())
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.position = pointer.pointerRenderer.GetDestinationHit().point;
            Debug.Log("Created object & set position to:" + obj.transform.position);
        }
    }

}
