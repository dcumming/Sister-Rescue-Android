// Attached to tilemap

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnImpact : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("IMPACT: " + other.tag);
        if (other.tag == "Bullet" || other.tag == "Fire")
        {
            Destroy(other.gameObject);
        }
    }
}
