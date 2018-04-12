using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class CouchCursorScr : MonoBehaviour 
{
	private Rigidbody2D rb;
	[SerializeField]
	private float speed = 1024;
	private string controllerID = "J01";

	private Collider2D col;

	private CouchCrewScript couchCrew;


	void Start () {
		rb = gameObject.GetComponent <Rigidbody2D> ();
		couchCrew = transform.parent.parent.GetComponent <CouchCrewScript> ();
	}

	void Update () {
		Vector3 _vect = new Vector3 (Input.GetAxis (controllerID + "-H"), -Input.GetAxis (controllerID + "-V"));
		if (_vect.magnitude > 1) {
			_vect.Normalize ();
		}

		rb.MovePosition (transform.position + _vect * speed * Time.deltaTime);
		//Debug.Log (Input.GetAxis (controllerID + "-H") + ", " + Input.GetAxis (controllerID + "-V"));

		if (col != null) {
			if (Input.GetButtonDown (controllerID + "-s")) {
				//mayhap just make the cursor useless outside the panel...
				if (col.GetComponent <ElevatorBtnPanelScr> () != null) {
					int _levelNum = col.GetComponent <ElevatorBtnPanelScr> ().PressButton ();
					Debug.Log ("please dont press that button again");
					couchCrew.UseElevator (_levelNum);
				}
			}
		}
	}

	public void OnTriggerEnter2D (Collider2D _col) {
		col = _col;

		_col.GetComponent <SpriteRenderer> ().color = Color.green;
		//_col.GetComponent <ElevatorBtnPanelScr> ().PressButton ();
	}

	public void OnTriggerExit2D (Collider2D _col) {
		col = null;

		_col.GetComponent <SpriteRenderer> ().color = Color.grey;
		//_col.GetComponent <ElevatorBtnPanelScr> ().PressButton ();
	}
}