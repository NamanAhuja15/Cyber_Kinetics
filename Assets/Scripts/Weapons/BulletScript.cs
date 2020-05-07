using UnityEngine;
using System.Collections;
using Photon.Pun;

public class BulletScript :MonoBehaviourPunCallbacks  {



	 private RaycastHit hit;
	public GameObject decalHitWall;

	public float floatInfrontOfWall;
	public int damage;
	public GameObject[] hitEffects;
	public GameObject[] bloodEffects;
	public LayerMask ignoreLayer;
	public Vector3 direction;
	public float maxDistance;
	private float time;
	/*
	* Uppon bullet creation with this script attatched,
	* bullet creates a raycast which searches for corresponding tags.
	* If raycast finds somethig it will create a decal of corresponding tag.
	*/
	void Start()
	{
		time = 0f;
	}
	void Update () {
		time += Time.deltaTime;
		if (Physics.Raycast(transform.position, direction,out hit, maxDistance, LayerMask.GetMask("Shootable"))){
			string hit_tag = hit.transform.gameObject.tag;
			switch (hit_tag)
			{
				case "Player":
					{
						
						if (hit.transform.gameObject.layer != LayerMask.GetMask("Player"))
						{
							Instantiate(bloodEffects[0], hit.point + hit.normal * floatInfrontOfWall, Quaternion.LookRotation(hit.normal));
							hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage, PhotonNetwork.LocalPlayer.NickName);
						}
						Destroy(this.gameObject);
						break;
					}
				default:
					if (decalHitWall)
					{
						Instantiate(decalHitWall, hit.point + hit.normal * floatInfrontOfWall, Quaternion.LookRotation(hit.normal));
					Destroy(this.gameObject);
					}
					break;
			}		
		
		}
		if (time > 5f)
			Destroy(this.gameObject);
	}


}
