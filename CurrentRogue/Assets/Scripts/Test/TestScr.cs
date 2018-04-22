using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScr : MonoBehaviour 
{
	[SerializeField]
	private GameObject obj;
	[SerializeField]
	private GameObject parentObj;

	void Start () {
		//GameObject _obj = (GameObject)Instantiate (obj);
		StartCoroutine (PlaceStuff ());
	}

	private IEnumerator PlaceStuff () {
		while (true) {
			//Debug.LogFormat ("oi"); 
			GameObject _obj = (GameObject)Instantiate (obj, parentObj.transform);
			yield return new WaitForSeconds (1f);
		}
	}
}