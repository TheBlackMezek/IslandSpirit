using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Profile", menuName = "Object Property Profiles/Basic")]
public class PropertiesProfile : ScriptableObject {

    [SerializeField]
    private string[] tags;



    public bool HasTag(string tag)
    {
        for(int i = 0; i < tags.Length; ++i)
        {
            if(tags[i] == tag)
            {
                return true;
            }
        }
        return false;
    }

}
