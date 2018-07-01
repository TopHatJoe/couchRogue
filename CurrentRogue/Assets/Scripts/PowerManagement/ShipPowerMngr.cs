using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPowerMngr : MonoBehaviour 
{
	[SerializeField]
	private PowerPanelScr powerPanel;
	//[SerializeField]
	//private GameObject powerBar;

	[SerializeField]
	private int shipID;

	//the maximum capacity of the system based on what the ship has installed
	private int[] maxCapacityArr = new int[6];
	//the capacity that is available when considering damages
	private int[] capacityArr = new int[6];
	//the amount of power invested
	private int[] powerArr = new int[6];

    //classic -> power by btn //which systems are up, whuch aren't?
    private int[] pwrdSystemsCount = new int[6];

	private List <ISystem> iSysList = new List<ISystem> ();
	public List <ISystem> ISysList { get { return iSysList; } }

	private List <WeaponScript> weaponList = new List <WeaponScript> ();
	//public List <WeaponScript> WeaponList { get { return weaponList; } set { weaponList = value; } }

	//keeps track of that shit
	private int wpnPwrUsage;


    //lists of all systems on ship //not very elegant really
    private List<SystemScript> reactorList = new List<SystemScript>();
    private List<SystemScript> shieldList = new List<SystemScript>();
    private List<SystemScript> weaponSysList = new List<SystemScript>();
    private List<SystemScript> engineList = new List<SystemScript>();
    private List<SystemScript> medbayList = new List<SystemScript>();
    private List<SystemScript> teleporterList = new List<SystemScript>();



	void Start () {
		if (NetManager.Instance.localPlayerID == shipID) {
			powerPanel.gameObject.SetActive (true);
			//Debug.LogError ("da same");
		}

		//Debug.LogError ("ship: " + shipID + ", localPlayer: " + NetManager.Instance.localPlayerID);
	}


	//system setup adding capacities to systemType
	public void PowerSetup (int _sysType, int _amount) {
		maxCapacityArr [_sysType] += _amount;
		capacityArr [_sysType] = maxCapacityArr [_sysType];

		powerPanel.AddBars (_sysType, _amount);
		//Debug.Log (_sysType + ", " + maxCapacityArr [_sysType]);

        /*
        if (_sysType == 0) {
            Debug.Log("reactor added to list");
            reactorList.Add(_iSys);
        }
        */
	}

    public void AddToSysScrList (int _sysType, SystemScript _sysScr) {
        if (_sysType == 0) {
            //Debug.Log("reactor added to list");
            reactorList.Add(_sysScr);
        } else if (_sysType == 1) {
            shieldList.Add(_sysScr);
        } else if (_sysType == 2) {
            weaponSysList.Add(_sysScr);
        } else if (_sysType == 3) {
            engineList.Add(_sysScr);
        } else if (_sysType == 4) {
            medbayList.Add(_sysScr);
        } else if (_sysType == 5) {
            teleporterList.Add(_sysScr);
        }
    } 


	//system gets powered or unpowered
	public void PowerDistribution (int _sysType, int _amount, ISystem _iSys) {
		//Debug.LogError ("powerDist");

		//if reactor has enough power to give //do that seperately?
		//if (powerArr [0] - _amount >= 0) {
		if (powerArr [_sysType] + _amount >= 0) {
			if (powerArr [_sysType] + _amount <= capacityArr [_sysType]) {
				powerArr [_sysType] += _amount;


				//power down weapons if not enough power
				if (_sysType == 2) {
					int _emergencyCounter = 0;
					while (wpnPwrUsage > powerArr [2]) {
						//makes shit slitly predictable, but it's suuuper hard. is it reliable though?
						int _i = (weaponList.Count - 1); //Random.Range (0, weaponList.Count);
						Debug.LogError ("hi");
						//weaponList [_i].IsPowered = false;
						weaponList [_i].ReceiveHandleCharge (false);

						//weaponList.Remove (weaponList [_i]);

						if (_emergencyCounter > 100) {
							Debug.LogError ("while issue!");
							break;
						}
					}


					/*
					int _emergencyCounter = 0;
					while (wpnPwrUsage > powerArr [_sysType]) {
						//Debug.LogError ("shut guns down!");
						int _i = Random.Range (0, weaponList.Count);
						weaponList [_i].IsPowered = false;
						weaponList.Remove (weaponList [_i]);
						_emergencyCounter++;

						if (weaponList.Count <= 0) {
							Debug.LogError ("running out of list");
						}

						Debug.LogError ("hi");
						if (wpnPwrUsage <= powerArr [_sysType]) {
							break;
						}

						if (_emergencyCounter > 100) {
							Debug.LogError ("while issue!");
							break;
						}
					}
					*/
				}




				powerPanel.UpdateUsage (_sysType, powerArr [_sysType]);

				if (_amount < 0) {
					iSysList.Remove (_iSys);
				} else {
					iSysList.Add (_iSys);
				}
			} else {
				Debug.LogError ("fuller than full power!");
			}
		} else {
			Debug.LogError ("sub zero power!");
		}

		//} else {
		//	Debug.LogError ("not enough power dipshit!");
		//}
	}

	public void UpdateReactor (int _amount) {
		//Debug.LogError ("power: " + powerArr [0] + " + " + _amount + " = " + (powerArr [0] + _amount));

		if (powerArr [0] + _amount >= 0) {
			if (powerArr [0] + _amount <= capacityArr [0]) {
				powerArr [0] += _amount;

				powerPanel.UpdateUsage (0, powerArr [0]);
			} else {
				Debug.LogError ("fuller than full power!");
			}
		} else {
			Debug.LogError ("sub zero power!");
		}
	}

	public bool EnoughPower (int _amount) {
		if (powerArr [0] - _amount >= 0) {
			//reactor reaction
			//PowerDistribution (0, -_amount);
			UpdateReactor (-_amount);
			return true;
		} else {
			return false;
		}
	}


	public void ApplyHealthState (int _sysType, int _amount, bool _wasPowered, ISystem _iSys) {
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
		PowerDistribution (_sysType, 0, _iSys);

		/*
		if (_amount > 0 &&  _wasPowered) {
			//only if system was powered to begin with!
			//if () {
			PowerDistribution (_sysType, -_amount, _iSys);
			//}
		} else {
			//to update the red bars
			PowerDistribution (_sysType, 0, _iSys);
		}

		capacityArr [_sysType] -= _amount;
		Debug.Log (capacityArr [_sysType]);
		*/
	}


	//has the weapon sys enough available power to power weapon?
	public bool EnoughWeaponSysPower (int _amount) {
		if ((wpnPwrUsage + _amount) <= powerArr [2]) {
			Debug.Log ("weapon power available. powerArr: " + powerArr [2]);
			return true;
		} else {
			Debug.LogError ("not enough weapon power!");
			return false;
		}
	}

    public void HandleWeaponPower (int _amount, WeaponScript _weapon) {
		if (_amount < 0) {
			//remove
			weaponList.Remove (_weapon);
            //return (-1);
		} else { //if (_amount > 0) {
                 //add
            weaponList.Add(_weapon);
            //return (weaponList.Count - 1);
		}

		/* 
		else {
			Debug.LogError ("powerReq = " + _amount + "!");
		}
		*/ 


		wpnPwrUsage += _amount;

		Debug.LogError ("available pwr: " + powerArr [2] + ", usedPwr: " + wpnPwrUsage);

		if (wpnPwrUsage > powerArr [2]) {
			Debug.LogError ("something in weapon power is fuuuuckd!");


			Debug.LogError ("SHUT WEAPON DOWN DAMMIT!");

			//power down random weapon //not here
		} else if (wpnPwrUsage < 0) {
			Debug.LogError ("sub zero weapon power!");
		}
	}



    public void PowerByBtn (int _sysType, bool _powerUp) {
        //Debug.Log("systype: " + _sysType);
        List<SystemScript> _sysList = GetSysScrList(_sysType);
        Debug.LogError(_sysList.Count);

        //if the player tries to power it up //make it loop trhough all to avoid dmg issues
        if (_powerUp) {
            //if (_sysType == 0) {
            for (int i = 0; i < _sysList.Count; i++) {
                //SystemScript _sysScr = reactorList[i];
                //reactorList[pwrdSystemsCount[_sysType]].SyncPowerUpdate(true);

                if (!_sysList [i].HScr.IsFullyDamaged) {
                    if (!_sysList[i].SysIsPowered ()) {
                        Debug.LogError(i + ", " + _sysList[i].SysIsPowered ());
                        _sysList[i].SyncPowerUpdate(true);
                        break;
                    }
                }
            }

            /*
            if (pwrdSystemsCount[_sysType] < reactorList.Count) {
                //Debug.Log("reactor added to list");
                reactorList[pwrdSystemsCount[_sysType]].SyncPowerUpdate(true);
                pwrdSystemsCount[_sysType]++;

                //Debug.LogError("pwrdSysCount: " + pwrdSystemsCount[_sysType]);
            }
            */
           // }
        } else {
            //if (_sysType == 0) {
            for (int i = (_sysList.Count -1); i >= 0; i--)
            {
                //SystemScript _sysScr = reactorList[i];
                //reactorList[pwrdSystemsCount[_sysType]].SyncPowerUpdate(true);

                if (_sysList[i].SysIsPowered()) {
                    _sysList[i].SyncPowerUpdate(false);
                    break;
                }
            }

            /*
            if (pwrdSystemsCount[_sysType] > 0) {
               // Debug.Log("reactor added to list");
                pwrdSystemsCount[_sysType]--;
                reactorList[pwrdSystemsCount[_sysType]].SyncPowerUpdate(false);

                //Debug.LogError("pwrdSysCount: " + pwrdSystemsCount[_sysType]);
            }
            */
            //}
        }
    }


    private List <SystemScript> GetSysScrList (int _sysType) {
        Debug.LogError("sysType: " + _sysType);
        if (_sysType == 0) {
            return reactorList;
        } else if (_sysType == 1) {
            return shieldList;
        } else if (_sysType == 2) {
            return weaponSysList;
        } else if (_sysType == 3) {
            return engineList;
        } else if (_sysType == 4) {
            return medbayList;
        } else if (_sysType == 5) {
            return teleporterList;
        } else {
            Debug.LogError("no list!");
            return null;
        }
    }
}