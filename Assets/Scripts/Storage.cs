using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage
{
    public string name;
    public FSM.State state;
    public Queue<Capsule> queue;

    public Storage(string theName, FSM.State theState)
    {
        name = theName;
        state = theState;
        queue = new Queue<Capsule>();
    }

}
