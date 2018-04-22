using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorScript : MonoBehaviour, ISystem
{
	[SerializeField]
	//the amount of bars added by placing this.Reactor
	private int componentCapacity;

	//maybe if only the localPlayer adds to capacity...
	private static int reactorCapacity = 0;
	public int ReactorCapacity { get { return reactorCapacity; } }
	private static int reactorUsage = 0;
	//public int ReactorUsage { get { return reactorUsage; } }

	//availablePower -> capacity - usage (updated whenever either one is changed, if negative, random system shutdown)
	private static int availablePower;

	private Point gridPos;
	private SystemScript sysScr;

	private bool isLocalReact = false;

	//[SerializeField]
	private int systemType = 0;
	private ShipPowerMngr pwrMngr;



	void Start ()
	{
		sysScr = gameObject.GetComponent <SystemScript> ();
		gridPos = sysScr.GridPos;

		GameObject _ship = transform.parent.parent.parent.parent.gameObject;
		pwrMngr = _ship.GetComponent <ShipPowerMngr> ();

		ReactorSetup ();
	}


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
		*/
	}


	public void UpdateHealthState (bool _isFullyDamaged, bool _isFullyRepaired) {
		Debug.Log ("reactor: " + gridPos.X + ", " + gridPos.Y);

		if (_isFullyDamaged) {
			//PowerManager.Instance.DamageSystem (systemType, -powerReq);
			pwrMngr.ApplyHealthState (systemType, componentCapacity);
		} 

		if (_isFullyRepaired) {
			//PowerManager.Instance.DamageSystem (systemType, powerReq);
			pwrMngr.ApplyHealthState (systemType, -componentCapacity);
		}

		//Debug.Log ("reaktor: ");
		//Debug.Log ("isFullyDamaged = " + _isFullyDamaged);
		//Debug.Log ("isFullyRepaired = " + _isFullyRepaired);
	}


	//to all
	public void ReceivePowerUpdate (bool _isPowered) {
		
	}

	public void UpdatePowerState (bool _isPowered) {
		//isPowered = !isPowered;
		//Debug.LogError ("engine powered = " + isPowered);
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