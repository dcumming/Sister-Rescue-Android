using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
	{
		Time.timeScale = 1.0f;
	}


	public IEnumerator transition (int s, string scene) {
		yield return new WaitForSeconds (s);
		SceneManager.LoadScene (scene);
	}

	public IEnumerator hold (int s) {
		yield return new WaitForSeconds (s);
	}

	public void changeScene(string name) {
		if (name == "-1") {
			Application.Quit ();
		} else {
			SceneManager.LoadScene (name);
		}
	}

	public void changeSceneWait(string name) {
		StartCoroutine (transition(2, name));
	}
}
