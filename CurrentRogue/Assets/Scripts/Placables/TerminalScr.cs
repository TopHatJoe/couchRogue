using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalScr : MonoBehaviour
{
	private ShipScript ship;
	private WeaponSysScr weaponSysScr;
	private WeaponScript currentWeapon;
	private int currentWeaponID = 0;
	public int CurrentWeaponID { get { return currentWeaponID; } }

	private Color userColor;


	public void UseTerminal (CouchCrewScript _user) {
		Debug.LogError ("hello there mr. crew!");

		userColor = _user.CrewColor;
		//gun terminal behaviour

			//switch to full ship view

		//Camera.main.enabled = true;
		ship = LevelManager.Instance.Ships[0].GetComponent <ShipScript> ();
		Camera _shipCam = ship.ShipCam;
		if (_shipCam == null) {
			Debug.LogError ("no cam!");
		} else {
			//_shipCam.gameObject.SetActive (true);
			_user.SetCrewCamValues (ship, true);
		}

		GameObject _sysObj = transform.parent.parent.GetChild (1).gameObject;
		if (_sysObj.transform.childCount > 0) {
			if (_sysObj.transform.GetChild (0).GetComponent <WeaponSysScr> () != null) {
				weaponSysScr = _sysObj.transform.GetChild (0).GetComponent <WeaponSysScr> ();
			} else {
				Debug.LogError ("weaponSysScr == null");
			}
		}


		/*
		//should be outsourced to weaponTerminal subScr?
		if (ship.WeaponList.Count > currentWeaponID) {
			currentWeapon = ship.WeaponList [currentWeaponID];
		}

		if (currentWeapon != null) {
			currentWeapon.IsPowered = true;
		}
		*/
		GetWeapon ();
		GiveTerminalReference (_user);

		//when terminal is used -> sys is powered. 
		//for weaponScr that means, that you may distribute any available weaponPower
		//players can only do that by using a terminal.
		//-> there may be weaponSys without terminals, but you can't interface with it, just boost the available power


			//enabe cursor //-> where does it start?

			//while in this view, deactivate couchCrew movement

			//o exits the terminal
	}

	public void StopUsingTerminal () {
		/*
		//Camera.main.enabled = false;
		Camera _shipCam = LevelManager.Instance.Ships[0].GetComponent <ShipScript> ().ShipCam;
		if (_shipCam == null) {
			Debug.LogError ("no cam!");
		} else {
			_shipCam.gameObject.SetActive (false);
		}
		*/

		//_user.SetCrewCamValues (ship, false);

		//whats the purpose of this?
		if (currentWeapon != null) {
			currentWeapon.HandleOutline (false, userColor);
			currentWeapon.isUsedByCrew = false;
			currentWeapon.IsPowered = true;
		}
	}


	public void SwapWeapon (int _amount) {
		if (currentWeapon != null) { 
			//deacts the outline of previous weapon
			currentWeapon.HandleOutline (false, userColor);
			currentWeapon.isUsedByCrew = false;
		}

		int _counter = currentWeaponID;
		bool _weaponFree;

		while (true) {
			currentWeaponID += _amount;

			if (currentWeaponID < 0) {
				currentWeaponID = (ship.WeaponList.Count - 1);
			} else if (currentWeaponID >= ship.WeaponList.Count) {
				currentWeaponID = 0;
			}

			//Debug.LogError ("current gun: " + currentWeaponID);
			GetWeapon ();

			if (!currentWeapon.isUsedByCrew) {
				_weaponFree = true;
				break;
			}

			if (currentWeaponID == _counter) {
				Debug.LogError ("all weapons in use!");
				_weaponFree = false;
				break;
			}
		}


		if (_weaponFree) {
			ActivateWeapon ();
		} else {
			Debug.LogError ("stop using terminal");
			//StopUsingTerminal ();
		}
	}


	private void GetWeapon () {
		//should be outsourced to weaponTerminal subScr?

		/*
		if (currentWeapon != null) { 
			//deacts the outline of previous weapon
			currentWeapon.HandleOutline (false);
		}
		*/

		if (ship.WeaponList.Count > currentWeaponID) {
			currentWeapon = ship.WeaponList [currentWeaponID];
		} 

		/*
		if (currentWeapon != null) {
			currentWeapon.HandleOutline (true); 	

			if (!currentWeapon.IsPowered) {
				currentWeapon.IsPowered = true;
			}
		}
		*/
	}

	private void ActivateWeapon () {
		if (currentWeapon != null) {
			currentWeapon.isUsedByCrew = true;
			currentWeapon.HandleOutline (true, userColor); 	

			if (!currentWeapon.IsPowered) {
				currentWeapon.IsPowered = true;
			}
		}
	}

	private void GiveTerminalReference (CouchCrewScript _user) {
		_user.GiveTerminalReference (this);
	}
}