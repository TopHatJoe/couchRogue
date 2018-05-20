using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSysScr : MonoBehaviour, ISystem
{
	//[SerializeField]
	private SystemScript systemScr;
	private HealthScript hScr;

	private Point gridPos;

	private int playerID;

	private ShipScript ship;

	[SerializeField]
	private int powerReq;
	//improtant for "isThere enough power?" checks
	private int fullPwrReq = 0;

	[SerializeField]
	//the amount by which the evasiveness is hightened
	private int componentCapacity;

	private bool isPowered = false;
	//public bool IsPowered { get { return isPowered; } }

	private int systemType = 2;

	//private bool isDamaged = false;
	//private bool isLocal = false;

	private ShipPowerMngr pwrMngr;
	private WeaponSysScr weaponSysScr;
	private bool isOrigin = false;


	void Start () {
		Setup ();
	}

	private void Setup () {
		systemScr = GetComponent <SystemScript> ();
		gridPos = systemScr.GridPos;
		playerID = gridPos.Z;

		ship = LevelManager.Instance.Ships [playerID].GetComponent <ShipScript> ();
		pwrMngr = ship.GetComponent <ShipPowerMngr> ();

		hScr = systemScr.GetOriginObj ().GetComponent <HealthScript> ();

		//ship.IncreaseEvasionChance (componentCapacity);

		pwrMngr.PowerSetup (systemType, powerReq);

		weaponSysScr = GetOriginWeaponSys ();
		if (this == weaponSysScr) {
			isOrigin = true;
		}

		weaponSysScr.fullPwrReq += powerReq;
	}


	public void SyncedPower (bool _isPowered) {
		isPowered = _isPowered;

		if (isPowered) {
			ship.IncreaseEvasionChance (componentCapacity);
		} else {
			ship.IncreaseEvasionChance (-componentCapacity);
		}
	}


	public void UpdateHealthState (bool _isFullyDamaged, bool _isFullyRepaired) {
		Debug.Log ("weaponSys: " + gridPos.X + ", " + gridPos.Y);

		if (_isFullyDamaged) {
			if (isPowered) {
				ReceivePowerUpdate (false);
			}

			pwrMngr.ApplyHealthState (systemType, powerReq, isPowered, this);
		} 

		if (_isFullyRepaired) {
			pwrMngr.ApplyHealthState (systemType, -powerReq, isPowered, this);
		}
	}



	//to all
	public void ReceivePowerUpdate (bool _isPowered) {
		if (isOrigin) {
			if (isPowered) {
				SystemScript _sysScr = systemScr.GetOriginObj ().GetComponent <SystemScript> ();
				_sysScr.UpdatePowerState (false);
			} else {
				if (!hScr.IsFullyDamaged) {
					if (pwrMngr.EnoughPower (fullPwrReq)) {
						//try power up
						SystemScript _sysScr = systemScr.GetOriginObj ().GetComponent <SystemScript> ();
						_sysScr.UpdatePowerState (true);
					} else {
						Debug.LogError ("not enough power");
					}
				}
			}
		} else {
			weaponSysScr.ReceivePowerUpdate (_isPowered);
		}
	}

	public void UpdatePowerState (bool _isPowered) {
		//at this point we know theres enough power and can power down or up
		if (isPowered) {
			//try power down
			pwrMngr.PowerDistribution (systemType, -powerReq, this);
			pwrMngr.UpdateReactor (powerReq);

			//system specific

			isPowered = false;
		} else {
			pwrMngr.PowerDistribution (systemType, powerReq, this);

			//system specific

			isPowered = true;
		}

		systemScr.IsPowered = isPowered;
	}


	private WeaponSysScr GetOriginWeaponSys () {
		WeaponSysScr _weaponSysScr = systemScr.GetOriginObj ().GetComponent <WeaponSysScr> ();

		if (_weaponSysScr == null) {
			Debug.LogError ("_weaponSys == null!");
		}
	
		return _weaponSysScr;
	}


	public bool IsPowered () {
		return isPowered;
	}
}