using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorBtnPanelScr : MonoBehaviour 
{
	[SerializeField]
	private GameObject[] btnArr = new GameObject[8];

	/*
	void Start () {
		for (int i = 0; i < btnArr.Length; i++) {
			btnArr [i] = transform.GetChild (i).gameObject;
		}
	}
	*/


	public void SetBtns (ElevatorScript _elevator) {
		Debug.Log ("elevatro"); 

		for (int i = 0; i < btnArr.Length; i++) {
			btnArr [i].gameObject.SetActive (false);
		}

		List <int> _levels = _elevator.AccessIndexList;
		for (int i = 0; i < _levels.Count; i++) {
			btnArr [_levels [i] - 1].gameObject.SetActive (true);
		}
	}

	/*
	[SerializeField]
	private int levelNum;


	public int PressButton () {
		Debug.Log (levelNum);

		return levelNum;
		//close the menu
		//move towards level
		//once reached, reenable crew movement
	}
	*/
}