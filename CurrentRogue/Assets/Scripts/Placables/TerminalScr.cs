using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalScr : MonoBehaviour
{
	private ShipScript ship;
	private ShipScript targetedShip;

	private WeaponSysScr weaponSysScr;
    private TeleporterScr teleporterScr;


	private WeaponScript currentWeapon;
	private int currentWeaponID = 0;
	public int CurrentWeaponID { get { return currentWeaponID; } }
	private int targetedShipID = 0;
	private Camera shipCam;

	private GameObject[] shipArr;
	private int numOfShips;

	private Color userColor;

	private bool isUsed = false;
	public bool IsUsed { get { return isUsed; } }

	private CouchCrewScript userScr;

	private Point gridPos;
	public Point GridPos { get { return gridPos; } }

    private bool isWeaponTerminal = false;
    public bool IsWeaponTerminal { get { return isWeaponTerminal; } }
    private bool isTeleporterTerminal = false;



	void Start () {
		Setup ();
	}

	private void Setup () {
		SubSystemScript _subSysScr = gameObject.GetComponent <SubSystemScript> ();
		gridPos = _subSysScr.GridPos;
		ship = LevelManager.Instance.Ships [_subSysScr.GridPos.Z].GetComponent <ShipScript> ();	


        GameObject _sysObj = transform.parent.parent.GetChild(1).gameObject;
        if (_sysObj.transform.childCount > 0) {
            if (_sysObj.transform.GetChild(0).GetComponent<WeaponSysScr>() != null) {
                weaponSysScr = _sysObj.transform.GetChild(0).GetComponent<WeaponSysScr>();
                isWeaponTerminal = true;
            
            } else if (_sysObj.transform.GetChild(0).GetComponent<TeleporterScr>() != null) {
                teleporterScr = _sysObj.transform.GetChild(0).GetComponent<TeleporterScr>();
                isTeleporterTerminal = true;
           
            } else {
                Debug.LogError("/*weaponSysScr*/ someSysScr == null");
            }
        }
	}


	public void UseTerminal (CouchCrewScript _user) {
		//Debug.LogError ("hello there mr. crew!");
		userScr = _user;


		isUsed = true;

		shipArr = LevelManager.Instance.Ships;
		numOfShips = NetManager.Instance.NumOfPlayers;
		//Debug.LogError ("numOfShips: " + numOfShips);

		userColor = _user.CrewColor;
		//gun terminal behaviour

			//switch to full ship view

		//Camera.main.enabled = true;

		SetCrewCam ();
        /*
		ship = LevelManager.Instance.Ships[targetedShipID].GetComponent <ShipScript> ();
		Camera _shipCam = ship.ShipCam;
		if (_shipCam == null) {
			Debug.Log ("no cam!");
		} else {
			//_shipCam.gameObject.SetActive (true);
			_user.SetCrewCamValues (ship, true);
		}
		*/



        /*
		GameObject _sysObj = transform.parent.parent.GetChild (1).gameObject;
		if (_sysObj.transform.childCount > 0) {
			if (_sysObj.transform.GetChild (0).GetComponent <WeaponSysScr> () != null) {
				weaponSysScr = _sysObj.transform.GetChild (0).GetComponent <WeaponSysScr> ();
			} else {
				Debug.Log ("weaponSysScr == null");
			}
		}
        */


        /*
		//should be outsourced to weaponTerminal subScr?
		if (ship.WeaponList.Count > currentWeaponID) {
			currentWeapon = ship.WeaponList [currentWeaponID];
		}

		if (currentWeapon != null) {
			currentWeapon.IsPowered = true;
		}
		*/

        if (isWeaponTerminal)
        {
            GetWeapon();
            GiveTerminalReference(_user);

            //when terminal is used -> sys is powered. 
            //for weaponScr that means, that you may distribute any available weaponPower
            //players can only do that by using a terminal.
            //-> there may be weaponSys without terminals, but you can't interface with it, just boost the available power


            //enabe cursor //-> where does it start?

            //while in this view, deactivate couchCrew movement

            //o exits the terminal

            SwapWeapon(1);
        } else if (isTeleporterTerminal) {
            GiveTerminalReference(_user);
        }
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

		isUsed = false;	

		userScr = null;

		//_user.SetCrewCamValues (ship, false);

		//whats the purpose of this?
		if (currentWeapon != null) {
			currentWeapon.HandleOutline (false, userColor);
			currentWeapon.isUsedByCrew = false;
		
			//wait what? ill comment this out. 270518
			//currentWeapon.IsPowered = true;
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
				Debug.Log ("all weapons in use!");
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


			/*
			if (!currentWeapon.IsPowered) {
				currentWeapon.IsPowered = true;
			}
			*/

		}
	}

    //gives terminal reference to cursor
	private void GiveTerminalReference (CouchCrewScript _user) {
		_user.GiveTerminalReference (this);
	}

	public void SwapTargetedShip (int _amount) {
		targetedShipID += _amount;

		if (targetedShipID < 0) {
			targetedShipID = numOfShips - 1;
		} else if (targetedShipID >= numOfShips) {
			targetedShipID = 0;
		}

		Debug.LogError ("targeted ship: " + targetedShipID);



		SetCrewCam ();
	}

	private void SetCrewCam () {
		targetedShip = shipArr [targetedShipID].GetComponent <ShipScript> ();
		shipCam = targetedShip.ShipCam;
		if (shipCam == null) {
			Debug.Log ("no cam!");
		} else {
			//_shipCam.gameObject.SetActive (true);
			userScr.SetCrewCamValues (targetedShip, true, targetedShipID);
		}
	}

	public void TryPowerWeapon (bool _isPowered) {
        //Debug.LogError (currentWeapon.IsPowered);
        if (isWeaponTerminal) {
            if (currentWeapon.IsPowered) {
                currentWeapon.IsPowered = false;
            }
            else {
                currentWeapon.IsPowered = true;
            }
        }

		//bool _bool = currentWeapon.IsPowered;
		//currentWeapon.IsPowered = !_bool;
		//Debug.LogError (currentWeapon.IsPowered);
	}


    public void Teleport (Point _point) {
        if (isTeleporterTerminal) {
            Debug.LogError("teleportin to: " + _point.X + ", " + _point.Y);
            teleporterScr.Teleport(_point);
       
        } else {
            Debug.LogError("i aint portin' nowhere!");
        }
    }
}