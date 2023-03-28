using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Other")]
    private GameObject player;
    [Header("Phase2")]
    [SerializeField] Transform goundCheckPos;
    [SerializeField] Transform wallCheckPos;
    [SerializeField] LayerMask whatIsGround;
    public float _cirRadius;
    public GameObject standPos;
    [Header("Boss Setting")]
    [SerializeField] float _speedP1;
    [SerializeField] float _speedP2;
    [SerializeField] float _lineOfSight;
    [SerializeField] float _shootRange;
    private float _moveDir = 1;
    private bool IsFacingRight;
    private bool checkGround;
    private bool checkWall;
    private Rigidbody2D rb;
    [Header("Phase 1 and Phase 1 Plus")]
    [SerializeField] GameObject bullet,shootPos;
    [SerializeField] float _fireRate;
    private BossHealth _bossHealth;
    float _nextFireTime; 
    [Header("KnockBack Player")]
    [SerializeField] PLayerController pLayerCon;
    

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        pLayerCon = FindObjectOfType<PLayerController>();
        _bossHealth = GetComponent<BossHealth>();
    }

    private void Update() {
        checkGround = Physics2D.OverlapCircle(goundCheckPos.position, _cirRadius, whatIsGround);
        checkWall = Physics2D.OverlapCircle(wallCheckPos.position, _cirRadius, whatIsGround);
        // rb.gravityScale = 1f;
        // standPos.SetActive(false);
        // Phase2();
        if(_bossHealth.currentHealth > 20){
            Phase1();
            return;
        }
        if(_bossHealth.currentHealth >= 10){
            Phase1Plus();
            return;
        }
        if(_bossHealth.currentHealth < 10){
            rb.gravityScale = 1f;
            standPos.SetActive(false);
            Phase2();
        }
        
    }
    private void Phase1(){
        //tinh toan vi tri giua enemy va player
        float distantFromPlayer = Vector2.Distance(transform.position,player.transform.position);
        //neu trong tam phat hien nhung ngoai tam ban
        if(distantFromPlayer < _lineOfSight &&  distantFromPlayer > _shootRange){
        transform.position = Vector2.MoveTowards(this.transform.position,player.transform.position, _speedP1 *Time.deltaTime);
        }
        //neu trong tam ban va thoi gian giua moi phat ban
        else if(distantFromPlayer <= _shootRange && _nextFireTime <Time.time){
            Instantiate(bullet,shootPos.transform.position,Quaternion.identity);
            _nextFireTime = Time.time + _fireRate;
        }
    }
    private void Phase1Plus(){
        //tinh toan vi tri giua enemy va player
        float distantFromPlayer = Vector2.Distance(transform.position,player.transform.position);
        //neu trong tam phat hien nhung ngoai tam ban
        if(distantFromPlayer < _lineOfSight &&  distantFromPlayer > _shootRange){
        transform.position = Vector2.MoveTowards(this.transform.position,player.transform.position, _speedP1*_speedP1 *Time.deltaTime);
        }
        //neu trong tam ban va thoi gian giua moi phat ban
        else if(distantFromPlayer <= _shootRange && _nextFireTime <Time.time){
            Instantiate(bullet,shootPos.transform.position,Quaternion.identity);
            _nextFireTime = Time.time + _fireRate - 1;
        }
    }
    private void Phase2(){
        if(!checkGround || checkWall)
        {
            if(IsFacingRight){
                Flip();
                transform.localScale = new Vector3(1,1,1);
            }
            else if(!IsFacingRight)
            {
                Flip();
                transform.localScale = new Vector3(-1,1,1);
            }
        }
        rb.velocity = new Vector2(_speedP2 *_moveDir, rb.velocity.y);
    }
    private void Flip(){
        _moveDir *= -1;
        IsFacingRight = !IsFacingRight;
        
    }
    //hien thi tam` ban, tam phat hien cua enemy
    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, _lineOfSight);
        Gizmos.DrawWireSphere(transform.position, _shootRange);
        Gizmos.DrawWireSphere(wallCheckPos.position, _cirRadius);
        Gizmos.DrawWireSphere(goundCheckPos.position, _cirRadius);
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
