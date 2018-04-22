using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSystemScript : MonoBehaviour, ISystem
{
	[SerializeField]
	private SystemScript systemScr;

	private Point gridPos;

	[SerializeField]
	private GameObject shield;
	private ShieldScript shieldScr;

	[SerializeField]
	private int powerReq;
	public int PowerReq { get { return powerReq; } }
	[SerializeField]
	//the amount of additional hitpoints
	private int shieldBoost;

	//[SerializeField]
	private int systemType = 1;

	void Start () {
		Setup ();
	}

	private void Setup () {
		gridPos = systemScr.GridPos;

		GameObject _ship = transform.parent.parent.parent.parent.gameObject;

		//Debug.LogError (_ship.transform.childCount);

		if (_ship.transform.childCount < 7) {
		//if (_ship.transform.GetChild (7) == null) {
			GameObject _shield = (GameObject)Instantiate (shield, _ship.transform);
			shield = _shield;

			shieldScr = shield.GetComponent <ShieldScript> ();
			shieldScr.Setup (gridPos.Z);

			/*
			GameObject _field = _ship.transform.GetChild (1).gameObject;

			shield.transform.position = _field.transform.position;
			shield.transform.localScale = (_field.transform.localScale * 16 / 1920);
			//float _scale = (Screen.width / shield.GetComponent <SpriteRenderer> ().bounds.size.x);
			float _scale0 = (_field.transform.localScale.x * 16 / 1920);
			float _scale1 = (_field.transform.localScale.y * 16 / 1080);
			//shield.transform.localScale = new Vector3 (_scale, _scale, _scale); 

			shield.transform.localScale = new Vector3 (_scale0, _scale1);
			*/
		} else {
			shield = _ship.transform.GetChild (6).gameObject;
			shieldScr = shield.GetComponent <ShieldScript> ();
		}

		AddToHP (shieldBoost);
	}

	/*
	public void OnDamage () {
		//AddToHP (-shieldBoost);
		Debug.Log ("ISystem/Shield");
	}
	*/

	private void AddToHP (int _amount) {
		shieldScr.SetMax (_amount);
	}


	public void UpdateHealth (int _hp) {
		Debug.Log ("ISystem/Shield");

		/* 	
		//onDamage
		if (_hp <= 0 && !isDamaged) {
			isDamaged = true;

			Debug.Log ("ISystem/Engine Damaged");

			PowerManager.Instance.DamageSystem (systemType, -powerReq);
		}

		//onRepair
		if (_hp > 0 && isDamaged) {
			isDamaged = false;

			Debug.Log ("ISystem/Engine Repaired");

			PowerManager.Instance.DamageSystem (systemType, powerReq);
		}
		*/
	}


	public void UpdateHealthState (bool _isFullyDamaged, bool _isFullyRepaired) {
		//Debug.Log ("shield: " + gridPos.X + ", " + gridPos.Y);

		if (_isFullyDamaged) {
			PowerManager.Instance.DamageSystem (1, -powerReq);
		} 

		if (_isFullyRepaired) {
			PowerManager.Instance.DamageSystem (1, powerReq);
		}

		//Debug.Log ("isFullyDamaged = " + _isFullyDamaged);
		//Debug.Log ("isFullyRepaired = " + _isFullyRepaired);
	}
}