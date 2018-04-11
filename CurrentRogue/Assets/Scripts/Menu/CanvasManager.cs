using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : Singleton <CanvasManager> 
{
	[SerializeField]
	private Canvas[] canvasArr;

	void Start () {
		if (!CasheScript.Instance.CouchMode) {
			gameObject.SetActive (false);
		}
	}

	public void CouchCanvas (int _couchPlayerID, Camera _cam) {
		canvasArr [_couchPlayerID - 1].worldCamera = _cam;
		canvasArr [_couchPlayerID - 1].gameObject.SetActive (true);
	}
}