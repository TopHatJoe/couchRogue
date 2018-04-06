using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineScript : MonoBehaviour, ISystem
{
	[SerializeField]
	private SystemScript systemScr;

	private Point gridPos;

	private int playerID;

	private ShipScript ship;

	[SerializeField]
	private int powerReq;

	[SerializeField]
	//the amount by which the evasiveness is hightened
	private int componentCapacity;

	private bool isPowered = false;
	public bool IsPowered { get { return isPowered; } }

	private int systemType = 2;

	private bool isDamaged = false;
	private bool isLocal = false;


	void Start () {
		Setup ();
	}

	private void Setup () {
		gridPos = systemScr.GridPos;
		playerID = gridPos.Z;

		ship = LevelManager.Instance.Ships [playerID].GetComponent <ShipScript> ();

		//ship.IncreaseEvasionChance (componentCapacity);
		if (NetManager.Instance != null) {
			if (playerID == NetManager.Instance.localPlayerID) {
				PowerManager.Instance.GetEngine (this);
				PowerManager.Instance.UpdateSystemCapacity (systemType, powerReq);
				isLocal = true;
			}
		}
	}

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