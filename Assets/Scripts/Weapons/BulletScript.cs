using UnityEngine;
using System.Collections;
using Photon.Pun;

public class BulletScript :MonoBehaviourPunCallbacks  {



	 private RaycastHit hit;
	private BulletAudio source;
	public GameObject decalHitWall;

	public float floatInfrontOfWall;
	public float damage;
	public string Shooter;
	public GameObject[] bloodEffects;
	public LayerMask ignoreLayer;
	public Vector3 direction;
	public float maxDistance;
	/*
	* Uppon bullet creation with this script attatched,
	* bullet creates a raycast which searches for corresponding tags.
	* If raycast finds somethig it will create a decal of corresponding tag.
	*/
	void Start()
	{
		source = GetComponent<BulletAudio>();
	}
	void Update () {
		
		if (Physics.Raycast(transform.position, direction,out hit, maxDistance, LayerMask.GetMask("Shootable"))){
			string hit_tag = hit.transform.gameObject.tag;
			switch (hit_tag)
			{
				case "Player":
					{
						Instantiate(bloodEffects[Random.Range(0,bloodEffects.Length-1)], hit.point, Quaternion.LookRotation(hit.normal));
					hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage, Shooter);
						source.Player();
						Destroy(gameObject,0.5f);
						break;
					}
				default:
					if (decalHitWall)
					{
							Instantiate(decalHitWall, hit.point + hit.normal * floatInfrontOfWall, Quaternion.LookRotation(hit.normal));
						source.Terrain(1);
						Destroy(gameObject,0.5f);
					}
					break;
			}		
		
		}
		Destroy(gameObject, 5f);
	}


}
