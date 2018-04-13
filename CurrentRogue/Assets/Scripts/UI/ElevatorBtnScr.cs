using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorBtnScr : MonoBehaviour
{
	[SerializeField]
	private int levelNum;


	public int PressButton () {
		Debug.Log (levelNum);

		return levelNum;
		//close the menu
		//move towards level
		//once reached, reenable crew movement
	}
}