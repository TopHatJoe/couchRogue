using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerMenu : MonoBehaviour 
{
	[SerializeField]
	private Button[] buttons;

	[SerializeField]
	private int standard;
	private int activeBtn;


	void Start () {
		activeBtn = standard;
		buttons [activeBtn].image.color = Color.green;
	}

	void Update () {
		if (Input.GetButtonDown ("J00-H")) {
			buttons [activeBtn].image.color = Color.gray;

			float _value = Input.GetAxisRaw ("J00-H");
			if (_value < 0) {
				//Debug.Log ("+1");
				activeBtn += 1;
			} else if (_value > 0) {
				//Debug.Log ("-1");
				activeBtn -= 1;
			}

			if (activeBtn >= buttons.Length) {
				activeBtn = 0;
			} else if (activeBtn < 0) {
				activeBtn = (buttons.Length - 1);
			}

			buttons [activeBtn].image.color = Color.green;
		}

		if (Input.GetButtonDown ("Submit")) {
			buttons [activeBtn].onClick.Invoke ();
		}
	}
}