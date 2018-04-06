using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node
{
	public Point GridPosition { get; private set; }

	public TileScript TileRef { get; private set; }

	public Node Parent { get; private set; }

	public Vector2 TileTransform { get; private set; }


	//path bools
	public bool IsTile { get; private set; }
	//public bool HasElevator { get; private set; }
	public bool HasElevator { get { if (TileRef != null) { return TileRef.HasElevator; } else { return false; } } }
	public bool HasDoor { get; private set; }
	//public bool DoorOpen { get; private set; }


	public int G { get; set; }
	public int H { get; set; }
	public int F { get; set; }

	//creates a refrence to the tile
	public Node (TileScript tileRef)
	{
		this.TileRef = tileRef;
		//sets its position
		this.GridPosition = tileRef.GridPosition;

		this.TileTransform = tileRef.transform.position;

		//bools
		this.IsTile = tileRef.IsTile;
		//this.HasElevator = tileRef.HasElevator;
		this.HasDoor = tileRef.HasDoor;
		//this.DoorOpen = tileRef.DoorOpen;
	}

	public void CalcValues (Node parent, Node goal, int gCost)
	{
		this.Parent = parent;
		this.G = parent.G + gCost;
		this.H = Mathf.RoundToInt(((Math.Abs(GridPosition.X - goal.GridPosition.X)) + (Math.Abs(goal.GridPosition.Y - GridPosition.Y))) * 10);
		this.F = G + H;
	}
}