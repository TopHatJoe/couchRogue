using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarDebugger : MonoBehaviour
{ 
	//[SerializeField]
	private TileScript start, goal;

	[SerializeField]
	private Sprite blankTile;

	[SerializeField]
	private GameObject arrowPrefab;

	[SerializeField]
	private GameObject debugTilePrefab;

	/*
	void Update ()
	{
		ClickTile ();	

		if (Input.GetKeyDown(KeyCode.Space))
		{
			AStar.GetPath (start.GridPosition, goal.GridPosition);
		}
	}
	*/

	private void ClickTile ()
	{
		//if (Input.GetMouseButton(1))
		if (Input.GetKeyDown(KeyCode.P))
		{
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);

			if (hit.collider != null)
			{
				TileScript tmp = hit.collider.GetComponent<TileScript> ();

				if (tmp != null)
				{
					if (start == null) 
					{
						start = tmp;
						CreateDebugTile(start.WorldPostion, new Color32 (255, 132, 0, 255));
					} 

					else if (goal == null) 
					{
						goal = tmp;
						CreateDebugTile(goal.WorldPostion, new Color32 (255, 0, 0, 255));
					}
				}
			}
		}
	}

	public void DebugPath (HashSet<Node> openList, HashSet<Node> closedList, Stack<Node> path)
	{
		foreach (Node node in openList) 
		{
			if (node.TileRef != start && node.TileRef != goal)
			{
				CreateDebugTile (node.TileRef.WorldPostion, Color.cyan, node);

				//node.TileRef.SpriteRenderer.color = Color.cyan;
				//node.TileRef.SpriteRenderer.sprite = blankTile;
			}

			PointToParent (node, node.TileRef.WorldPostion);
		}

		foreach (Node node in closedList) 
		{
			if (node.TileRef != start && node.TileRef != goal && !path.Contains(node))
			{
				CreateDebugTile (node.TileRef.WorldPostion, Color.blue, node);
			}

			PointToParent (node, node.TileRef.WorldPostion);
		}

		foreach (Node node in path) 
		{
			if (node.TileRef != start && node.TileRef != goal)
			{
				CreateDebugTile (node.TileRef.WorldPostion, Color.green, node);
			}
		}
	}

	private void PointToParent (Node node, Vector2 position)
	{
		if (node.Parent != null)
		{
			GameObject arrow = (GameObject) Instantiate (arrowPrefab, position, Quaternion.identity);

			//right
			if ((node.GridPosition.X < node.Parent.GridPosition.X) && (node.GridPosition.Y == node.Parent.GridPosition.Y))
			{
				arrow.transform.eulerAngles = new Vector3 (0, 0, 0);
			}
			//top right
			else if ((node.GridPosition.X < node.Parent.GridPosition.X) && (node.GridPosition.Y < node.Parent.GridPosition.Y))
			{
				arrow.transform.eulerAngles = new Vector3 (0, 0, 45);
			}
			//up
			else if ((node.GridPosition.X == node.Parent.GridPosition.X) && (node.GridPosition.Y < node.Parent.GridPosition.Y))
			{
				arrow.transform.eulerAngles = new Vector3 (0, 0, 90);
			}
			//top left
			else if ((node.GridPosition.X > node.Parent.GridPosition.X) && (node.GridPosition.Y < node.Parent.GridPosition.Y))
			{
				arrow.transform.eulerAngles = new Vector3 (0, 0, 135);
			}
			//left
			else if ((node.GridPosition.X > node.Parent.GridPosition.X) && (node.GridPosition.Y == node.Parent.GridPosition.Y))
			{
				arrow.transform.eulerAngles = new Vector3 (0, 0, 180);
			}
			//bottom left
			else if ((node.GridPosition.X > node.Parent.GridPosition.X) && (node.GridPosition.Y > node.Parent.GridPosition.Y))
			{
				arrow.transform.eulerAngles = new Vector3 (0, 0, 225);
			}
			//down
			else if ((node.GridPosition.X == node.Parent.GridPosition.X) && (node.GridPosition.Y > node.Parent.GridPosition.Y))
			{
				arrow.transform.eulerAngles = new Vector3 (0, 0, 270);
			}
			//bottom right
			else if ((node.GridPosition.X < node.Parent.GridPosition.X) && (node.GridPosition.Y > node.Parent.GridPosition.Y))
			{
				arrow.transform.eulerAngles = new Vector3 (0, 0, 315);
			}
		}
	}

	//to make a parameter optional, set it equal to something
	private void CreateDebugTile (Vector3 worldPos, Color32 color, Node node = null)
	{
		GameObject debugTile = (GameObject)Instantiate (debugTilePrefab, worldPos, Quaternion.identity);

		if (node != null)
		{
			DebugTile tmp = debugTile.GetComponent<DebugTile> ();

			tmp.G.text += node.G;
			tmp.H.text += node.H;
			tmp.F.text += node.F;
		}

		debugTile.GetComponent<SpriteRenderer> ().color = color;
	}

	public void DisplayPoint (Vector3 worldPos, Color32 color)
	{
		GameObject debugTile = (GameObject)Instantiate (debugTilePrefab, worldPos, Quaternion.identity);
		debugTile.GetComponent<SpriteRenderer> ().color = color;
	}
}