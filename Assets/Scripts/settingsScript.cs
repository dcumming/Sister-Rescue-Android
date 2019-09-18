using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settingsScript : MonoBehaviour
{
	public Text how;
	private bool toggle;
	public GameObject cv;

    // Start is called before the first frame update
    void Start()
    {	
		if (SystemInfo.deviceType == DeviceType.Handheld) {
			how.text = "Use the joystick in the bottom left corner to move Sammy around the world. The button in the bottom right will make Sammy jump.";
		} else {
			how.text = "Use the WASD keys to move Sammy around the world. The space button will make Sammy jump";
		}
		how.text += "\n To complete a level you must defeat all animals and collect all gems. Be careful, you have a limited amount of health, if you are hit too many times or fall off a level, you will have to start over.";
    }

    // Update is called once per frame
    void Update()
    {
		cv.SetActive (toggle);
    }

	public void clickd(bool clicked) {
		toggle = clicked;
	}
}
