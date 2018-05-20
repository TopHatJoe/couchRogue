using UnityEngine;

public interface ISystem {
	bool IsPowered ();

	void UpdateHealthState (bool _isFullydamaged, bool isFullyRepaired);
	//void OnDamage ();
	//void OnRepair ();

	void ReceivePowerUpdate (bool _isPowered);
	void UpdatePowerState (bool _isPowered);
}