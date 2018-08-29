using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProperties : MonoBehaviour {

    [SerializeField]
    private PropertiesProfile profile;

    public PropertiesProfile GetProfile() { return profile; }

}
