using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour 
{
	private int couchHealth = 100;

	[SerializeField]
	private int maxHealth;
	[SerializeField]
	private int health;
	public int Health { get { return health; } }

	//private int[] damageStages;

	[SerializeField]
	private Sprite standardSpr;
	[SerializeField]
	//an array so i can add damageStages Later, maybe even damageType based
	private Sprite damageSpr; // = new Sprite[1];

	private SpriteRenderer sprRenderer;

	private bool isDamaged = false;
	public bool IsDamaged { get { return isDamaged; } }
	private bool isInoperable = false;

	//roomOrigin
	private RoomScript room;

	/*
	[SerializeField]
	private bool isRoom;
	[SerializeField]
	private bool isSystem;
	[SerializeField]
	private bool isCrew;
	*/

	[SerializeField]
	private int objType;

	private void Start ()
	{
		health = maxHealth;
		sprRenderer = gameObject.GetComponent <SpriteRenderer> ();
		room = transform.parent.parent.GetChild (0).GetChild (0).GetComponent <RoomScript> ().GetRoomOrigin ();
	}

	public void TakeDamage (int _dmg)
	{
		health += _dmg;

		//obj was just damaged
		if (health < maxHealth && !isDamaged) {
			isDamaged = true;

			IPlacable _placable = gameObject.GetComponent <IPlacable> ();
			_placable.UpdateHealth (health);

			sprRenderer.sprite = damageSpr;

			room.Damages (1);
		} else if (health >= maxHealth && isDamaged) {
			//that way, if its greater by accident its corrected
			health = maxHealth;
			isDamaged = false;

			IPlacable _placable = gameObject.GetComponent <IPlacable> ();
			_placable.UpdateHealth (health);

			sprRenderer.sprite = standardSpr;

			room.Damages (-1);
		
		//ugly fix to damage without purpose
		} else if (health < maxHealth) {
			IPlacable _placable = gameObject.GetComponent <IPlacable> ();
			_placable.UpdateHealth (health);
		}


		/*
		bool _wasDmgd = false;
		if (health <= 0) {
			_wasDmgd = true;
		}

		if (health )

		if ((health + _dmg) <= maxHealth) {

			health += _dmg;

			//Debug.Log ("health: " + health);

			if (health <= 0) {
				if (!isDamaged) {
					//Debug.Log ("object damaged!");
					isDamaged = true;
					//gameObject.SetActive (false);

					//DEBUG -> REMOVES DESTROYED OBJECTS //currently, should the hp of a room be lower than the systemHP the room is destroyed but not the system -> needs fix

					//GameObject _obj = (GameObject)Instantiate (LevelManager.Instance.ObjDict [_objStr], transform.position, Quaternion.identity);
					IPlacable _placable = gameObject.GetComponent <IPlacable> ();

					//_amount is set to 0 for now... that might change later...
					_placable.UpdateHealth (health);

					sprRenderer.sprite = damageSpr;

					room.Damages (1);
				}
			} else {
				if (_wasDmgd) {
					isDamaged = false;

					IPlacable _placable = gameObject.GetComponent <IPlacable> ();
					_placable.UpdateHealth (health);

					sprRenderer.sprite = standardSpr;


				}
			}
		} else {
			//should return smthn to room, so that if all is repaired crew stops repairin
			//Debug.Log ("max reached");
		}


		if (health >= maxHealth) {
			health = maxHealth;
			room.Damages (-1);
		}
		*/


		/*
		if (health == maxHealth) {
			room.Damages (-1);
		} else if (health > maxHealth) {
			health = maxHealth;
			room.Damages (-1);
		}
		*/
	}

	public void TakeCrewDamage (int _amount) {
		room.TakeCrewDamage (_amount);
	}
}