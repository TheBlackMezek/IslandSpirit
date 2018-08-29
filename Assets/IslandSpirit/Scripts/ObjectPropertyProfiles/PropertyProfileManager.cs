using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyProfileManager : MonoBehaviour {
    
    public PropertyProfileManager Instance { get; private set; }

    [SerializeField]
    private PropertiesProfile[] propertyProfiles;



    private void Awake()
    {
        Instance = this;
    }

    public PropertiesProfile GetProfile(int idx)
    {
        return propertyProfiles[idx];
    }

    public PropertiesProfile GetObjectProfile(Transform t)
    {
        ObjectProperties op = t.root.GetComponent<ObjectProperties>();

        return op == null ? null : op.GetProfile();
    }

}
