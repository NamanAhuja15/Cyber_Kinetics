using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] FootSteps;
    public AudioClip[] JumpSounds; 
    public AudioClip[] LandSounds;
    public AudioClip[] HurtSounds;
    private AudioSource audioSource;
  private  int index = 0;
    private float time=0f;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void Walk(float delay)
    {
        if (time > delay/2)
        {
            audioSource.PlayOneShot(FootSteps[Random.Range(0, FootSteps.Length-1)]);
            time = 0f;
        }
    }
    public void Jump()
    {
        audioSource.PlayOneShot(JumpSounds[Random.Range(0, JumpSounds.Length-1)]);
    }
    public void Land()
    {
        audioSource.PlayOneShot(LandSounds[Random.Range(0, LandSounds.Length-1)]);
    }
    public void Hurt()
    {
        audioSource.PlayOneShot(HurtSounds[Random.Range(0, HurtSounds.Length-1)]);
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }
}
