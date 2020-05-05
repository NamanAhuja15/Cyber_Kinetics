using UnityEngine;
using System.Collections;
using Photon.Pun;

public class BulletScript :MonoBehaviour  {



	 private RaycastHit hit;
	public GameObject decalHitWall;

	public float floatInfrontOfWall;
	public int damage;
	public string Shooter;
	public GameObject[] hitEffects;
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
	}
	void Update () {
		
		if (Physics.Raycast(transform.position, direction,out hit, maxDistance, LayerMask.GetMask("Shootable"))){
			string hit_tag = hit.transform.gameObject.tag;
			switch (hit_tag)
			{
				case "Player":
					{
						PhotonNetwork.Instantiate(bloodEffects[Random.Range(0,bloodEffects.Length-1)].name, hit.point, Quaternion.LookRotation(hit.normal));
						if(hit.transform.gameObject.layer!=LayerMask.GetMask("Player"))
					hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage, PhotonNetwork.LocalPlayer.NickName);
						Destroy(gameObject);
						break;
					}
				default:
					if (decalHitWall)
					{
							Instantiate(decalHitWall, hit.point + hit.normal * floatInfrontOfWall, Quaternion.LookRotation(hit.normal));
						Destroy(gameObject);
					}
					break;
			}		
		
		}
		Destroy(gameObject, 5f);
	}


}
