using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GunScript : MonoBehaviour
{
    public Transform muzzel, LeftHand,RightHand,LeftElbow,RightElbow,LookObj;

	public float roundsPerSecond, bulletsInTheGun, bulletImpulse, magazine_size,total_bullets;
  
	public bool reloading;

	public GameObject bullet;
	public GameObject flash;	


	public bool shooting;
	public Transform lookat;
	public Image crosshair;
	private float waitTillNextFire;
	 private float offset;
	private GameObject aim_control;
	private Vector3 hit_dir;
	private Ray ray;
	private RaycastHit hit;
	void Start()
    {
		reloading = false;
		shooting = false;
		flash.SetActive(false);
		aim_control = GameObject.FindGameObjectWithTag("Fire");
		crosshair = GameObject.FindGameObjectWithTag("Crosshair").GetComponent<Image>();
	}

    // Update is called once per frame
    void Update()
    {
		waitTillNextFire -= roundsPerSecond * Time.deltaTime;
		transform.LookAt(lookat);
		ray = Camera.main.ScreenPointToRay(crosshair.transform.position);
		if(Physics.Raycast(ray,out hit))
		{
			if (hit.collider.gameObject)
			{
				hit_dir = hit.point;
			}
			else
				hit_dir = ray.GetPoint(100);

		}
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

					Vector3 direction = hit_dir - muzzel.transform.position ;
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
