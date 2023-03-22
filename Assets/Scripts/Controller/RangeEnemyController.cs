using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float speedPatroll;
    [SerializeField] GameObject player;
    [SerializeField] float lineOfSight;
    [SerializeField] float shootRange;
    [SerializeField] GameObject bullet,shootPos;
    [SerializeField] float fireRate;
    float nextFireTime;
    private Rigidbody2D rb;
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update() {
        Chase();
    }
    void Chase(){
        float distantFromPlayer = Vector2.Distance(transform.position,player.transform.position);
        if(distantFromPlayer < lineOfSight &&  distantFromPlayer >shootRange){
        transform.position = Vector2.MoveTowards(this.transform.position,player.transform.position,speed *Time.deltaTime);
        }else if(distantFromPlayer <= shootRange && nextFireTime <Time.time){
            Instantiate(bullet,shootPos.transform.position,Quaternion.identity);
            nextFireTime = Time.time +fireRate;
        }
    }
    
    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        Gizmos.DrawWireSphere(transform.position, shootRange);

    }
}
