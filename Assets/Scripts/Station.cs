using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station
{
    public string name;
    public FSM.State state;
    public float speed;
    public Queue<Capsule> queue;

    public Station(string theName, FSM.State theState, float theSpeed)
    {
        name = theName;
        state = theState;
        speed = theSpeed;
        queue = new Queue<Capsule>();
    }

}
