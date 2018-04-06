using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScript : MonoBehaviour 
{
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
}