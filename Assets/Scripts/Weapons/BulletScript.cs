using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {


	public float maxDistance;
	 private RaycastHit hit;

	public GameObject decalHitWall;

	public float floatInfrontOfWall;

	public GameObject bloodEffect;

	public LayerMask ignoreLayer;
	public Vector3 direction;
	/*
	* Uppon bullet creation with this script attatched,
	* bullet creates a raycast which searches for corresponding tags.
	* If raycast finds somethig it will create a decal of corresponding tag.
	*/
	private void Start()
	{
		
	}
	void Update () {
		
		if (Physics.Raycast(transform.position, direction,out hit, maxDistance, LayerMask.GetMask("Shootable"))){
			string hit_tag = hit.transform.gameObject.tag;
			switch (hit_tag)
			{
				case "Player":
					Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
					hit.collider.gameObject.GetComponent<Player_Scripts.Player_Health>().health -= 10;
					Destroy(gameObject);
					break;
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
