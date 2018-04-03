using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {

    private float collisionEffectSound = 1f;

    public float audioFootVolume = 1f;
    public float soundEffectPitchRandomness = 0.05f;

    private AudioSource audioSource;
    public AudioClip genericFootSound;
    public AudioClip medalFootSound;

	void Awake () {
        audioSource = GetComponent<AudioSource>();
	}

    void FootSound() {
        audioSource.volume = collisionEffectSound * audioFootVolume;
        audioSource.pitch = Random.Range(1.0f - soundEffectPitchRandomness, 1.0f + soundEffectPitchRandomness);

        if(Random.Range(0f, 2f) > 0) {
            audioSource.clip = genericFootSound;
        } else {
            audioSource.clip = medalFootSound;
        }
        audioSource.Play();
    }
	
}
