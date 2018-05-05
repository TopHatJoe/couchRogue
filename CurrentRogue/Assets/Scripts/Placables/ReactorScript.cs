using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorScript : MonoBehaviour, ISystem
{
	[SerializeField]
	//the amount of bars added by placing this.Reactor
	private int componentCapacity;
	private int fullCapacity;

	//maybe if only the localPlayer adds to capacity...
	private static int reactorCapacity = 0;
	public int ReactorCapacity { get { return reactorCapacity; } }
	private static int reactorUsage = 0;
	//public int ReactorUsage { get { return reactorUsage; } }

	//availablePower -> capacity - usage (updated whenever either one is changed, if negative, random system shutdown)
	private static int availablePower;

	private Point gridPos;
	private SystemScript sysScr;
	private ReactorScript originReactorScr;
	private bool isOrigin = false;

	private bool isLocalReact = false;

	//[SerializeField]
	private int systemType = 0;
	private ShipPowerMngr pwrMngr;

	private bool isPowered = false;
	private HealthScript hScr;
	//private SystemScript systemScr;




	void Start ()
	{
		sysScr = gameObject.GetComponent <SystemScript> ();
		gridPos = sysScr.GridPos;

		GameObject _ship = transform.parent.parent.parent.parent.gameObject;
		pwrMngr = _ship.GetComponent <ShipPowerMngr> ();
		hScr = gameObject.GetComponent <HealthScript> ();
		originReactorScr = GetOriginReactor ();

		originReactorScr.fullCapacity += componentCapacity;
		if (this == originReactorScr) {
			isOrigin = true;
		}

		//ReactorSetup ();

		pwrMngr.PowerSetup (systemType, componentCapacity);
	}

	/*
	void Update () {
		if (Input.GetKeyDown (KeyCode.Comma)) {
			Debug.Log ("capacity: " + reactorCapacity + ", reactorUsage: " + reactorUsage + "\navailablePower: " + availablePower);
		}

		//DEBUG -> needs better place 
		availablePower = reactorCapacity - reactorUsage;

		if (availablePower < 0) {
			if (PowerManager.Instance != null) {
				//RedirectPower (-availablePower);
				PowerManager.Instance.RandomSystemShutdown ();
			} else {
				Debug.LogError ("PowerManager == null");
			}
		}
	}


	private void ReactorSetup () {
		if (NetManager.Instance != null) {
			if (gridPos.Z == NetManager.Instance.localPlayerID) {
				//reactorCapacity += componentCapacity;
				isLocalReact = true;
				UpdateReactorCapacity (componentCapacity);

				//Debug.Log ("reactorCapacity: " + reactorCapacity);
			}
		}
	}


	public static bool DirectPower (int _amount) {
		if (reactorUsage + _amount <= reactorCapacity) {
			reactorUsage += _amount;
			//UpdateReactorUsage (-_amount);
			UpdateUI ();

			return true;
		} else {
			return false;
		}
	}


	public static bool RedirectPower (int _amount) {
		if (reactorUsage - _amount < 0) {
			return true;
		} else {
			reactorUsage -= _amount;
			//UpdateReactorUsage (_amount);
			UpdateUI ();

			return false;
		}
	}


	private void UpdateReactorCapacity (int _amount) {
		reactorCapacity += _amount;

		if (PowerManager.Instance != null) {
			PowerManager.Instance.UpdateReactorCapacity (_amount);
		} else {
			//Debug.LogError ("PowerManager == null");
		}
	}

	private static void UpdateUI () {
		if (PowerManager.Instance != null) {
			PowerManager.Instance.UpdatePowerDistribution (reactorCapacity - reactorUsage);
		}
	}


	void OnDestroy () {
		//Debug.Log ("waeeee");
		if (isLocalReact) {
			UpdateReactorCapacity (-componentCapacity);
		}
		//UpdateUI ();
	}


	public void UpdateHealth (int _hp) {
		Debug.Log ("ISystem/Reactor");

		/*
		//onDamage
		if (_hp <= 0 && !isDamaged) {
			isDamaged = true;

			Debug.Log ("ISystem/Engine Damaged");

			PowerManager.Instance.DamageSystem (systemType, -powerReq);
		}

		//onRepair
		if (_hp > 0 && isDamaged) {
			isDamaged = false;

			Debug.Log ("ISystem/Engine Repaired");

			PowerManager.Instance.DamageSystem (systemType, powerReq);
		}
		////
	}
	*/


	public void UpdateHealthState (bool _isFullyDamaged, bool _isFullyRepaired) {
		Debug.Log ("reactor: " + gridPos.X + ", " + gridPos.Y);

		if (_isFullyDamaged) {
			//PowerManager.Instance.DamageSystem (systemType, -powerReq);

			if (isPowered) {
				ReceivePowerUpdate (false);
			}

			pwrMngr.ApplyHealthState (systemType, componentCapacity, true, this);
		} 

		if (_isFullyRepaired) {
			//might cause reaktor bugs!
			//PowerManager.Instance.DamageSystem (systemType, powerReq);
			pwrMngr.ApplyHealthState (systemType, -componentCapacity, true, this);
		}

		//Debug.Log ("reaktor: ");
		//Debug.Log ("isFullyDamaged = " + _isFullyDamaged);
		//Debug.Log ("isFullyRepaired = " + _isFullyRepaired);
	}


	//to all
	public void ReceivePowerUpdate (bool _isPowered) {
		if (isOrigin) {
			if (isPowered) {
				//tryPowerDown
				if (pwrMngr.EnoughPower (fullCapacity)) {
					Debug.Log ("canPowerDown");
					SystemScript _sysScr = sysScr.GetOriginObj ().GetComponent <SystemScript> ();
					_sysScr.UpdatePowerState (_isPowered);
				} else {
					//shut down random system & call ReceivePowerUpdate again
					Debug.LogError ("cant shut down! initiating emergency shutdown");
					ShutDownRandomSystem (_isPowered);
				}
			} else {
				if (!hScr.IsFullyDamaged) {
					SystemScript _sysScr = sysScr.GetOriginObj ().GetComponent <SystemScript> ();
					_sysScr.UpdatePowerState (_isPowered);
				}
			}
		} else {
			originReactorScr.ReceivePowerUpdate (_isPowered);
		}
	}

	private void ShutDownRandomSystem (bool _isPowered) {
		List <ISystem> _iSysList = pwrMngr.ISysList;
		int _int = Random.Range (1, (_iSysList.Count - 1));
		_iSysList [_int].ReceivePowerUpdate (false);

		//call ReceivePowerUpdate again
		ReceivePowerUpdate (_isPowered);
	}

	public void UpdatePowerState (bool _isPowered) {
		Debug.Log ("heil reactor");

		if (isPowered) {
			//try power down

			//checks if theres enough free power for shut down
			//if (pwrMngr.EnoughPower (componentCapacity)) {
				//is done by the power check already
				//pwrMngr.UpdateReactor (-componentCapacity);

				//reactorCapacity -= componentCapacity;
				//Debug.Log ("reactor capacity 00: " + reactorCapacity);
				isPowered = false;
			//} else {
				//if there isn't, it shuts a random system down //<- this might prove to be tricky!
			//	Debug.LogError ("cant shut down! initiating emergency shutdown"); 
			//}

			/*
			List <ISystem> _iSysList = pwrMngr.ISysList;
			int _emergency = 0;
			while (_emergency < 100) {
				//checks if theres enough free power for shut down
				if (pwrMngr.EnoughPower (componentCapacity)) {
					//pwrMngr.PowerDistribution (systemType, -componentCapacity, this);
					pwrMngr.UpdateReactor (-componentCapacity);
					isPowered = false;
					break;
				} else {
					//if there isn't, it shuts a random system down //<- this might prove to be tricky!
					Debug.LogError ("cant shut down! initiating emergency shutdown"); 

					int _int = Random.Range (1, (_iSysList.Count - 1));
					_iSysList [_int].ReceivePowerUpdate (false);
					_emergency++;
				}
			}
			*/
		} else {
			//if (!hScr.IsFullyDamaged) {
				//try power up
				//pwrMngr.PowerDistribution (systemType, componentCapacity, this);
				pwrMngr.UpdateReactor (componentCapacity);
				//reactorCapacity += componentCapacity;
				//Debug.Log ("reactor capacity 01: " + reactorCapacity);
				isPowered = true;
			//}

			//Debug.Log ("is fully damaged: " + hScr.IsFullyDamaged);
		}

		//isPowered = !isPowered;
		Debug.Log ("reactor powered = " + isPowered);

		//systemScr.IsPowered = isPowered;

		//isPowered = !isPowered;
		//Debug.LogError ("engine powered = " + isPowered);
	}

	private ReactorScript GetOriginReactor () {
		ReactorScript _reactScr = sysScr.GetOriginObj ().GetComponent <ReactorScript> ();
		return _reactScr;
	}


	/*
	public void OnDamage () {
		Debug.Log ("ISystem/Reactor");
	}
	*/

	/*
	private static void UpdateReactorUsage (int _amount) {
		reactorUsage += _amount;

		if (PowerManager.Instance != null) {
			PowerManager.Instance.UpdatePowerDistribution (_amount);
		} else {
			Debug.LogError ("PowerManager == null");
		}
	}
	*/
}