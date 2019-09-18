using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundFX : MonoBehaviour
{
	public AudioSource select;

	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

	public void play() {
		select.Play ();
	}
}
