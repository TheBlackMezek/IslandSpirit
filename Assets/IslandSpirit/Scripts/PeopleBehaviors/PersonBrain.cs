using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PersonMover))]
public class PersonBrain : MonoBehaviour {

    private enum PersonState
    {
        IDLE,
        WALK
    }



    [SerializeField]
    private float woodForHouse;
    [SerializeField]
    private float woodPerTree;
    [SerializeField]
    private float treeCheckRadius;
    [SerializeField]
    private GameObject housePrefab;
    [SerializeField]
    private string treeLayer;
    [SerializeField]
    private string treeTag;
    [SerializeField]
    private float randomWalkMaxDist;
    [SerializeField]
    private float arrivalThreshold;

    [SerializeField]
    private PersonMover mover;

    private PersonState state = PersonState.IDLE;
    private Vector3 walkGoal;
    private Transform walkGoalSource = null;
    private Transform house = null;



    private void OnValidate()
    {
        mover = GetComponent<PersonMover>();
    }

    private void Update()
    {
        if(state == PersonState.WALK)
        {
            mover.Walk((walkGoal - transform.position).normalized);
            if(Vector3.Distance(transform.position, walkGoal) <= arrivalThreshold)
            {
                state = PersonState.IDLE;
            }
        }
        else
        {
            if(house == null)
            {
                CheckForTree();
                if(walkGoalSource == null)
                {
                    SetRandomWalkGoal();
                }
            }
        }
    }

    private void CheckForTree()
    {
        Collider[] clist = Physics.OverlapSphere(transform.position, treeCheckRadius,
            LayerMask.NameToLayer(treeLayer));
        float leastDist = float.MaxValue;
        Transform leastTree = null;

        for(int i = 0; i < clist.Length; ++i)
        {
            PropertiesProfile profile = PropertyProfileManager.Instance.GetObjectProfile(clist[i].transform);
            if(profile && profile.HasTag(treeTag))
            {
                float dist = Vector3.Distance(transform.position, clist[i].transform.root.position);
                if(dist < leastDist)
                {
                    leastDist = dist;
                    leastTree = clist[i].transform.root;
                }
            }
        }

        walkGoalSource = leastTree;

        if(leastTree != null)
        {
            state = PersonState.WALK;
            walkGoal = leastTree.position;
        }
    }

    private void SetRandomWalkGoal()
    {
        if(house)
        {
            walkGoal = house.position;
        }
        else
        {
            walkGoal = transform.position;
        }

        walkGoal += new Vector3(Random.Range(-randomWalkMaxDist, randomWalkMaxDist),
                                0,
                                Random.Range(-randomWalkMaxDist, randomWalkMaxDist));
        walkGoal.y = TerrainGlobal.terrain.SampleHeight(walkGoal);

        state = PersonState.WALK;
    }

}
