using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private GameObject _bossDoor;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag =="Player"){
            Destroy(gameObject);
            _bossDoor.SetActive(false);
        }
    }
}
