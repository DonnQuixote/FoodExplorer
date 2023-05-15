using UnityEngine;
using System.Collections;
using System;

public class LivingEntity : MonoBehaviour, IDamageable
{

	public float startingHealth;
	[SerializeField]protected float health;
	protected bool dead;

	public event System.Action OnDeath;
	public event EventHandler<OnHealthChangeArgs> OnHealthChange;
	public event EventHandler OnPlayerDestroy;

	public class OnHealthChangeArgs : EventArgs
    {
		public float health;
    }
	protected virtual void Start()
	{
		health = startingHealth;
	}



	public virtual void TakeHit(float damage, Vector3 hitPoint , Vector3 hitDirection)
	{
		// Do some stuff here with hit var
		TakeDamage(damage);
	}

	public void TakeDamage(float damage)
	{
		health -= damage;
		//Debug.Log("injured");
		OnHealthChange?.Invoke(this, new OnHealthChangeArgs
		{
			health = health / startingHealth
		});
		if (health <= 0 && !dead)
		{
			Die();
		}
	}

	protected void Die()
	{
		dead = true;
		if (OnDeath != null)
		{
			OnDeath();
		}
		if(gameObject.tag == "Player")
        {
			OnPlayerDestroy?.Invoke(this, EventArgs.Empty);
		}
		GameObject.Destroy(gameObject);
	}
}