using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerIni : MonoBehaviour {
	private int localPlayerCount = 0;
	[SerializeField]
	private GameObject crewPrefab;

	private List <GameObject> crewList = new List <GameObject> ();
	private List <Camera> camList = new List <Camera> ();

	void Update () {
		if (Input.GetButtonDown ("J01-s")) {
			CreateCrew ("J01");
		}	
		if (Input.GetButtonDown ("J02-s")) {
			CreateCrew ("J02");
		}
		if (Input.GetButtonDown ("J03-s")) {
			CreateCrew ("J03");
		}
		if (Input.GetButtonDown ("J04-s")) {
			CreateCrew ("J04");
		}
	}

	private void CreateCrew (string _controllerID) {
		Debug.Log (_controllerID);

		GameObject _obj = (GameObject)Instantiate (crewPrefab);
		ControllerMappingTemp _ctrl = _obj.GetComponent <ControllerMappingTemp> ();
		_ctrl.ControllerID = _controllerID;

		crewList.Add (_obj);
		camList.Add (_obj.transform.GetChild (0).GetComponent <Camera> ());
		localPlayerCount++;


		if (localPlayerCount == 1) {
			Camera.main.gameObject.SetActive (false);
		} else if (localPlayerCount == 2) {
			TwoPlayerSplit ();
		} else if (localPlayerCount == 3) {
			ThreePlayerSplit ();
		} else if (localPlayerCount == 4) {
			FourPlayerSplit ();
		}
	}

	private void TwoPlayerSplit () {
		Rect _rect0 = new Rect (0f, 0.5f, 1f, 0.5f);
		Rect _rect1 = new Rect (0f, 0f, 1f, 0.5f);

		camList [0].rect = _rect0;
		camList [1].rect = _rect1;
	}

	private void ThreePlayerSplit () {
		Rect _rect0 = new Rect (0f, 0.5f, 1f, 0.5f);
		Rect _rect1 = new Rect (0f, 0f, 0.5f, 0.5f);
		Rect _rect2 = new Rect (0.5f, 0f, 0.5f, 0.5f);

		camList [0].rect = _rect0;
		camList [1].rect = _rect1;
		camList [2].rect = _rect2;
	}

	private void FourPlayerSplit () {
		Rect _rect0 = new Rect (0f, 0.5f, 0.5f, 0.5f);
		Rect _rect1 = new Rect (0.5f, 0f, 0.5f, 0.5f);
		Rect _rect2 = new Rect (0f, 0.5f, 0.5f, 0.5f);
		Rect _rect3 = new Rect (0.5f, 0f, 0.5f, 0.5f);

		camList [0].rect = _rect0;
		camList [1].rect = _rect1;
		camList [2].rect = _rect2;
		camList [3].rect = _rect3;
	}
}