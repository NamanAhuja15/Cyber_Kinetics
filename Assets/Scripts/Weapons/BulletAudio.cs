using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAudio : MonoBehaviour
{
    private AudioSource source;
    public AudioClip[] Blood;
    public AudioClip[] Ground;
    public AudioClip[] Water;
    public AudioClip[] Metal;
    public AudioClip[] Wood;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Player()
    {
        source.PlayOneShot(Blood[Random.Range(0, Blood.Length - 1)]);
    }
    public void Terrain(int index)
    {
        switch(index)
        {
            case 1:
                {
                    source.PlayOneShot(Ground[Random.Range(0, Ground.Length - 1)]);
                    break;
                }
            case 2:
                {
                    source.PlayOneShot(Wood[Random.Range(0, Wood.Length - 1)]);
                    break;
                }
            case 3:
                {
                    source.PlayOneShot(Metal[Random.Range(0, Metal.Length - 1)]);
                    break;
                }
            case 4:
                {
                    source.PlayOneShot(Water[Random.Range(0, Water.Length - 1)]);
                    break;
                }
        }
    }

}
