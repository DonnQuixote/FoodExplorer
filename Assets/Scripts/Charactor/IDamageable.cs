using UnityEngine;

public interface IDamageable
{

	void TakeHit(float damage,Vector3 hitpoint, Vector3 hidDirection);
	//void TakeHit(float damage, RaycastHit hit);

	void TakeDamage(float damage);

}