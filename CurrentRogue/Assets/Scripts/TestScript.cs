using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//probably needs a better name
public class TestScript : MonoBehaviour 
{
	[SerializeField]
	private int amount;

	[SerializeField]
	private GameObject spawn;
	[SerializeField]
	private GameObject projectile;
	[SerializeField]
	private GameObject target;

	public void IncrementReactor () {
		if (ReactorScript.DirectPower (amount)) {
			Debug.Log ("let there be light");
		} else {
			Debug.Log ("Darkness cometh");
		}
	}

	public void DecrementReactor () {
		if (ReactorScript.RedirectPower (amount)) {
			Debug.Log ("Reactor already down");
		} else {
			Debug.Log ("Powering down");
		}
	}


	public void Shoot () {
		GameObject _obj = (GameObject)Instantiate (projectile, spawn.transform.position, Quaternion.identity);
		//_obj.GetComponent <AmmoScript> ().Fire (target, _angle);
	}





	/*
	Vector2 _vect = targetObj.transform.position - transform.position;
	float _angle = Mathf.Atan2 (_vect.y, _vect.x) * Mathf.Rad2Deg;
	Quaternion _q = Quaternion.AngleAxis (_angle, Vector3.forward);
	transform.rotation = _q;
	*/
}