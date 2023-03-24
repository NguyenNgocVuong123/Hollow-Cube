using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBackManager : MonoBehaviour
{
    GameManager gameManager;
    SaveSystemManager saveSystem;
    public LayerMask playerMask;
    private GameObject player;
    private string sceneIndex;
    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        saveSystem = FindObjectOfType<SaveSystemManager>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        // if(other.CompareTag("Player"))
        //     SceneManager.LoadScene("Stage1");
        if(((1 << other.gameObject.layer) & playerMask) != 0){
            gameManager.SaveData();
            gameManager.LoadBackLevel();
            // if(PlayerPrefs.GetInt(saveSystem.currentScene) == 2)
            //     player.transform.position = new Vector3(-13.97f,20.53f,0);
            // else if(PlayerPrefs.GetInt(saveSystem.currentScene) == 1)
            //     player.transform.position = new Vector3(-13.97f,20.53f,0);
        }

    }
}
