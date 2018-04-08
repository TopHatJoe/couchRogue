using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerMenu : MonoBehaviour 
{
	[SerializeField]
	private Button[] buttons;

	private string controllerID = "J01";

	[SerializeField]
	private int standard;
	private int activeBtn;
	private bool hasSwitchedUp = false;
	private bool hasSwitchedDown = false;

	private bool isReady = false;
	public bool IsReady { get { return isReady; } set { isReady = value; } }


	void Start () {
		activeBtn = standard;
		buttons [activeBtn].image.color = Color.cyan;
	}

	void Update () {
		if (!isReady) {
			if (Input.GetButtonDown (controllerID + "-V")) {
				buttons [activeBtn].image.color = Color.gray;

				float _value = Input.GetAxisRaw (controllerID + "-V");
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

				buttons [activeBtn].image.color = Color.cyan;
			}

			if (Input.GetButtonDown (controllerID + "-s")) {
				buttons [activeBtn].onClick.Invoke ();
			}


			if (!hasSwitchedUp) {
				if (Input.GetAxisRaw (controllerID + "-V") > 0.32f) {
					buttons [activeBtn].image.color = Color.gray;
					activeBtn += 1;

					if (activeBtn >= buttons.Length) {
						activeBtn = 0;
					} else if (activeBtn < 0) {
						activeBtn = (buttons.Length - 1);
					}

					buttons [activeBtn].image.color = Color.cyan;

					hasSwitchedUp = true;
				} 
			} else {
				if (Input.GetAxisRaw (controllerID + "-V") < 0.32f) {
					hasSwitchedUp = false;
				} 
			}

			if (!hasSwitchedDown) {
				if (Input.GetAxisRaw (controllerID + "-V") < -0.32f) {
					buttons [activeBtn].image.color = Color.gray;
					activeBtn -= 1;

					if (activeBtn >= buttons.Length) {
						activeBtn = 0;
					} else if (activeBtn < 0) {
						activeBtn = (buttons.Length - 1);
					}

					buttons [activeBtn].image.color = Color.cyan;

					hasSwitchedDown = true;
				}
			} else {
				if (Input.GetAxisRaw (controllerID + "-V") > -0.5f) {
					hasSwitchedDown = false;
				}
			}
		}
	}

	public void GetControllerID (string _controllerID) {
		controllerID = _controllerID;
	}
}