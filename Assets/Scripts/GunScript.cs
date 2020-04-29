using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GunScript : MonoBehaviour
{
	//IK position variables
    public Transform muzzle, LeftHand,RightHand,LeftElbow,RightElbow,LookObj;

	public enum WeaponType { Beam,Rifle}
	public WeaponType weapon;
	//Rifle variables
	public float roundsPerSecond, bulletsInTheGun, bulletImpulse, magazine_size,total_bullets;
	public GameObject bullet;
	public bool shooting;
	private Vector3 hit_dir;
	private Ray ray;
	private RaycastHit hit;
	private float waitTillNextFire;
	private Image crosshair;


	//Beam variables
	public bool reloading;
	public GameObject[] muzzleEffects;
	public GameObject[] hitEffects;
	public GameObject flash;
	public GameObject beamGO;
	public float  maxBeamHeat,range,maxReflections;
	private float beamHeat;
	public Transform lookat;
	public Material beamMaterial;
	public Color beamColor = Color.red;
	private bool coolingDown,beaming;
	public float startBeamWidth = 0.5f;             
	public float endBeamWidth = 1.0f;
	public Transform raycastStartSpot;

	public AudioSource audioSource;
	void Start()
    {
		reloading = false;
		shooting = false;
		flash.SetActive(false);
		crosshair = GameObject.FindGameObjectWithTag("Crosshair").GetComponent<Image>();
		raycastStartSpot = muzzle;
	}

    // Update is called once per frame
    void Update()
    {
		transform.LookAt(lookat);

		if(Input.GetMouseButton(0))
		{
			if(weapon==WeaponType.Rifle)
			{
				RifleShootMethod();
				waitTillNextFire -= roundsPerSecond * Time.deltaTime;
			}
			else if (weapon == WeaponType.Beam&& beamHeat <= maxBeamHeat && !coolingDown)
		    {
				BeamShootMethod();
				if (beamHeat >= maxBeamHeat)
				{
					coolingDown = true;
				}
				else if (beamHeat <= maxBeamHeat - (maxBeamHeat / 2))
				{
					coolingDown = false;
				}
			}
		}
		if(Input.GetMouseButtonUp(0))
		{
			shooting = false;
			beaming = false;
			
		}
		if(!beaming)
			StopBeam();

		if (weapon == WeaponType.Rifle)
		{
			if (shooting)
				flash.SetActive(true);
			else if (!shooting)
				flash.SetActive(false);
		}

		ray = Camera.main.ScreenPointToRay(crosshair.transform.position);
		if (Physics.Raycast(ray,out hit))
		{
			if (hit.collider.gameObject)
			{
				hit_dir = hit.point;
			}
			else
				hit_dir = ray.GetPoint(100);
		}
    }

	public void RifleShootMethod()
	{
		if (waitTillNextFire <= 0 && !reloading)
		{

			if (bulletsInTheGun > 0)
			{
				shooting = true;
				if (bullet)
				{
					GameObject bullet_ = Instantiate(bullet, muzzle.transform.position, muzzle.transform.rotation) as GameObject;
					Rigidbody bulletRigidbody = bullet_.GetComponent<Rigidbody>();
					Vector3 direction = hit_dir - muzzle.transform.position ;
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
	public void BeamShootMethod()
	{
		beamHeat += Time.deltaTime;
		Debug.Log(beamHeat);
		int reflections = 0;
		List<Vector3> reflectionPoints = new List<Vector3>();
		reflectionPoints.Add(raycastStartSpot.position);
		Vector3 lastPoint = raycastStartSpot.position;
		Vector3 incomingDirection;
		Vector3 reflectDirection;
		bool keepReflecting = true;
		Ray ray = new Ray(lastPoint, hit_dir - raycastStartSpot.position);
		RaycastHit beamhit;
		if (beamGO == null)
		{
			beamGO = new GameObject("beamLR", typeof(LineRenderer));
			// Make the beam object a child of the weapon object, so that when the weapon is deactivated the beam will be as well	- was beamGO.transform.SetParent(transform), which only works in Unity 4.6 or newer;
			beamGO.transform.parent = transform;
		}
			LineRenderer beamLR = beamGO.GetComponent<LineRenderer>();
			beamLR.material = beamMaterial;			
			beamLR.material.SetColor("_TintColor", beamColor);
			beamLR.startWidth = startBeamWidth;
			beamLR.endWidth = endBeamWidth;
		
		do
		{
			Vector3 nextPoint = ray.direction * range;
			if (Physics.Raycast(ray, out beamhit, range))
			{
				nextPoint = beamhit.point;
				incomingDirection = nextPoint - lastPoint;
				reflectDirection = Vector3.Reflect(incomingDirection, beamhit.normal);
				ray = new Ray(nextPoint, reflectDirection);
				lastPoint = beamhit.point;
				// Hit Effects
				foreach (GameObject hitEffect in hitEffects)
				{
					if (hitEffect != null)
						Instantiate(hitEffect, beamhit.point, Quaternion.FromToRotation(Vector3.up, beamhit.normal));
				}
				reflections++;
			}
			else
			{

				keepReflecting = false;
			}
			reflectionPoints.Add(nextPoint);
		} while (keepReflecting && reflections < maxReflections);

		beamLR.positionCount = reflectionPoints.Count;
		for (int i = 0; i < reflectionPoints.Count; i++)
		{
			beamLR.SetPosition(i, reflectionPoints[i]);
			if (i > 0)    
			{
				GameObject muzfx = muzzleEffects[Random.Range(0, muzzleEffects.Length)];
				if (muzfx != null)
				{
					Instantiate(muzfx, reflectionPoints[i], muzzle.rotation);
				}
			}

		}
		GameObject muzfx_ = muzzleEffects[Random.Range(0, muzzleEffects.Length)];
		if (muzfx_ != null)
		{
			GameObject mfxGO = Instantiate(muzfx_, muzzle.position, muzzle.rotation) as GameObject;
			mfxGO.transform.parent = raycastStartSpot;
		}
		beaming = true;
	}
	public void StopBeam()
	{
		beamHeat -= Time.deltaTime;
		if (beamHeat < 0)
			beamHeat = 0;
	//	GetComponent<AudioSource>().Stop();

		if (beamGO != null)
		{
			Destroy(beamGO);
		}

	}
	public void Reload()
	{
		total_bullets -= magazine_size - bulletsInTheGun;
		bulletsInTheGun = magazine_size;
	}





}
