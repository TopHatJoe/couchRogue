using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CouchCrewScript : MonoBehaviour 
{
	private Rigidbody2D rb;
	private BoxCollider2D col;
	private float speed = 1024;
	private string controllerID;
	[SerializeField]
	private Camera _cam;


	void Start () {
		rb = gameObject.GetComponent <Rigidbody2D> ();
		col = gameObject.GetComponent <BoxCollider2D> ();
		//_cam = transform.GetChild (0).gameObject.GetComponent <Camera> ();
	}

	void Update () {
		//if (Input.GetButton (controllerID + "-V")) {
			//Debug.Log ("axis");
			Vector3 _vect = new Vector3 (Input.GetAxis (controllerID + "-H"), 0);
			rb.MovePosition (transform.position + _vect * speed * Time.deltaTime);
		//}
	}

	public void CouchCrewSetup (string _controllerID, int _couchPlayerID, int _couchCount) {
		controllerID = _controllerID;

		Debug.Log ("playerID: " + _couchPlayerID);
		Debug.Log ("couchCount: " + _couchCount);

		if (_couchCount == 2) {
			TwoPlayerSplit (_couchPlayerID);
		} else if (_couchCount == 3) {
			ThreePlayerSplit (_couchPlayerID);
		} else if (_couchCount == 4) {
			FourPlayerSplit (_couchPlayerID);
		}
	}

	private void TwoPlayerSplit (int _couchPlayerID) {
		if (_couchPlayerID == 1) {
			Rect _rect0 = new Rect (0f, 0.5f, 1f, 0.5f);
			_cam.rect = _rect0;
		} else if (_couchPlayerID == 2) {
			Rect _rect1 = new Rect (0f, 0f, 1f, 0.5f);
			_cam.rect = _rect1;
		}


		//Rect _rect0 = new Rect (0f, 0.5f, 1f, 0.5f);
		//Rect _rect1 = new Rect (0f, 0f, 1f, 0.5f);

		//camList [0].rect = _rect0;
		//camList [1].rect = _rect1;
	}

	private void ThreePlayerSplit (int _couchPlayerID) {
		if (_couchPlayerID == 1) {
			Rect _rect0 = new Rect (0f, 0.5f, 1f, 0.5f);
			_cam.rect = _rect0;
		} else if (_couchPlayerID == 2) {
			Rect _rect1 = new Rect (0f, 0f, 0.5f, 0.5f);
			_cam.rect = _rect1;
		} else if (_couchPlayerID == 3) {
			Rect _rect2 = new Rect (0.5f, 0f, 0.5f, 0.5f);
			_cam.rect = _rect2;
		}


		//Rect _rect0 = new Rect (0f, 0.5f, 1f, 0.5f);
		//Rect _rect1 = new Rect (0f, 0f, 0.5f, 0.5f);
		//Rect _rect2 = new Rect (0.5f, 0f, 0.5f, 0.5f);

		//camList [0].rect = _rect0;
		//camList [1].rect = _rect1;
		//camList [2].rect = _rect2;
	}

	private void FourPlayerSplit (int _couchPlayerID) {
		if (_couchPlayerID == 1) {
			Rect _rect0 = new Rect (0f, 0.5f, 0.5f, 0.5f);
			_cam.rect = _rect0;
		} else if (_couchPlayerID == 2) {
			Rect _rect1 = new Rect (0.5f, 0f, 0.5f, 0.5f);
			_cam.rect = _rect1;
		} else if (_couchPlayerID == 3) {
			Rect _rect2 = new Rect (0f, 0.5f, 0.5f, 0.5f);
			_cam.rect = _rect2;
		} else if (_couchPlayerID == 4) {
			Rect _rect3 = new Rect (0.5f, 0f, 0.5f, 0.5f);
			_cam.rect = _rect3;
		}


		//Rect _rect0 = new Rect (0f, 0.5f, 0.5f, 0.5f);
		//Rect _rect1 = new Rect (0.5f, 0f, 0.5f, 0.5f);
		//Rect _rect2 = new Rect (0f, 0.5f, 0.5f, 0.5f);
		//Rect _rect3 = new Rect (0.5f, 0f, 0.5f, 0.5f);

		//camList [0].rect = _rect0;
		//camList [1].rect = _rect1;
		//camList [2].rect = _rect2;
		//camList [3].rect = _rect3;
	}


	public void OnCollosionEnter2D (Collision2D _col) {
		Debug.Log (_col.gameObject.layer);

		if (_col.gameObject.layer == 8) {
			Physics2D.IgnoreCollision (_col.collider, col);
		}
	}
}