using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 20f;
    [SerializeField] Rigidbody2D rb;
    private void Start() {
        rb.velocity = transform.right *speed;
    }
    private void OnTriggerEnter2D(Collider2D hitInfo) {
        Debug.Log(hitInfo.name);
        EnemyHealth enemyHealth = hitInfo.GetComponent<EnemyHealth>();
        if(enemyHealth != null){
        enemyHealth.TakeDmg(3);
        }
        RangeEnemyHealth rangeEnemyHealth = hitInfo.GetComponent<RangeEnemyHealth>();
        if(rangeEnemyHealth != null){
        rangeEnemyHealth.TakeDmg(3);
        }
        
        Destroy(gameObject);
    }
}
