using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyController : MonoBehaviour
{
    private GameObject player;
    [Header("Enemy Setting")]
    [SerializeField] float speed;
    [SerializeField] float speedPatroll;
    [SerializeField] float lineOfSight;
    [SerializeField] float shootRange;
    private Rigidbody2D rb;
    [Header("Enemy Shooting")]
    [SerializeField] GameObject bullet,shootPos;
    [SerializeField] float fireRate;
    float nextFireTime;
    
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update() {
        Chase();
    }
    private void Chase(){
        //tinh toan vi tri giua enemy va player
        float distantFromPlayer = Vector2.Distance(transform.position,player.transform.position);
        //neu trong tam phat hien nhung ngoai tam ban
        if(distantFromPlayer < lineOfSight &&  distantFromPlayer >shootRange){
        transform.position = Vector2.MoveTowards(this.transform.position,player.transform.position,speed *Time.deltaTime);
        }
        //neu trong tam ban va thoi gian giua moi phat ban
        else if(distantFromPlayer <= shootRange && nextFireTime <Time.time){
            Instantiate(bullet,shootPos.transform.position,Quaternion.identity);
            nextFireTime = Time.time +fireRate;
        }
    }
    //hien thi tam` ban, tam phat hien cua enemy
    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        Gizmos.DrawWireSphere(transform.position, shootRange);

    }
}
