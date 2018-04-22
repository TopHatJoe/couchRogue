using UnityEngine;

public interface ISystem {
	void UpdateHealthState (bool _isFullydamaged, bool isFullyRepaired);
	//void OnDamage ();
	//void OnRepair ();
}