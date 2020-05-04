using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class GunInventory : MonoBehaviourPunCallbacks
{
    public int index;
    public GameObject[] guns;
    public GameObject gun_new;
    private Animator animator;
    public Transform gun_pos;
    public GameObject selected_gun;
    public Image crosshair;
    private float time;
    void Start()
    {
        time = 0f;
        index = 0;
        selected_gun = guns[index];
        animator = GetComponent<Animator>();
        crosshair = transform.GetComponentInChildren<Image>();
        SpawnGun();
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0.5f)
        {
            photonView.RPC("ChangeGun", RpcTarget.All); 
            selected_gun = guns[index];
      
        }
        time += Time.deltaTime;
    }
    [PunRPC]
    void ChangeGun()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            time = 0f;
                index++;
                animator.SetTrigger("Switch");
         
            if (index > guns.Length - 1)
                index = 0;
            Destroy(gun_new);
            SpawnGun();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            time = 0f;
            if (index - 1 >= 0)
            {
                index--;
                animator.SetTrigger("Switch");
                Destroy(gun_new);
                SpawnGun();
            }
            else
                index = 0;
        }
    }
    [PunRPC]
    void SpawnGun()
    {
        if (selected_gun.GetComponent<GunScript>())
        {
            var gun= PhotonNetwork.Instantiate(selected_gun.name,gun_pos.position, gun_pos.rotation, 0);
            gun.transform.parent = gun_pos;
            gun_new = gun;
        }
    }
}
