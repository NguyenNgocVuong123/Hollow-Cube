using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Text _text;
    public GameObject player;
    public SaveSystemManager saveSystem;
    public GameObject gameOverUI;
    PLayerController PLayerCon;
    
    
    void MakeInstance(){
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }

    private void Awake(){
        SceneManager.sceneLoaded += Initialized;
        
        PLayerCon = FindObjectOfType<PLayerController>();
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

    public void GameOver(){
        Time.timeScale =0f;
        PLayerCon.enabled = false;
        gameOverUI.SetActive(true);
        _text.text ="Game Over";
    }
    public void Win(){
        Time.timeScale =0f;
        PLayerCon.enabled = false;
        gameOverUI.SetActive(true);
        _text.text ="You Win";
    }
    public void Home(){
        saveSystem.ResetData();
        SceneManager.LoadScene(SceneManager.GetSceneByName("Menu").buildIndex +1);
        Time.timeScale =1f;
        PLayerCon.enabled = true;
    }
    public void Restart(){
        saveSystem.ResetData();
        SceneManager.LoadScene(SceneManager.GetSceneByName("Menu").buildIndex +2);
        Time.timeScale =1f;
        PLayerCon.enabled = true;
    }
    public void Quit(){
        Application.Quit();
    }
}
