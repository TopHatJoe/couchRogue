using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorBtnPanelScr : MonoBehaviour 
{
	[SerializeField]
	private int levelNum;


	public void PressButton () {
		Debug.Log (levelNum);

		//close the menu
		//move towards level
		//once reached, reenable crew movement
	}
}