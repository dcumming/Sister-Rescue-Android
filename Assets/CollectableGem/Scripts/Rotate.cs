using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

	public float speed;
	public Vector3 rotateDirection;

	void Start() {
		speed += (Random.Range (-5.0f, 5.0f));
	}

	void Update(){
		transform.Rotate (rotateDirection * speed * Time.deltaTime);
	}
}
