using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartOpen : MonoBehaviour
{
    [SerializeField]private GameObject _door;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag =="Player"){
            Destroy(gameObject);
            _door.SetActive(false);
        }
    }
}
