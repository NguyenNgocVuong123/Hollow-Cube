using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeBullet : MonoBehaviour
{
    [Header("Bullet Setting")]
    private GameObject target;
    public float speed;
    public Rigidbody2D rb;

    private void Start() {
        target = GameObject.FindGameObjectWithTag("Player");
        Vector2 moveDir = (target.transform.position - transform.position).normalized *speed;
        rb.velocity = new Vector2(moveDir.x, moveDir.y);
    }
    private void OnTriggerEnter2D(Collider2D hitInfo) {
        Debug.Log(hitInfo.name);
        Health health = hitInfo.GetComponent<Health>();
        if(health != null){
        health.TakeDmg(1);
        }
        Destroy(gameObject);
        
    }
}
