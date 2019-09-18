// Attached to boundary component

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {


        Debug.Log("i will lap dance 4 extra credit");
        Destroy(other.gameObject);
    }
}
