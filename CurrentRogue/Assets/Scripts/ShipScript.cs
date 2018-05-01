﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScript : MonoBehaviour 
{
	private ShieldScript shield;
	public ShieldScript Shield { get { if (shield == null) { GetShield (); } return shield; } }

	private int evasionChance = 0;
	public int EvasionChance { get { return evasionChance; } }


	public void RemoteUpdateEvasionChance (int _eC) {
		evasionChance = _eC;
	}

	public void IncreaseEvasionChance (int _amount) {
		evasionChance += _amount;

		Debug.Log (evasionChance + "% evasion");
		if (evasionChance < 0) {
			Debug.LogError ("Negative Evasion!? IMPOSSIBLE!!!");
		}
	}

	private void GetShield () {
		shield = transform.GetChild (6).GetComponent <ShieldScript> ();
		//int _int = shield.Power;
	}
}