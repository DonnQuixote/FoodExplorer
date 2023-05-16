using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

	//public static GunController Instance { get; private set; }
	public Transform weaponHold;
	//public Gun startingGun;
	public GameObject startingGun;
	Gun equippedGun;

	[SerializeField]private InputMessage inputMessage;
	private bool isPressedFirePrepared;



	void Start()
	{
		//Instance = this;
        inputMessage.OnFireGunChanging += GunController_OnFireChanging;
		startingGun.SetActive(false);
		equippedGun = startingGun.GetComponent<Gun>();
	}



    private void GunController_OnFireChanging(object sender, InputMessage.OnFireGunChangingArgs e)
    {
		 Quaternion q= e.playerRotation;

		if (isPressedFirePrepared)
		{
			isPressedFirePrepared = false;
			startingGun.SetActive(false);
		}
		else
		{
			isPressedFirePrepared = true;
			startingGun.SetActive(true);
		}
	}

	public void Shoot()
	{
		if (startingGun.activeSelf)
		{
			equippedGun.Shoot();
		}
	}
}