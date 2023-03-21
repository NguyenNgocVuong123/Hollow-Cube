using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int _health;
    public int _numOfheart;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private void Update() {
        if(_health >_numOfheart){
            _health = _numOfheart;
        }
        for (int i =0; i< hearts.Length;i++){
            if( i<_health){
                hearts[i].sprite = fullHeart;
            }else{
                hearts[i].sprite = emptyHeart;
            }
            if(i<_numOfheart){
                hearts[i].enabled = true;
            }else{
                hearts[i].enabled = false;
            }
        }
    }
    public void TakeDmg(int dmg){
        // if(Input.GetKeyDown(KeyCode.U)){
        //     _health -= 1;
        // }
        _health = _health - dmg;
        if(_health <= 0){
            _health = 0;
            Debug.Log("game Over");
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag=="Enemy"){
            TakeDmg(1);
        }
    }
}
