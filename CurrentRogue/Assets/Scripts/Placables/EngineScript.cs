using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineScript : MonoBehaviour, ISystem
{
	[SerializeField]
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
	public bool IsPowered { get { return isPowered; } }

	private int systemType = 3;

	//private bool isDamaged = false;
	//private bool isLocal = false;

	private ShipPowerMngr pwrMngr;
	private EngineScript originEngScr;
	private bool isOrigin = false;


	void Start () {
		Setup ();
	}

	private void Setup () {
		gridPos = systemScr.GridPos;
		playerID = gridPos.Z;

		ship = LevelManager.Instance.Ships [playerID].GetComponent <ShipScript> ();
		pwrMngr = ship.GetComponent <ShipPowerMngr> ();

		hScr = systemScr.GetOriginObj ().GetComponent <HealthScript> ();

		//ship.IncreaseEvasionChance (componentCapacity);

		pwrMngr.PowerSetup (systemType, powerReq);

		originEngScr = GetOriginEngine ();
		if (this == originEngScr) {
			isOrigin = true;
		}

		originEngScr.fullPwrReq += powerReq;


		/* 220418
		if (NetManager.Instance != null) {
			if (playerID == NetManager.Instance.localPlayerID) {
				PowerManager.Instance.GetEngine (this);
				PowerManager.Instance.UpdateSystemCapacity (systemType, powerReq);
				isLocal = true;
			}
		}
		*/
	}

	/*
	public void TryPowerUp () {
		if (PowerManager.Instance.HasCapacity (systemType, powerReq)) {
			if (ReactorScript.DirectPower (powerReq)) {
				//sync powerstate here
				if (NetManager.Instance != null) {
					NetManager.Instance.SyncPowerState (gridPos, systemType, true);
				} else {
					ship.IncreaseEvasionChance (componentCapacity);
				}

				PowerManager.Instance.RoutePower (systemType, powerReq);

				isPowered = true;

				//ship.IncreaseEvasionChance (componentCapacity);
			} else {
				Debug.Log ("not enough power");
			}
		}
	}

	public void TryPowerDown () {
		if (isPowered) {
			//frees the power

			//sync powerstate here
			if (NetManager.Instance != null) {
				NetManager.Instance.SyncPowerState (gridPos, systemType, false);
			} else {
				ship.IncreaseEvasionChance (-componentCapacity);
			}

			ReactorScript.RedirectPower (powerReq);

			isPowered = false;
			//ship.IncreaseEvasionChance (-componentCapacity);

			PowerManager.Instance.RoutePower (systemType, -powerReq);
		} else {
			Debug.Log ("already powered down");
		}
	}
	*/


	public void SyncedPower (bool _isPowered) {
		isPowered = _isPowered;

		if (isPowered) {
			ship.IncreaseEvasionChance (componentCapacity);
		} else {
			ship.IncreaseEvasionChance (-componentCapacity);
		}
	}

	/*
	public void OnDamage () {
		Debug.Log ("ISystem/Engine");

		PowerManager.Instance.DamageSystem (systemType, -powerReq);
	}

	//still needs to be implemented as ISystem
	public void OnRepair () {
		
	}
	*/

	/*
	public void UpdateHealth (int _hp) {
		//onDamage
		if (_hp <= 0 && !isDamaged) {
			isDamaged = true;

			Debug.Log ("ISystem/Engine Damaged");

			if (isLocal) {
				PowerManager.Instance.DamageSystem (systemType, -powerReq);
			}
		}

		//onRepair
		if (_hp > 0 && isDamaged) {
			isDamaged = false;

			Debug.Log ("ISystem/Engine Repaired");

			if (isLocal) {
				PowerManager.Instance.DamageSystem (systemType, powerReq);
			}
		}
	}
	*/

	public void UpdateHealthState (bool _isFullyDamaged, bool _isFullyRepaired) {
		Debug.Log ("engine: " + gridPos.X + ", " + gridPos.Y);

		if (_isFullyDamaged) {
			//PowerManager.Instance.DamageSystem (systemType, -powerReq);

			if (isPowered) {
				ReceivePowerUpdate (false);
			}

			pwrMngr.ApplyHealthState (systemType, powerReq, isPowered, this);
			//isPowered = false;
		} 

		if (_isFullyRepaired) {
			//PowerManager.Instance.DamageSystem (systemType, powerReq);
			pwrMngr.ApplyHealthState (systemType, -powerReq, isPowered, this);
		}

		//Debug.Log ("isFullyDamaged = " + _isFullyDamaged);
		//Debug.Log ("isFullyRepaired = " + _isFullyRepaired);
	}



	//to all
	public void ReceivePowerUpdate (bool _isPowered) {
		if (isOrigin) {
			if (isPowered) {
				SystemScript _sysScr = systemScr.GetOriginObj ().GetComponent <SystemScript> ();
				_sysScr.UpdatePowerState (false);
			} else {
				if (!hScr.IsFullyDamaged) {
					//Debug.Log ("req: " + powerReq);
					//Debug.Log ("full req: " + originEngScr.fullPwrReq);

					if (pwrMngr.EnoughPower (fullPwrReq)) {
						//try power up
						SystemScript _sysScr = systemScr.GetOriginObj ().GetComponent <SystemScript> ();
						_sysScr.UpdatePowerState (true);

						//pwrMngr.PowerDistribution (systemType, powerReq, this);
						//isPowered = true;
					} else {
						Debug.LogError ("not enough power");
					}
				}
			}
		} else {
			originEngScr.ReceivePowerUpdate (_isPowered);
		}
	}

	public void UpdatePowerState (bool _isPowered) {
		//Debug.LogError ("PowerUpdate!!!");
		//at this point we know theres enough power and can power down or up
		if (isPowered) {
			//try power down
			pwrMngr.PowerDistribution (systemType, -powerReq, this);
			pwrMngr.UpdateReactor (powerReq);

			ship.IncreaseEvasionChance (-componentCapacity);

			isPowered = false;
		} else {
			//Debug.Log ("req: " + powerReq);
			//Debug.Log ("full req: " + originEngScr.fullPwrReq);

			pwrMngr.PowerDistribution (systemType, powerReq, this);

			ship.IncreaseEvasionChance (componentCapacity);

			isPowered = true;
		}


		/*
		if (isPowered) {
			//try power down
			pwrMngr.PowerDistribution (systemType, -powerReq, this);
			//updates available power
			//pwrMngr.PowerDistribution (0, powerReq);
			pwrMngr.UpdateReactor (powerReq);
			isPowered = false;
		} else {
			if (!hScr.IsFullyDamaged) {
				Debug.Log ("req: " + powerReq);
				Debug.Log ("full req: " + originEngScr.fullPwrReq);
				if (isOrigin) {
					if (pwrMngr.EnoughPower (fullPwrReq)) {
						//try power up
						pwrMngr.PowerDistribution (systemType, powerReq, this);
						isPowered = true;
					}
				} else {
					//stupid as fuuuuk! -> will always execute no matter what //right?
					Debug.Log ("ehhhhh");
					pwrMngr.PowerDistribution (systemType, powerReq, this);
					isPowered = true;
				}
			}

			Debug.Log ("is fully damaged: " + hScr.IsFullyDamaged);
		}
		*/

		//isPowered = !isPowered;
		//Debug.Log ("engine powered = " + isPowered);
	}


	private EngineScript GetOriginEngine () {
		EngineScript _engScr = systemScr.GetOriginObj ().GetComponent <EngineScript> ();
		return _engScr;
	}


	/*
	private void SyncPower (bool _isPowered) {
		//Vector3 _vect = new Vector3 (gridPos.X, gridPos.Y, gridPos.Z);

		NetManager.Instance.SyncPowerState (gridPos, 2, _isPowered);
	}
	*/
	/*
	public void TryPowerUp () {
		if (ReactorScript.DirectPower (powerReq)) {
			//Debug.Log ("powering up");

			//sync powerstate
			if (NetManager.Instance != null) {
				NetManager.Instance.SyncPowerState (weapon.GridPos, true);
			}

			//still needs to do this, since the btn statements cant be synced
			objImg.color = Color.white;
			isPowered = true;

			//sghalfefgyu
			PowerManager.Instance.RoutePower (1, powerReq);
		} else {
			Debug.Log ("not enough power");
		}
	}

	public void TryPowerDown () {
		if (isPowered) {
			//frees the power

			//sync powerstate
			if (NetManager.Instance != null) {
				NetManager.Instance.SyncPowerState (weapon.GridPos, false);
			}

			ReactorScript.RedirectPower (powerReq);
			isPowered = false;

			PowerManager.Instance.RoutePower (1, -powerReq);
		} else {
			Debug.Log ("already powered down");
		}
	}
	*/
}