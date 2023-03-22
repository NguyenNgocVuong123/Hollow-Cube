using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject PointA;
    [SerializeField] GameObject PointB;
    [SerializeField] float speed;
    private Rigidbody2D rb;
    private Transform currentPoint;

    [SerializeField] GameObject player;
    [SerializeField]private float distantChase;
    private float chaseSpeed = 2f;
    private bool isChasing;
    [SerializeField] PLayerController pLayerCon;
    

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        pLayerCon = FindObjectOfType<PLayerController>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentPoint = PointB.transform;
    }
    private void Update() {
        if(Vector2.Distance(transform.position, player.transform.position) < distantChase){
            isChasing = true;
        }else{
            isChasing = false;
        }
        if(isChasing){
            Chasing();
        }else{
            Patrol();
        }
    }
    void Patrol(){
        Vector2 dir = currentPoint.position - transform.position;
        if(currentPoint == PointB.transform){
            rb.velocity = new Vector2(speed, 0);
        }else{
            rb.velocity = new Vector2(-speed,0);
        }
        if(Vector2.Distance(transform.position,currentPoint.position) <0.5f && currentPoint == PointB.transform){
            transform.localScale = new Vector3(-1,1,1);
            currentPoint  = PointA.transform;
        }
        if(Vector2.Distance(transform.position,currentPoint.position) <0.5f && currentPoint == PointA.transform){
            transform.localScale = new Vector3(1,1,1);
            currentPoint  = PointB.transform;
        }
    }
    void Chasing(){
        if(transform.position.x > player.transform.position.x){
            transform.localScale = new Vector3(1,1,1);
            transform.position += Vector3.left * chaseSpeed *Time.deltaTime;
        }
        if(transform.position.x < player.transform.position.x){
            transform.localScale = new Vector3(-1,1,1);
            transform.position += Vector3.right * chaseSpeed *Time.deltaTime;
        }
    }
    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(PointB.transform.position, 0.5f);
        Gizmos.DrawWireSphere(PointA.transform.position, 0.5f);
        Gizmos.DrawLine(PointA.transform.position,PointB.transform.position); 
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag =="Player"){
            pLayerCon.knockBackCounter = pLayerCon.knockBackTime;
            if(other.transform.position.x <= transform.position.x){
                pLayerCon.knockDirRight = true;
            }
            if(other.transform.position.x >= transform.position.x){
                pLayerCon.knockDirRight = false;
            }
        }
    }
}
