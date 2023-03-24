using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    GameManager gameManager;
    public LayerMask playerMask;
    private PLayerController player;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        player = FindObjectOfType<PLayerController>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        // if(other.CompareTag("Player"))
        //     SceneManager.LoadScene("Stage1");
        if(((1 << other.gameObject.layer) & playerMask) != 0){
            gameManager.SaveData();
            gameManager.LoadOnNextLevel();
            // player.transform.position = new Vector2(-13.97f,4f);
        }

    }
}
