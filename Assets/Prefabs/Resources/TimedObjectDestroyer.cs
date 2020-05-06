/// <summary>
/// TimedObjectDestroyer.cs
/// Author: MutantGopher
/// This script destroys a GameObject after the number of seconds specified in
/// the lifeTime variable.  Useful for things like explosions and rockets.
/// </summary>

using UnityEngine;
using System.Collections;
using Photon.Pun;
public class TimedObjectDestroyer : MonoBehaviourPunCallbacks
{
	public float lifeTime = 10.0f;

	// Use this for initialization
	void Start()
	{

		Invoke("DestroyObject", lifeTime);
	}
	void DestroyObject()
	{
		photonView.RPC("DestroyRPC", RpcTarget.All);
	}
	[PunRPC]
	 public void DestroyRPC()
	{
		PhotonNetwork.Destroy(gameObject);
	}
}
