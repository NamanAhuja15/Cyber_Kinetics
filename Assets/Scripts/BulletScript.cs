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
		
		if (Physics.Raycast(transform.position, direction,out hit, maxDistance, ~ignoreLayer)){
			if(decalHitWall){
					if(hit.collider.gameObject.transform.tag == "Level")
				{
					Instantiate(decalHitWall, hit.point + hit.normal * floatInfrontOfWall, Quaternion.LookRotation(hit.normal));
				}
				if(hit.transform.tag == "Dummie"){
					Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
					Destroy(gameObject);
				}
				
			}		
		
		}
		Destroy(gameObject, 2.5f);
	}


}
