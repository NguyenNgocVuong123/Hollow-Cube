using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Open1 : MonoBehaviour
{
    [SerializeField]private GameObject _door1;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag =="Player"){
            Destroy(gameObject);
            _door1.SetActive(false);
        }
    }
}
