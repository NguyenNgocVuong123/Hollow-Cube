using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource BGM;
    private void Start() {
        BGM = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeBGM(AudioClip music){
        BGM.Stop();
        BGM.clip = music;
        BGM.Play();
    }
}
