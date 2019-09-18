using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pauseGUI : MonoBehaviour
{
	public AudioSource music;
	public Slider vl;
	public Slider sfx;

    // Start is called before the first frame update
    void Start()
    {
		vl.value = 0.5f;
		sfx.value = 0.5f;
		music.ignoreListenerVolume = true;
    }

    // Update is called once per frame
    void Update()
    {
		music.volume = vl.value;
		AudioListener.volume = sfx.value;
    }
}
