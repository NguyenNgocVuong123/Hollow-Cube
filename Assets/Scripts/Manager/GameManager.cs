using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public SaveSystemManager saveSystem;

    private void Awake(){
        SceneManager.sceneLoaded += Initialized;
        DontDestroyOnLoad(gameObject);
    }
    private void Initialized(Scene scene, LoadSceneMode sceneMode){
        Debug.Log("a");
        var playerController = FindObjectOfType<PLayerController>();
        if(playerController != null)
            player = playerController.gameObject;
        saveSystem = FindObjectOfType<SaveSystemManager>();
        if(player != null && saveSystem._loadedData != null){
            var healthPlayer = player.GetComponent<Health>();
            healthPlayer._health = saveSystem._loadedData._playerHealth;
        } 
    }
    public void LoadLevel(){
        if(saveSystem._loadedData != null){
            SceneManager.LoadScene(saveSystem._loadedData._sceneIndex);
            return;
        }
        LoadOnNextLevel();
    }
    public void LoadOnNextLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void LoadOnBossLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
    public void LoadBackLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void SaveData(){
        if( player != null )
        saveSystem.SaveData(SceneManager.GetActiveScene().buildIndex + 1, player.GetComponent<Health>()._health);
    }
}
