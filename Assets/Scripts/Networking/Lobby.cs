using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour
{
    public GameObject player;
    private bool animate;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player.GetComponent<Animator>().SetBool("Dance", animate);
    }
    public void Animate()
    {
        animate = true;
    }
    public void StopAnimate()
    {
        animate = false;
    }
}
