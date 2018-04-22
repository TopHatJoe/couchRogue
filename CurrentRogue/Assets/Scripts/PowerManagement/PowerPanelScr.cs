using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPanelScr : MonoBehaviour 
{
	[SerializeField]
	private GameObject powerBar;
	[SerializeField]
	private BarTowerScr [] barTowerArr;
	//private int [] arrLengths = new int[4];
	//[SerializeField]
	//private 


	public void AddBars (int _sysType, int _amount) {
		Debug.Log ("oi Panel");
		barTowerArr [_sysType].AddBars (_amount);

		/*
		for (int i = 0; i < _amount; i++) {
			GameObject _obj = (GameObject)Instantiate (powerBar, barTowerArr [_sysType].transform);
			arrLengths [_sysType] += _amount;
			_obj.GetComponent <PowerBarScr> ().Recolour (Color.grey);
		}
		//btnArr[_sysType]
		*/
	}

	public void UpdateUsage (int _sysType, int _usage) {
		barTowerArr [_sysType].UpdateUsage (_usage);
	}
}