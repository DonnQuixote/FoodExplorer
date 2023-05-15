using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Gun : MonoBehaviour
{

	public Transform muzzle;
	public Projectile projectile;
	public float msBetweenShots = 100;
	public float muzzleVelocity = 35;
	[SerializeField] float damageValue = 1.0f;

	float nextShotTime;

	public void Shoot()
	{

		if (Time.time > nextShotTime)
		{
			nextShotTime = Time.time + msBetweenShots / 1000;
			Projectile newProjectile = Instantiate(projectile, muzzle.position, this.transform.rotation) as Projectile;
			newProjectile.SetSpeed(muzzleVelocity);
			newProjectile.SetDamageValue(damageValue);
		}
	}
}