using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class GunInventory : MonoBehaviourPunCallbacks { 
    public int index;
    private int clone;
    public GameObject[] guns;
    public GameObject gun_new=null;
    private Animator animator;
    public Transform gun_pos;
    public GameObject selected_gun;
    public Image crosshair;
    private PlayerHealth player;
    private float time;
    void Start()
    {
        time = 0f;
        index = 0;
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerHealth>();
        crosshair = player.crosshair;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0.5f)
        {
            photonView.RPC("ChangeGun", RpcTarget.All); 
        }
        time += Time.deltaTime; 
    }

    [PunRPC]
    void ChangeGun()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                time = 0f;
                index++;
                animator.SetTrigger("Switch");

                if (index > guns.Length - 1)
                    index = 0;
                selected_gun = guns[index];

                PhotonNetwork.Destroy(gun_new);
                photonView.RPC("SpawnGun", RpcTarget.All);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                time = 0f;
                if (index - 1 >= 0)
                {
                    index--;
                    animator.SetTrigger("Switch");
                    selected_gun = guns[index];

                    PhotonNetwork.Destroy(gun_new);
                    photonView.RPC("SpawnGun", RpcTarget.All);
                }
                else
                    index = 0;
            }
            if (gun_new == null)
            {
                selected_gun = guns[index];
                photonView.RPC("SpawnGun", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void SpawnGun()
    {
        gun_pos.gameObject.tag = "Gun_Pos";
            var gun= PhotonNetwork.Instantiate(selected_gun.name,gun_pos.position, gun_pos.rotation,0);
            gun.transform.parent = gun_pos;
          gun.GetComponent<GunScript>().crosshair = crosshair;
          gun.GetComponent<GunScript>().gun_pos = gun_pos;
            gun_new = gun;

    }


}
