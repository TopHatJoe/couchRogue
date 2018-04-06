using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareTileScript : MonoBehaviour 
{
	public Point SquareGridPosition { get; private set; }

	//private SpriteRenderer spriteRenderer;

	public Vector2 WorldPostion 
	{ get { return new Vector2 (transform.position.x + GetComponent<SpriteRenderer> ().bounds.size.x / 2, transform.position.y + GetComponent<SpriteRenderer> ().bounds.size.y / 2); } }


	void Start () 
	{
		//spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	public void Setup (Point gridPos, Vector3 worldPos, Transform parent)
	{
		this.SquareGridPosition = gridPos;
		transform.position = worldPos;
		//sets gamePbject grid as newTiles parent
		transform.SetParent (parent);
	}
}