using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageManager : MonoBehaviour
{
    GameManager gameManager;
    public LayerMask playerMask;
    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        // if(other.CompareTag("Player"))
        //     SceneManager.LoadScene("Stage1");
        if(((1 << other.gameObject.layer) & playerMask) != 0){
            gameManager.SaveData();
            gameManager.LoadOnBossLevel();
        }

    }
}
