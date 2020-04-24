using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public Transform muzzel, LeftHand,RightHand,LeftElbow,RightElbow,LookObj;

	public float roundsPerSecond, bulletsInTheGun, bulletImpulse, magazine_size,total_bullets;
  
	public bool reloading;

	public GameObject bullet;
	public GameObject flash;	

	public bool shooting;
	public Transform lookat;

	private float waitTillNextFire;
	 private float offset;
	void Start()
    {
		reloading = false;
		shooting = false;
		flash.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {
		waitTillNextFire -= roundsPerSecond * Time.deltaTime;
		transform.LookAt(lookat);
		if (shooting)
			flash.SetActive(true);
		else
			flash.SetActive(false);
    }

	public void ShootMethod()
	{
		if (waitTillNextFire <= 0 && !reloading)
		{

			if (bulletsInTheGun > 0)
			{
				shooting = true;
				int randomNumberForMuzzelFlash = Random.Range(0, 5);
				if (bullet)
				{
					//Instantiate(bullet, muzzel.transform.position, muzzel.transform.rotation);


					GameObject bullet_ = Instantiate(bullet, muzzel.transform.position, muzzel.transform.rotation) as GameObject;
					Rigidbody bulletRigidbody = bullet_.GetComponent<Rigidbody>();

					Vector3 direction = (muzzel.forward).normalized;
					bullet_.GetComponent<BulletScript>().direction = direction;
					bulletRigidbody.AddForce(direction * bulletImpulse, ForceMode.Impulse);
					waitTillNextFire = 1;
					bulletsInTheGun -= 1;

				}
				else
					print("Missing the bullet prefab");
			}

		}
		
	}
	public void Reload()
	{
		total_bullets -= magazine_size - bulletsInTheGun;
		bulletsInTheGun = magazine_size;
	}





}
