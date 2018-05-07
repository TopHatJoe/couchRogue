using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalScr : MonoBehaviour
{
	private ShipScript ship;
	private WeaponSysScr weaponSysScr;
	private WeaponScript currentWeapon;


	public void UseTerminal (CouchCrewScript _user) {
		Debug.LogError ("hello there mr. crew!");


		//gun terminal behaviour

			//switch to full ship view

		//Camera.main.enabled = true;
		ship = LevelManager.Instance.Ships[0].GetComponent <ShipScript> ();
		Camera _shipCam = ship.ShipCam;
		if (_shipCam == null) {
			Debug.LogError ("no cam!");
		} else {
			_shipCam.gameObject.SetActive (true);
		}

		GameObject _sysObj = transform.parent.parent.GetChild (1).gameObject;
		if (_sysObj.transform.childCount > 0) {
			if (_sysObj.transform.GetChild (0).GetComponent <WeaponSysScr> () != null) {
				weaponSysScr = _sysObj.transform.GetChild (0).GetComponent <WeaponSysScr> ();
			} else {
				Debug.LogError ("weaponSysScr == null");
			}
		}

		//should be outsourced to weaponTerminal subScr?
		if (ship.WeaponList.Count > 0) {
			currentWeapon = ship.WeaponList [0];
		}

		if (currentWeapon != null) {
			currentWeapon.IsPowered = true;
		}


		//when terminal is used -> sys is powered. 
		//for weaponScr that means, that you may distribute any available weaponPower
		//players can only do that by using a terminal.
		//-> there may be weaponSys without terminals, but you can't interface with it, just boost the available power


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

		if (currentWeapon != null) {
			currentWeapon.IsPowered = true;
		}
	}
}