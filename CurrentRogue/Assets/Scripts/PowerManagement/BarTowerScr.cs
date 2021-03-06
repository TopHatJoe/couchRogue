﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarTowerScr : MonoBehaviour 
{
	[SerializeField]
	private GameObject powerBar;
	private List <PowerBarScr> barList = new List <PowerBarScr> ();
	private int damage = 0;

	public void AddBars (int _amount) {
		for (int i = 0; i < _amount; i++) {
			GameObject _obj = (GameObject)Instantiate (powerBar, transform);
			PowerBarScr _bar = _obj.GetComponent <PowerBarScr> ();
			barList.Add (_bar);
			_bar.Recolour (Color.grey);
		}
	}

	public void UpdateUsage (int _usage) {
		for (int i = 0; i < barList.Count; i++) {
			barList [i].Recolour (Color.grey);
		}

		for (int i = barList.Count - 1; i >= barList.Count - damage; i--) {
			barList [i].Recolour (Color.red);
		}

		for (int i = 0; i < _usage; i++) {
			barList [i].Recolour (Color.green);
		}
	}

	public void UpdateDamage (int _amount) {
		damage += _amount;
	}
}