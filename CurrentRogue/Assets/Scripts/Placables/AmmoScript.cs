using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoScript : MonoBehaviour 
{
	[SerializeField]
	//the time the obj takes to reach its destination
	private float time;
	//the speed when taking the distance into account
	private float speed;

	//[SerializeField]
	private int damage;

	private Vector3 target;
	private Vector2 overshotTarget;
	[SerializeField]
	private float dist;
	private GameObject targetObj;

	[SerializeField]
	private float atkCircleCoEfficient;

	private float angle;	
	private Vector3 targetVect;

	//private Rigidbody2D rb2D;
	private int playerID;
	public int PlayerID { get { return playerID; } }

	//for quicker access
	private ShipScript ship;

	//hit probability
	private int prob;


	public void Fire (GameObject _target, int _prob, float _angle, int _ID, int _dmg) {
		playerID = _ID;
		int _enemyID = _target.transform.parent.GetComponent <TargetScript> ().GridPos.Z;
		ship = LevelManager.Instance.Ships [_enemyID].GetComponent <ShipScript> ();

		damage = _dmg;

		prob = _prob;

		//dist = Screen.width * 10;
		GameObject _gridField = LevelManager.Instance.GridField;
		float _a = _gridField.GetComponent <SpriteRenderer> ().sprite.bounds.size.x;
		float _b = _gridField.transform.localScale.x;
		//dist = _gridField.GetComponent <SpriteRenderer> ().sprite.bounds.size.x;
		dist = _a * _b;


		target = new Vector3 (transform.position.x + dist, transform.position.y, transform.position.z);
		targetObj = _target;

		//stores the targetPos so that if the targetObj is destroyed, it still knows where to go
		targetVect = targetObj.transform.position;

		angle = _angle;

		//test
		speed = dist/time;
		//Debug.LogError ("dist: " + dist);
		//Debug.LogError ("time: " + time);
		//Debug.LogError ("speed: " + speed);

		//rb2D = gameObject.GetComponent <Rigidbody2D> ();

		StartCoroutine (Move ());
	}


	public void SetTarget (TileScript _tile) {
		targetObj = _tile.gameObject;
	}


	private IEnumerator Move ()
	{
		while (Vector2.Distance (transform.position, target) > 0.005f) {

			//print ("test1");


			yield return new WaitForSeconds (0.002f);

			//Debug.Log ("Movin");

			transform.position = Vector2.MoveTowards (transform.position, target, speed);
			//transform.position = Vector2.MoveTowards (transform.position, target, speed * Time.deltaTime);
			//rb2D.MovePosition (transform.position + target * speed * Time.deltaTime);

			//rb2D.AddRelativeForce (target * speed, ForceMode2D.Force);

			/*
		yield return new WaitForSeconds (0.002f);

		if (transform.position == target) {
			yield break;
		}
		*/

		}

	

		//gets a random position from the edge of a circle 
		//Vector2 _circlePoint = GetPointOnCircle ();
		Vector2 _circlePoint = new Vector2 (Mathf.Sin (angle), Mathf.Cos (angle)).normalized;
		//Debug.Log (_circlePoint); 
		overshotTarget = -_circlePoint;

		// = dist;

		//applies the _circlePoint to the target obj, so that it is in its center
		//Vector2 _newPos = new Vector2 ((targetObj.transform.position.x + (_circlePoint.x * atkCircleCoEfficient)), (targetObj.transform.position.y + (_circlePoint.y * atkCircleCoEfficient)));
		//Vector2 _newPos = new Vector2 ((targetVect.x + (_circlePoint.x * atkCircleCoEfficient)), (targetVect.y + (_circlePoint.y * atkCircleCoEfficient)));
		//Vector2 _newPos = new Vector2 ((targetVect.x + (_circlePoint.x * atkCircleCoEfficient)), (targetVect.y + (_circlePoint.y * atkCircleCoEfficient)));
		Vector2 _newPos = new Vector2 ((targetVect.x + (_circlePoint.x * dist)), (targetVect.y + (_circlePoint.y * dist)));

		//positions the projectile
		transform.position = _newPos;

		//rotation
		//Vector2 _vect = targetObj.transform.position - transform.position;
		Vector2 _vect = targetVect - transform.position;
		float _angle = Mathf.Atan2 (_vect.y, _vect.x) * Mathf.Rad2Deg;
		Quaternion _q = Quaternion.AngleAxis (_angle, Vector3.forward);
		transform.rotation = _q;


		//sets the direction to center
		//target = targetObj.transform.position;
		target = targetVect;
		//target = targetObj.transform.

		while (Vector2.Distance (transform.position, target) > 0.005f) {

			//print ("test2");


			yield return new WaitForSeconds (0.002f);

			//Debug.Log ("Movin");

			//transform.position = Vector2.MoveTowards (transform.position, target, speed * Time.deltaTime);
			transform.position = Vector2.MoveTowards (transform.position, target, speed);
		}

		//print ("here2");


		//evasion check 
		//if (targetObj.GetComponent <TargetScript>().GridPos.Z == NetManager.Instance.localPlayerID) {
		int _x = Random.Range (1, 100);
		//50 is a placeholder, needs to be evasion chance
		//int _evasion = LevelManager.Instance.Ships [playerID].GetComponent <ShipScript> ().EvasionChance;
		int _evasion = ship.EvasionChance;
		Debug.Log ("evasion: " + _evasion);
		
			//if (_x <= _evasion) {
		if (prob <= _evasion) {
				//miss
				HitAndMiss (false);
		} else {
			//hit
			HitAndMiss (true);
		}
		//}


		/*
		//destroys targetObj
		if (targetObj != null) {
			Destroy (targetObj.transform.parent.gameObject);
		}

		//destroys projectileObj
		Destroy (gameObject);
		*/
	}


	/*
	private Vector2 GetPointOnCircle () {
		float _angle = Random.Range (0f, Mathf.PI * 2f);

		//angle = _angle;
		//return new Vector2 (Mathf.Sin (_angle), Mathf.Cos (_angle)).normalized;

		//gets point on opposite side of circle
		//overshotTarget = new Vector2 (Mathf.Sin (_angle + 180), Mathf.Cos (_angle + 180)).normalized;

		return new Vector2 (Mathf.Sin (_angle), Mathf.Cos (_angle)).normalized;
	}
	*/



	//called from playerInfo //not -> totally worthless you piece of (s)hit
	public void HitAndMiss (bool _hit) {
		if (_hit) {
			Hit ();
		} else {
			Miss ();
		}
	}

	private void Hit () {
		Debug.Log ("Target Hit!");

		DealDamage (damage);

		//destroys targetObj
		if (targetObj != null) {
			Destroy (targetObj.transform.parent.gameObject);
		}

		//destroys projectileObj
		Destroy (gameObject);
	}

	private void Miss () {
		Debug.Log ("Target missed!");
		StartCoroutine (Flyby ());
	}

	private IEnumerator Flyby () {
		//target = transform.forward * dist;

		Vector2 _circlePoint = overshotTarget; //new Vector2 (Mathf.Sin (angle + 180), Mathf.Cos (angle + 180)).normalized;
		//Debug.Log (_circlePoint); 

		Vector2 _newPos = new Vector2 ((targetVect.x + (_circlePoint.x * dist)), (targetVect.y + (_circlePoint.y * dist)));


		//int _elapsedTime = 0;
		//while (Vector2.Distance (transform.position, overshotTarget) > 0.005f) {
		while (Vector2.Distance (transform.position, _newPos) > 0.005f) {
		//while (_elapsedTime < 10000) {
			//target = transform.forward * dist;

			//transform.position = Vector2.MoveTowards (transform.position, overshotTarget, speed);
			transform.position = Vector2.MoveTowards (transform.position, _newPos, speed);
			//Vector2.MoveTowards (

			//transform.position = Vector2.MoveTowards (transform.position, transform.forward, -speed); //transform.forward * speed;
			//transform.Translate (transform.forward * 1000);
			//transform.position = transform.Translate (transform.forward);
			//transform.position += transform.forward * speed;

			//_elapsedTime++;

			yield return new WaitForSeconds (0.002f);
		}

		print ("done");
	}




	private void DealDamage (int _dmg) {
		Point _pos = targetObj.transform.parent.GetComponent <TargetScript> ().GridPos;
		//-_dmg so damage can be positive but deals negative effect
		LevelManager.Instance.Tiles [_pos].TakeDamage (-_dmg);
	}


	//BuffDamage () 
}