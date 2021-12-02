using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule
{
    public string capsuleGUID;
    public GameObject capsuleObject;
    public FSM.State currentState;
    public string stateOfMatter;
    public Material material;

    public bool inProlog;
    public bool inMain;
    public bool inEpilog;

    public float timer;

    public bool inQueue;

    public Capsule(GameObject theCapsuleObject, FSM.State theCurrentState, string theStateOfMatter, Material theMaterial, string theGUID = "null")
    {
        capsuleObject = theCapsuleObject;
        currentState = theCurrentState;
        stateOfMatter = theStateOfMatter;

        theCapsuleObject.GetComponent<MeshRenderer>().material = theMaterial;
        material = theMaterial;

        capsuleGUID = "Capsule-" + System.Guid.NewGuid().ToString().Split('-')[0];

        inProlog = true;
        inMain = false;
        inEpilog = false;

        timer = 0f;

        inQueue = false;
    }

}
