using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Point 
{
	public int X { get; set; }
	public int Y { get; set; }
	//introducing Z as player identifier
	public int Z { get; set; }

	public Point (int x, int y, int z)
	{
		this.X = x;
		this.Y = y;
		this.Z = z;
	}

	//makes points comparable.
	public static bool operator == (Point first, Point second)
	{
		return first.X == second.X && first.Y == second.Y && first.Z == second.Z;
	}

	public static bool operator != (Point first, Point second)
	{
		return first.X != second.X || first.Y != second.Y || first.Z != second.Z;
	}

	public static Point operator - (Point x, Point y)
	{
		//return new Point (x.X - y.X, y.X - y.Y, 1);
		return new Point (x.X - y.X, x.Y - y.Y, x.Z - y.Z);
	}
}