using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : MonoBehaviour
{
    public AudioClip[] projectileSounds;
    public AudioClip[] hitSounds;
    public AudioClip[] punchSounds;
    public AudioSource audioSource;
    public AudioClip currentSound;
    public AudioClip currentPunchSound;
    public AudioClip shieldSound;
    public float timer;
    private float currentTimer;

    void Start()
    {
    }
    void Update()
    {
        currentTimer -= Time.deltaTime;
    }

    public void GettingHitManager()
    {
        currentTimer = -1f;
        currentSound = hitSounds[Random.Range(0, hitSounds.Length)];
        currentTimer = timer;
    }

    public void DodgeSound()
    {
        currentSound = projectileSounds[Random.Range(0, projectileSounds.Length)];
        currentTimer = timer;
    }    
    public void ShieldSound()
    {
        audioSource.PlayOneShot(shieldSound);
    }    
    public void PunchSound()
    {
        currentPunchSound = punchSounds[Random.Range(0, punchSounds.Length)];
        audioSource.PlayOneShot(currentPunchSound);
    }

    public void PlayCurrentSound()
    {
        audioSource.PlayOneShot(currentSound);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (currentTimer <= 0)
        {
            DodgeSound();
            Invoke("PlayCurrentSound", 0.1f);
            currentTimer = timer;
        }
    }
}
