using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemParentScr : MonoBehaviour {
	[SerializeField]
	private int sysType;
	[SerializeField]
	private int powerReq;

	private ShipPowerMngr pwrMngr;


	void Start () {
		ShipScript _ship = LevelManager.Instance.Ships [0].GetComponent <ShipScript> ();
		pwrMngr = _ship.GetComponent <ShipPowerMngr> ();
	}

	public void UpdatePowerState (bool _isPowered) {
		//Debug.Log ("blah blah blah mister freeman");
	}
}