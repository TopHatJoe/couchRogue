using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMappingTemp : MonoBehaviour 
{
	private float threshhold = 0.16f;
	private Rigidbody2D rb2D;
	[SerializeField]
	private float speed;

	[SerializeField]
	private string controllerID;

	void Start () {
		rb2D = gameObject.GetComponent <Rigidbody2D> ();
	}

	void Update () {
		//Vector3 _vect = new Vector3 (Input.GetAxis ("J01-H"), -Input.GetAxis ("J01-V"), 0);
		Vector3 _vect = new Vector3 (Input.GetAxis (controllerID + "-H"), 0, 0);

		rb2D.MovePosition (transform.position + _vect * Time.deltaTime * speed);
	}
}