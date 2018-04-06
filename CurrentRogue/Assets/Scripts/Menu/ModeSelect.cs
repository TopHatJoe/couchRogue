﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelect : MonoBehaviour {
	[SerializeField]
	private Text text;
	[SerializeField]
	private bool couchMode = true;


	void Start () {
		couchMode = !couchMode;
		SwitchMode ();
	}

	public void SwitchMode () {
		couchMode = !couchMode;

		if (couchMode) {
			text.text = "switch to classic";
		} else {
			text.text = "switch to couch";
		}

		Debug.Log ("couchmode = " + couchMode);
		CasheScript.Instance.SwitchMode (couchMode);
	}
}