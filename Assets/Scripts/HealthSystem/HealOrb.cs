using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOrb : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D hitInfo) {
        Health health = hitInfo.GetComponent<Health>();
        if(health != null){
        health.Healing(1);
        }
        Destroy(gameObject);
        
    }
}
