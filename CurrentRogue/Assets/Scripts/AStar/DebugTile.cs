using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugTile : MonoBehaviour 
{
	[SerializeField]
	private Text f;
	public Text F 
	{ 
		get 
		{
			f.gameObject.SetActive (true);
			return f; 
		} set { this.f = value; } }

	[SerializeField]
	private Text g;
	public Text G 
	{ 
		get 
		{ 
			g.gameObject.SetActive (true);
			return g; 
		} set { this.g = value; } }

	[SerializeField]
	private Text h;
	public Text H 
	{ 
		get 
		{ 
			h.gameObject.SetActive (true);
			return h; 
		} set { this.h = value; } }
}