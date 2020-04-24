using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class GunInventory : MonoBehaviour
{
    public int index;
    public GameObject[] guns;
    public GameObject gun_new;
    private Animator animator;
    public Transform gun_pos;
    public GameObject selected_gun;
    private float time;
    void Start()
    {
        time = 0f;
        index = 0;
        selected_gun = guns[index];
        animator = GetComponent<Animator>();
        SpawnGun();
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 1.2f)
        {
            ChangeGun();
            selected_gun = guns[index];
      
        }
        time += Time.deltaTime;
    }
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
    void SpawnGun()
    {
       // if (gun_new==null)
        {
          var gun= Instantiate(selected_gun, gun_pos.position,gun_pos.rotation);
            gun.transform.parent = gun_pos;
            gun_new = gun;
        }
    }
}
