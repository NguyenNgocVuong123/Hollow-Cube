using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Open2 : MonoBehaviour
{
    [SerializeField] private GameObject _door2;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag =="Player"){
            Destroy(gameObject);
            _door2.SetActive(false);
        }
    }
}
