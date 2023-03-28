using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text _text;
    public GameObject player;
    public SaveSystemManager saveSystem;
    public GameObject gameOverUI;
    public Animator animator;
    PLayerController _pLayerCon;
    public AudioClip newTrack;
    private AudioManager audioManager;

    private void Awake(){
        SceneManager.sceneLoaded += Initialized;
        _pLayerCon = FindObjectOfType<PLayerController>();
        animator = GetComponentInChildren<Animator>();
        audioManager = FindObjectOfType<AudioManager>();
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
        audioManager.ChangeBGM(newTrack);
        if(saveSystem._loadedData != null){
            SceneManager.LoadScene(saveSystem._loadedData._sceneIndex);
        }
        LoadOnNextLevel();
    }
    public void LoadOnNextLevel(){
        
        animator.SetBool("IsStart", true);
        Invoke("LoadWait",1f);
    }
    private void LoadWait(){
        animator.SetBool("IsStart", false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void SaveData(){
        if( player != null )
        saveSystem.SaveData(SceneManager.GetActiveScene().buildIndex + 1, player.GetComponent<Health>()._health);
    }

    public void GameOver(){
        Time.timeScale =0f;
        _pLayerCon.enabled = false;
        gameOverUI.SetActive(true);
        _text.text ="Game Over";
    }
    public void Win(){
        Time.timeScale =0f;
        _pLayerCon.enabled = false;
        gameOverUI.SetActive(true);
        _text.text ="You Win";
    }
    public void Home(){
        saveSystem.ResetData();
        SceneManager.LoadScene(SceneManager.GetSceneByName("Menu").buildIndex +1);
        Time.timeScale =1f;
        _pLayerCon.enabled = true;
        audioManager.ChangeBGM(newTrack);
    }
    public void Restart(){
        saveSystem.ResetData();
        SceneManager.LoadScene(SceneManager.GetSceneByName("Menu").buildIndex +2);
        Time.timeScale =1f;
        _pLayerCon.enabled = true;
    }
    public void Quit(){
        Application.Quit();
    }
}
