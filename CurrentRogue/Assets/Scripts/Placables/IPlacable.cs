using UnityEngine;

public interface IPlacable {
	void PlaceObj (int _index, Point _gridPos, GameObject _originObj);

	//void RemoveObj (int _index, Point _gridPos);
	void RemoveObj ();

	//basically just updates sprites now...
	//string ReturnParameter ();
	void UpdateHealthState (bool _isFullyDamaged, bool _isFullyRepaired);

	GameObject GetOriginObj ();
}