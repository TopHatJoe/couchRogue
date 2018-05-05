using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalScr : MonoBehaviour
{
	public void UseTerminal (CouchCrewScript _user) {
		Debug.LogError ("hello there mr. crew!");


		//gun terminal behaviour

			//switch to full ship view

		//Camera.main.enabled = true;
		Camera _shipCam = LevelManager.Instance.Ships[0].GetComponent <ShipScript> ().ShipCam;
		if (_shipCam == null) {
			Debug.LogError ("no cam!");
		} else {
			_shipCam.gameObject.SetActive (true);
		}

			//enabe cursor //-> where does it start?

			//while in this view, deactivate couchCrew movement

			//o exits the terminal
	}

	public void StopUsingTerminal () {
		//Camera.main.enabled = false;
		Camera _shipCam = LevelManager.Instance.Ships[0].GetComponent <ShipScript> ().ShipCam;
		if (_shipCam == null) {
			Debug.LogError ("no cam!");
		} else {
			_shipCam.gameObject.SetActive (false);
		}
	}
}