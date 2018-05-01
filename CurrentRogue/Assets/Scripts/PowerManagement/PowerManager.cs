using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PowerManager : Singleton <PowerManager>
{
	[SerializeField]
	private GameObject overallPower;
	private List <GameObject> overallBarList = new List<GameObject> ();

	//[SerializeField]
	//private GameObject[] systemPower;
	//[SerializeField]
	//private int[] powerUsage;

	[SerializeField]
	private GameObject[] sysPowerBtns;

	//private List <GameObject> shieldBarList = new List<GameObject> ();

	//[SerializeField]
	//private GameObject weaponPower;
	//private List <GameObject> weaponBarList = new List<GameObject> ();


	[SerializeField]
	private GameObject powerBar;

	private int reactorCapacity;
	private int currentReactorCapacity = 0;
	//availablePower -> capacity - usage


	/*
	//for gun by btn
	void Update () {
		if (Input.GetButtonDown ("Gun0")) {
			if (Input.GetKey (KeyCode.LeftShift)) {
				//unpower corresponding gun //-> value = false;
				Debug.Log ("DERP");
				sysPowerBtns [1].GetComponent <PowerBtnScript> ().KeyWeapon (0, false);
			} else {
				//power corresponding gun //or target//not //-> value = true
				sysPowerBtns [1].GetComponent <PowerBtnScript> ().KeyWeapon (0, true);
			}
		} else if (Input.GetButtonDown ("Gun1")) {
			if (Input.GetKey (KeyCode.LeftShift)) {
				//unpower corresponding gun //-> value = false;
				sysPowerBtns [1].GetComponent <PowerBtnScript> ().KeyWeapon (1, false);
			} else {
				//power corresponding gun //or target//not //-> value = true
				sysPowerBtns [1].GetComponent <PowerBtnScript> ().KeyWeapon (1, true);
			}
		} else if (Input.GetButtonDown ("Gun2")) {
			if (Input.GetKey (KeyCode.LeftShift)) {
				//unpower corresponding gun //-> value = false;
				sysPowerBtns [1].GetComponent <PowerBtnScript> ().KeyWeapon (2, false);
			} else {
				//power corresponding gun //or target//not //-> value = true
				sysPowerBtns [1].GetComponent <PowerBtnScript> ().KeyWeapon (2, true);
			}
		} else if (Input.GetButtonDown ("Gun3")) {
			if (Input.GetKey (KeyCode.LeftShift)) {
				//unpower corresponding gun //-> value = false;
				sysPowerBtns [1].GetComponent <PowerBtnScript> ().KeyWeapon (3, false);
			} else {
				//power corresponding gun //or target//not //-> value = true
				sysPowerBtns [1].GetComponent <PowerBtnScript> ().KeyWeapon (3, true);
			}
		}
	}


	public void UpdateReactorCapacity (int _amount) {
		reactorCapacity += _amount;

		if (_amount > 0) {
			for (int i = 0; i < _amount; i++) {
				//place Btn
				GameObject _obj = (GameObject) Instantiate (powerBar, overallPower.transform);
				overallBarList.Add (_obj);
				_obj.GetComponent <Image> ().color = Color.green;
				currentReactorCapacity++;
			}
		} else if (reactorCapacity - _amount < 0) {
			Debug.LogError ("to much power lost!");
		} else if (_amount < 0) {
			Debug.Log ("reducing");

			for (int i = 0; i < Mathf.Abs (_amount); i++) {
				//deactivate btn
				int _index = (overallBarList.Count - i) - 1;

				GameObject _obj = overallBarList [_index]; 
				overallBarList.Remove (_obj);
				Destroy (_obj);

				//overallBarList.Remove (overallBarList [_index]);
				//Destroy (overallBarList [_index]);

				currentReactorCapacity--;
			}
		}
	}

	public void UpdatePowerDistribution (int _amount) {
		for (int i = 0; i < overallBarList.Count; i++) {
			overallBarList [i].GetComponent <Image> ().color = Color.grey;
		}

		for (int i = 0; i < _amount; i++) {
			overallBarList [i].GetComponent <Image> ().color = Color.green;
		}

		/*
		if (_amount > 0) {
			for (int i = currentReactorCapacity; i < (currentReactorCapacity + _amount); i++) {
				//place Btn
				//GameObject _obj = (GameObject) Instantiate (powerBar, overallPower.transform);
				//overallBarList.Add (_obj);
				Debug.Log (i);
				overallBarList [i].GetComponent <Image> ().color = Color.green;


			}

			currentReactorCapacity += _amount;

		} else if (reactorCapacity - _amount < 0) {
			Debug.LogError ("to much power lost!");
		} else if (_amount < 0) {
			for (int i = currentReactorCapacity; i > (currentReactorCapacity + _amount); i--) {
				//current -> current - amount; i--

				overallBarList [i].GetComponent <Image> ().color = Color.grey;
			}

			currentReactorCapacity += _amount;
		}
		*/
	}

	/*
	public void UpdatePowerDistribution2 (int _amount, int _sysType) {
		for (int i = 0; i < _amount; i++) {
			GameObject _obj = (GameObject) Instantiate (powerBar, systemPower [_sysType].transform);
			_obj.GetComponent <Image> ().color = Color.grey;
		}
	}
	////

	public void UpdateSystemCapacity (int _sysType, int _amount) {
		sysPowerBtns [_sysType].GetComponent <PowerBtnScript> ().IncreaseMaxCapacity (_amount);

		/*
		for (int i = 0; i < _amount; i++) {
			GameObject _obj = (GameObject) Instantiate (powerBar, systemPower [_sysType].transform);
			_obj.GetComponent <Image> ().color = Color.grey;
		}
		////
	}

	public void UpdatePowerByBtn (int _sysType) {
		//GameObject _obj = (GameObject) Instantiate (powerBar, systemPower [_sysType].transform);
	}

	public void RoutePower (int _sysType, int _amount) {
		//do they do chunks themselves
		sysPowerBtns [_sysType].GetComponent <PowerBtnScript> ().UpdateBars (_amount);

		/*
		powerUsage [_sysType] += _amount;

		for (int i = 0; i < overallBarList.Count; i++) {
			overallBarList [i].GetComponent <Image> ().color = Color.grey;
		}

		for (int i = 0; i < _amount; i++) {
			overallBarList [i].GetComponent <Image> ().color = Color.green;
		}
		////
	}

	public void RandomSystemShutdown () {
		//actually 0 - number of systems //actually actually number of powerBtns
		int _x = Random.Range (0, sysPowerBtns.Length - 1);
		Debug.Log (_x + ", " + (sysPowerBtns.Length - 1)); 
		//RoutePower (_x, _amount);
		sysPowerBtns[_x].GetComponent <PowerBtnScript> ().RemoteShutdown ();
	}



	//updateSystemHealth would be more fitting don't ya think?
	public void DamageSystem (int _sysType, int _amount) {
		//reactor = 0, shield, gun, engine 
		sysPowerBtns [_sysType].GetComponent <PowerBtnScript> ().UpdateCurrentCapacity (_amount);
	}




	public void GetGunBtn (GunBtnScr _gunBtn) {
		sysPowerBtns [1].GetComponent <PowerBtnScript> ().GetGunBtns (_gunBtn);
	}

	public void GetShield (ShieldScript _shield) {
		sysPowerBtns [0].GetComponent <PowerBtnScript> ().GetShield (_shield);
	}

	public void GetEngine (EngineScript _engine) {
		sysPowerBtns [2].GetComponent <PowerBtnScript> ().GetEngines (_engine);
	}

	public bool HasCapacity (int _sysType, int _amount) {
		if (sysPowerBtns [_sysType].GetComponent <PowerBtnScript> ().HasCapacity (_amount)) {
			return true;
		} else {
			return false;
		}
	}

	*/
