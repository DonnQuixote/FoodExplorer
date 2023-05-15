using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{

	[SerializeField] float radius = 26.0f;
	public LayerMask collisionMask;
	float speed = 10;
	float damage = 1;

	public void SetSpeed(float newSpeed)
	{
		speed = newSpeed;
	}

	void Update()
	{
		float moveDistance = speed * Time.deltaTime;
		CheckCollisions(moveDistance);
		transform.Translate(Vector3.forward * moveDistance);
	}


	void CheckCollisions(float moveDistance)
	{
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
		{
			OnHitObject(hit.collider,hit.point);
		}

		if(Mathf.Abs(transform.position.x) > radius || Mathf.Abs(transform.position.z) > radius)
        {
			Destroy(gameObject);
        }
	}

	void OnHitObject(Collider c,Vector3 hitPoint)
	{
		IDamageable damageableObject = c.GetComponent<IDamageable>();
		//Debug.Log(damage);
		if (damageableObject != null)
		{
			damageableObject.TakeHit(damage, hitPoint,transform.forward);
		}
		GameObject.Destroy(gameObject);
	}

	public void SetDamageValue(float changeDamage)
    {
		damage = changeDamage;
	}
}