using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBarScript : MonoBehaviour 
{
	//public int Index { get; set; }
	private int barIndex;


	public void Setup (int _index) {
		barIndex = _index;
	}
}