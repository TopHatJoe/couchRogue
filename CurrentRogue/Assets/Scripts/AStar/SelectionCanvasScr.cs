using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionCanvasScr : MonoBehaviour {
	void Start () {
		if (CasheScript.Instance.CouchMode) {
			gameObject.SetActive (false);
		}
	}
}