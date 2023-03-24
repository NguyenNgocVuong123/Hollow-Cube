using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveSystemManager : MonoBehaviour
{
    [Header("Save Data Key")]
    public string currentHealth = "PlayerHealth",currentScene = "SceneIndex", currentSaveData = "SavePresent";
    public LoadedData _loadedData {get; private set; } //check savedata
    public UnityEvent<bool> OnDataLoadResult;//event onClick button Continue
    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
    private void Start() {
        var result = LoadData(); //if have loaded data
        Debug.Log(result);
        OnDataLoadResult?.Invoke(result);//then set the event of Continue button to true
    }
    public void ResetData(){//when click play == newgame, loaddata bay mau
        PlayerPrefs.DeleteKey(currentHealth);   
        PlayerPrefs.DeleteKey(currentScene);
        PlayerPrefs.DeleteKey(currentSaveData);
        _loadedData = null;
    }
    public bool LoadData(){
        if(PlayerPrefs.GetInt(currentScene) == 1){ //if there is save data
            _loadedData = new LoadedData();
            _loadedData._playerHealth = PlayerPrefs.GetInt(currentHealth);
            _loadedData._sceneIndex = PlayerPrefs.GetInt(currentScene);
            return true;
        }
        return false;
    }
    public void SaveData(int _sceneIndex, int _playerHealth){
        if(_loadedData == null)
            _loadedData = new LoadedData();
        Debug.Log("save");
        _loadedData._playerHealth = _playerHealth;
        _loadedData._sceneIndex = _sceneIndex;
        PlayerPrefs.SetInt(currentHealth,_playerHealth);
        PlayerPrefs.SetInt(currentScene,_sceneIndex);
        PlayerPrefs.SetInt(currentSaveData, 1);
    }
}

public class LoadedData{
    public int _playerHealth = -1;
    public int _sceneIndex = -1;
}
