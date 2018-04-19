using UnityEngine;

public interface IPlacable {
	void PlaceObj (int _index, Point _gridPos, GameObject _originObj);

	//void RemoveObj (int _index, Point _gridPos);
	void RemoveObj ();

	//string ReturnParameter ();
	void UpdateHealth (int _amount);

	GameObject GetOriginObj ();
}