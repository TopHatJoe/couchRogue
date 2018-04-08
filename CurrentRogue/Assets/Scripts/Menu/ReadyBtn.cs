using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyBtn : MonoBehaviour 
{
	private Image _img;
	[SerializeField]
	private ControllerMenu ctrlMenu;
	[SerializeField]
	private CouchSetupMenu couchMenu;
	[SerializeField]
	private int playerID;


	void Start () {
		_img = gameObject.GetComponent <Image> ();
	}

	public void Ready () {
		_img.color = Color.green;
		ctrlMenu.IsReady = true;
		couchMenu.SetReady (playerID, true);
	}
}