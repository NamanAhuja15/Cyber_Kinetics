using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon;
public class GunScript : MonoBehaviourPunCallbacks,IPunObservable
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
	private RaycastHit hit;
	private float waitTillNextFire;
	public Image crosshair;


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

	private string Shooter;
	private GunAudio gunAudio;
	private GunInventory inventory;
	public Transform gun_pos;
	private Vector3 position;
	private Quaternion rotation;
	private float smoothing = 10.0f;
	private float time = 0f;
	private GameObject gun_Position;
	void Start()
    {
			gun_Position = GameObject.FindGameObjectWithTag("Gun_Pos");
			gun_pos = gun_Position.transform;
			SetParent();


		reloading = false;
		shooting = false;
		flash.SetActive(false);
		inventory = GetComponentInParent<GunInventory>();
		if (inventory.gun_new == null)
			inventory.gun_new = this.gameObject;
		if (photonView.IsMine)
		{
			crosshair = GameObject.FindGameObjectWithTag("Crosshair").GetComponent<Image>();
		}
	//	crosshair = inventory.crosshair;
		gunAudio = GetComponent<GunAudio>();
		raycastStartSpot = muzzle;
	}

	// Update is called once per frame
	void SetParent()
	{
		transform.parent = gun_pos;
		inventory = GetComponentInParent<GunInventory>();
		if (inventory.gun_new == null)
			inventory.gun_new = this.gameObject;
		gun_Position.tag = "Untagged";
	}
	void Update()
	{

		if (!beaming)
			photonView.RPC("StopBeam", RpcTarget.All); 
		if (!shooting)
		{
			photonView.RPC("StopFlash", RpcTarget.All);
		}
		else
			flash.SetActive(true);


		if (!photonView.IsMine)
		{
			transform.localRotation = rotation;
			transform.localPosition = position;
		}

			time = +Time.deltaTime;

		if (photonView.IsMine)
		{
			if (Input.GetMouseButtonUp(0))
			{
				shooting = false;
				beaming = false;
			}
			transform.LookAt(lookat);
			if (Input.GetMouseButton(0))
			{
				photonView.RPC("RPC_Shooting", RpcTarget.All);
			}
			if (crosshair != null)
			{
				Ray ray = Camera.main.ScreenPointToRay(crosshair.transform.position);
				if (Physics.Raycast(ray, out hit))
				{
					if (hit.collider.gameObject)
					{
						hit_dir = hit.point;
					}
					else
						hit_dir = ray.GetPoint(100);
				}
			}
		}
	}

	[PunRPC]
	void RPC_Shooting()
	{

		if (weapon == WeaponType.Rifle)
		{
			RifleShootMethod();

		
			waitTillNextFire -= roundsPerSecond * Time.deltaTime;
		}
		else if (weapon == WeaponType.Beam && beamHeat <= maxBeamHeat && !coolingDown)
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

		if (Input.GetMouseButtonUp(0))
		{
			shooting = false;
			beaming = false;

		}
		if (!beaming && weapon == WeaponType.Beam)
		{
			StopBeam();
		}
		if (weapon == WeaponType.Rifle)
		{
			if (shooting)
				flash.SetActive(true);
			else
				flash.SetActive(false);

		}

	}

	[PunRPC]
	public void StopFlash()
	{
		flash.SetActive(false);
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
					gunAudio.Fire();
					GameObject bullet_ = Instantiate(bullet, muzzle.transform.position, muzzle.transform.rotation) as GameObject;
					Rigidbody bulletRigidbody = bullet_.GetComponent<Rigidbody>();
					Vector3 direction = hit_dir - muzzle.transform.position ;
					bullet_.GetComponent<BulletScript>().direction = direction;
					bullet_.GetComponent<BulletScript>().Shooter=Shooter;
					bulletRigidbody.AddForce(direction * bulletImpulse, ForceMode.Impulse);
					waitTillNextFire = 1;
					bulletsInTheGun -= 1;
					
				}
				else
					print("Missing the bullet prefab");
			}

		}
		
	}
	//[PunRPC]
	public void BeamShootMethod()
	{
		gunAudio.Fire();
		beamHeat += Time.deltaTime;
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
					PhotonNetwork.Instantiate(hitEffect.name, beamhit.point, Quaternion.FromToRotation(Vector3.up, beamhit.normal));
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
					PhotonNetwork.Instantiate(muzfx.name, reflectionPoints[i], muzzle.rotation);
				}
			}

		}
		GameObject muzfx_ = muzzleEffects[Random.Range(0, muzzleEffects.Length)];
		if (muzfx_ != null)
		{
			GameObject mfxGO = PhotonNetwork.Instantiate(muzfx_.name, muzzle.position, muzzle.rotation) as GameObject;
			mfxGO.transform.parent = raycastStartSpot;
		}
		beaming = true;
	}
	[PunRPC]
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
	[PunRPC]
	public void Reload()
	{
		gunAudio.Reload();
		total_bullets -= magazine_size - bulletsInTheGun;
		bulletsInTheGun = magazine_size;
	}


	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(transform.localPosition);
			stream.SendNext(transform.localRotation);
		}
		else
		{
			position = (Vector3)stream.ReceiveNext();
			rotation = (Quaternion)stream.ReceiveNext();
		}
	}

}
