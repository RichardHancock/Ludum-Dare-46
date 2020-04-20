using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTriggered : MonoBehaviour
{
    private int collisions = 0;

    public bool Triggered() { return collisions > 0; }

    private void OnTriggerEnter(Collider other)
    {
        collisions++;
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Exit");
        collisions--;
    }
}
