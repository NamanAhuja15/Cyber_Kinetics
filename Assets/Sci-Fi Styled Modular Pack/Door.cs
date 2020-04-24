using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;
    private GameObject player;
    public bool open;
    void Start()
    {
        player = GameObject.Find("Player");
        open = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
     void Update()
    {
        if(Vector3.Distance(player.transform.position,transform.position)<=10f)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if (!open)
                    open = true;
                else
                    open = false;
            }
        }
        animator.SetBool("character_nearby", open);
    }


}
