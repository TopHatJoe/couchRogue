using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public static class AStar 
{
	private static Dictionary<Point, Node> nodes;
	//public static Dictionary<Point, Node> Nodes { get { return nodes; } }

	private static void CreateNodes ()
	{
		nodes = new Dictionary<Point, Node> ();

		foreach (TileScript tile in LevelManager.Instance.Tiles.Values) 
		{
			nodes.Add (tile.GridPosition, new Node (tile));
		}
	}

	/*
	public static void Path (Point _start, Point _goal, CrewScript _crew)
	{
		
	}
	*/
	
	public static Stack <Node> GetPath (Point start, Point goal)
	{
		if (nodes == null)
		{
			CreateNodes ();
		}

		HashSet<Node> openList = new HashSet<Node> ();
		HashSet<Node> closedList = new HashSet<Node> ();

		Stack <Node> finalPath = new Stack<Node> ();

		Node currentNode = nodes [start];

		bool pathFound = false;


		//step 1
		openList.Add (currentNode);

		//step 10
		while (openList.Count > 0) 
		{
			//if current node contailns elevator:

			//step 2
			for (int x = -1; x <= 1; x++) 
			{
				//for (int y = -1; y <= 1; y++) 
				//{
				//Debug.Log (x + ", " + y);

				Point currentPos = new Point (currentNode.GridPosition.X, currentNode.GridPosition.Y, currentNode.GridPosition.Z);

				//takes the start nodes position and adds -1 through 1 to it //not both vertically and horizontally but horizontally

				Point neighbourPos = new Point (currentNode.GridPosition.X - x, currentNode.GridPosition.Y, currentNode.GridPosition.Z);



				//&& LevelManager.Instance.Tiles[neighbourPos].SystemPlacable	
				if (LevelManager.Instance.InBounds(neighbourPos) && LevelManager.Instance.Tiles[neighbourPos].Walkable && neighbourPos != currentNode.GridPosition) 
				{	
					int gCost = 0;
					if (LevelManager.Instance.Tiles [currentPos].IsTile) {
						//-10 = 10//-4 = 4// 8 = 8//no negatives
						if (Math.Abs (x) == 1) 
						{
							gCost = 10;
						}


						//probably best to add an if statement with the worst case and then going down to only one gCost modifier...

						//Danger Zone
						if (LevelManager.Instance.Tiles[currentPos].OnFire)
						{
							gCost = 420;
						}


						//Elevator Part
						if (LevelManager.Instance.Tiles[currentPos].HasElevator)
						{
							//Debug.Log (currentNode.GridPosition.X + ", " + currentNode.GridPosition.Y + "Has an Elevator");

							/*
							for (int y = -9; y <= 9; y++) 
							{
								if (y == 0) {
									y++;
								}

								Point _pos = new Point (currentNode.GridPosition.X, currentNode.GridPosition.Y + y, currentNode.GridPosition.Z);

								if (!LevelManager.Instance.InBounds (_pos) || !LevelManager.Instance.Tiles [_pos].Passable) { //mayhap tmp!!
									//break; 
								} else {

									//Debug.Log ("upperNeighbour" + upperNeighbourPos.X + ", " + upperNeighbourPos.Y);

									gCost = y * 10;


									Node _yNeighbour = nodes [_pos];

									//if (LevelManager.Instance.Tiles[upperNeighbourPos].HasElevator)
									if (_yNeighbour.HasElevator) {
										if (openList.Contains (_yNeighbour)) {
											//step 9.4
											if (currentNode.G + gCost < _yNeighbour.G) {
												_yNeighbour.CalcValues (currentNode, nodes [goal], gCost);
											}
										} else if (!closedList.Contains (_yNeighbour)) {
											//step 9.2
											openList.Add (_yNeighbour);
											//step 9.3
											_yNeighbour.CalcValues (currentNode, nodes [goal], gCost);
										}	
									}
								}
							}
							*/


							for (int positiveY = 1; positiveY <= 9; positiveY++) 
							{
								Point upperNeighbourPos = new Point (currentNode.GridPosition.X, currentNode.GridPosition.Y + positiveY, currentNode.GridPosition.Z);

								if (!LevelManager.Instance.InBounds(upperNeighbourPos) || !LevelManager.Instance.Tiles[upperNeighbourPos].Passable) //mayhap tmp!!
								{ break; }

								//Debug.Log ("upperNeighbour" + upperNeighbourPos.X + ", " + upperNeighbourPos.Y);

								gCost = positiveY * 10;


								Node upperNeighbour = nodes[upperNeighbourPos];

								//if (LevelManager.Instance.Tiles[upperNeighbourPos].HasElevator)
								if (upperNeighbour.HasElevator)
								{
									if (openList.Contains(upperNeighbour))
									{
										//step 9.4
										if (currentNode.G + gCost < upperNeighbour.G) 
										{
											upperNeighbour.CalcValues (currentNode, nodes [goal], gCost);
										}
									} else if (!closedList.Contains(upperNeighbour)) {
										//step 9.2
										openList.Add(upperNeighbour);
										//step 9.3
										upperNeighbour.CalcValues (currentNode, nodes [goal], gCost);
									}	
								}
							}


							for (int negativeY = 1; negativeY <= 9; negativeY++) 
							{
								Point lowerNeighbourPos = new Point (currentNode.GridPosition.X, currentNode.GridPosition.Y - negativeY, currentNode.GridPosition.Z);
								//!LevelManager.Instance.Tiles[lowerNeighbourPos].Walkable // baby problem me
								if (!LevelManager.Instance.InBounds(lowerNeighbourPos) || !LevelManager.Instance.Tiles[lowerNeighbourPos].Passable) //mayhap tmp!!
								{ break; }

								//Debug.Log ("lowerNeighbour" + lowerNeighbourPos.X + ", " + lowerNeighbourPos.Y);

								gCost = negativeY * 10;


								Node lowerNeighbour = nodes[lowerNeighbourPos];

								//if (LevelManager.Instance.Tiles[lowerNeighbourPos].HasElevator)
								//if (nodes [lowerNeighbourPos].HasElevator)
								if (lowerNeighbour.HasElevator)
								{
									if (openList.Contains(lowerNeighbour))
									{
										//step 9.4
										if (currentNode.G + gCost < lowerNeighbour.G) 
										{
											lowerNeighbour.CalcValues (currentNode, nodes [goal], gCost);
										}
									} else if (!closedList.Contains(lowerNeighbour)) {
										//step 9.2
										openList.Add(lowerNeighbour);
										//step 9.3
										lowerNeighbour.CalcValues (currentNode, nodes [goal], gCost);
									}
								}
							}
						}
					}

					//Tweens
					if (!nodes [currentPos].IsTile)
					{
						if (nodes [currentPos].HasDoor) {
							//gonna keep the Tiles[] here since otherwise id have to constantly update node
							if (LevelManager.Instance.Tiles[currentPos].DoorOpen) 
							{
								gCost = 0;
								//Debug.Log ("nope");
							} else {
								gCost = 5000;
								//Debug.Log ("gcost = its over 5000!!!");
							}
						}
					}

					/*
					if (!LevelManager.Instance.Tiles[currentPos].IsTile)
					{
						if (LevelManager.Instance.Tiles[currentPos].HasDoor) {
							if (LevelManager.Instance.Tiles[currentPos].DoorOpen) 
							{
								gCost = 0;
								//Debug.Log ("nope");
							} else {
								gCost = 5000;
								//Debug.Log ("gcost = its over 5000!!!");
							}
						}
					}
					*/


					//step 3
					Node neighbour = nodes[neighbourPos];

					if (openList.Contains(neighbour))
					{
						//step 9.4
						if (currentNode.G + gCost < neighbour.G)
						{
							neighbour.CalcValues (currentNode, nodes [goal], gCost);
						}
					}

					//step 9.1
					else if (!closedList.Contains(neighbour))
					{
						//step 9.2
						openList.Add(neighbour);
						//step 9.3
						neighbour.CalcValues (currentNode, nodes [goal], gCost);
					}
				}
			}

			//step 5 && 8
			openList.Remove (currentNode);
			//bug of the 25th of april
			closedList.Add (currentNode);

			if (openList.Count > 0)
			{
				//step 7
				//oders the openlist by F score and selects the first. (lowest f score)
				currentNode = openList.OrderBy (n => n.F).First ();
			}

			if (currentNode == nodes[goal])
			{
				//adds goal twice
				finalPath.Push (nodes [goal]);
				//gives the go ahead for movement
				pathFound = true;

				while (currentNode.GridPosition != start)
				{
					finalPath.Push (currentNode);
					currentNode = currentNode.Parent;
				}

				//adds start another time, for doorcheck
				finalPath.Push (nodes [start]);

				//Debug.Log ("Path found!");

				break;
			}
		}

		//DEBUG
		//GameObject.Find ("AStarDebugger").GetComponent<AStarDebugger>().DebugPath(openList, closedList, finalPath);

		if (pathFound) {
			return finalPath;
		} else {
			Debug.LogError ("NO PATH FOUND");
			return null;
		}

		//DEBUG!!!!!1!!!11!!!
		//GameObject.Find ("AStarDebugger").GetComponent<AStarDebugger>().DebugPath(openList, closedList, finalPath);
	}

	private static bool ConnectedDiagonally (Node currentNode, Node neighbor)
	{
		Point direction = neighbor.GridPosition - currentNode.GridPosition;

		Point first = new Point (currentNode.GridPosition.X + direction.X, currentNode.GridPosition.Y, currentNode.GridPosition.Z);

		Point second = new Point (currentNode.GridPosition.X, currentNode.GridPosition.Y + direction.Y, currentNode.GridPosition.Z);

		if (LevelManager.Instance.InBounds(first) && !LevelManager.Instance.Tiles[first].Walkable) {
			return false;
		} if (LevelManager.Instance.InBounds(second) && !LevelManager.Instance.Tiles[second].Walkable) 
		{
			return false;
		} else {
			return true;
		}
	}
}