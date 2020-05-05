using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAudio : MonoBehaviour
{
    public AudioClip FireSound;
    public AudioClip Reloading;
    private AudioSource audioSource;
    public float fire_delay;
    private float time;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Fire()
    {
        if (time > fire_delay)
        {
            audioSource.PlayOneShot(FireSound);
           time = 0f;
        }
    }
    public void Reload()
    {
        if (time > fire_delay)
        {
            audioSource.PlayOneShot(Reloading);
            time = 0f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }
}
