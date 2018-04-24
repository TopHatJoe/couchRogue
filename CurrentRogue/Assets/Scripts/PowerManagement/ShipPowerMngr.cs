using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPowerMngr : MonoBehaviour 
{
	[SerializeField]
	private PowerPanelScr powerPanel;
	//[SerializeField]
	//private GameObject powerBar;


	//the maximum capacity of the system based on what the ship has installed
	private int[] maxCapacityArr = new int[4];
	//the capacity that is available when considering damages
	private int[] capacityArr = new int[4];
	//the amount of power invested
	private int[] powerArr = new int[4];


	//system setup adding capacities to systemType
	public void PowerSetup (int _sysType, int _amount) {
		maxCapacityArr [_sysType] += _amount;
		capacityArr [_sysType] = maxCapacityArr [_sysType];

		powerPanel.AddBars (_sysType, _amount);
		Debug.Log (_sysType + ", " + maxCapacityArr [_sysType]);
	}


	//system gets powered or unpowered
	public void PowerDistribution (int _sysType, int _amount) {
		if (powerArr [_sysType] + _amount >= 0) {
			if (powerArr [_sysType] + _amount <= capacityArr [_sysType]) {
				powerArr [_sysType] += _amount;

				powerPanel.UpdateUsage (_sysType, powerArr [_sysType]);
			} else {
				Debug.LogError ("fuller than full power!");
			}
		} else {
			Debug.LogError ("sub zero power!");
		}
	}


	public void ApplyHealthState (int _sysType, int _amount, bool _wasPowered) {
		//reactor
		if (_sysType == 0) {
			Debug.Log ("reactor capaity reduced by " + _amount);

		//shield
		} else if (_sysType == 1) {
			Debug.Log ("shield capaity reduced by " + _amount);

		//weapons
		} else if (_sysType == 2) {
			Debug.Log ("weapons capaity reduced by " + _amount);

		//engines
		} else if (_sysType == 3) {
			Debug.Log ("engine capaity reduced by " + _amount);
		}


		powerPanel.UpdateDamage (_sysType, _amount);
		if (_amount > 0 &&  _wasPowered) {
			//only if system was powered to begin with!
			//if () {
			PowerDistribution (_sysType, -_amount);
			//}
		} else {
			//to update the red bars
			PowerDistribution (_sysType, 0);
		}

		capacityArr [_sysType] -= _amount;
		Debug.Log (capacityArr [_sysType]);
	}
}