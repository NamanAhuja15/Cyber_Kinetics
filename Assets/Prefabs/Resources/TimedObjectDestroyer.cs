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
	public float lifeTime;

	// Use this for initialization
	void Start()
	{
		Destroy(this.gameObject, lifeTime);
	}
	/*void DestroyObject()
	{
		if (this.gameObject.TryGetComponent<PhotonView>(out var photonView))
			photonView.RPC("DestroyRPC", RpcTarget.All);
		else
			Destroy(this.gameObject);
	}
	[PunRPC]
	 public void DestroyRPC()
	{
		PhotonNetwork.Destroy(gameObject);
	}*/
}
