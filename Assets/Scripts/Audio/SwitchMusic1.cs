using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMusic1 : MonoBehaviour
{
    public AudioClip newTrack;
    private AudioManager audioManager;
    private void Start() {
        audioManager = FindObjectOfType<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            if(newTrack != null)
                audioManager.ChangeBGM(newTrack);
        }
        Destroy(gameObject);
    }
}
