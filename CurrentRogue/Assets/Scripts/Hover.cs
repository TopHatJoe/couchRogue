using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover>
{
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	private bool couchMode = false;

	void Start () {
		couchMode = CasheScript.Instance.CouchMode;
	}

	void Update ()
	{
		if (!couchMode) {
			FollowMouse ();
		}
	}

	private void FollowMouse()
	{
		if (spriteRenderer.enabled)
		{
			//sets the position of the hover object equal to the mouse position
			transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			transform.position = new Vector3 (transform.position.x, transform.position.y, -10);
		}
	}

	public void Activate (Sprite sprite)
	{
		this.spriteRenderer.sprite = sprite;
		spriteRenderer.enabled = true;
	}

	public void Deactivate ()
	{
		//problem: spriteRenderer.enabled = false; disables the sprite of the previously placed roomTile
		spriteRenderer.enabled = false;
		GameManager.Instance.ClickedBtn = null;
	}
}