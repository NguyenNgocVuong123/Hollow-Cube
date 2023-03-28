using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Setting")]
    [SerializeField] float _speed = 20f;
    [SerializeField] Rigidbody2D rb;
    private void Start() {
        rb.velocity = transform.right * _speed;
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
        BossHealth bossHealth = hitInfo.GetComponent<BossHealth>();
        if(bossHealth != null){
        bossHealth.TakeDmg(3);
        }
        Destroy(gameObject);
    }
}
