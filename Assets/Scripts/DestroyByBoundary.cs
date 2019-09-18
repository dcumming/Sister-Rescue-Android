// Attached to boundary component

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Bullet" || other.tag == "Fire")
        {
            Destroy(other.gameObject);
        }
    }
}
