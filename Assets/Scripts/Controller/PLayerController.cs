using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PLayerController : MonoBehaviour
{
    [SerializeField]
    private float MoveSpeed = 10f,feetRadius,jumpHeight =11f, jumpTime;
    [SerializeField] Transform feetPos;
    [SerializeField] GameObject _camFollowCam;
    private float MoveX,jumpTimeCounter;
    private Rigidbody2D mybody;
    private bool IsGround,IsJumping;
    public bool IsFacingRight;
    float _fallSpeedYDampingChangeThres;
    [SerializeField] LayerMask WhatIsGround;
    private CamFollowPlayerCam _camFollowC;
    //
    private float currentMoveSpeed;
    public float dashSpeed;
    public float dashTime= 0.5f,dashCountDown = 1f;
    private float dashCounter;
    private float dashCountDownCounter;
    [SerializeField] TrailRenderer tr;
    [SerializeField] Animator animator;
    public Transform attackPos;
    public float attackRange = 0.5f;
    public float attackRate=2f;
    float nextAttackTime =0f;
    public LayerMask enemyLayer;
    [SerializeField] Transform rangeAttackPos;
    [SerializeField] GameObject bullet;
    [SerializeField] Image coolDownUI;
    [SerializeField] float coolDown =5f;
    private bool isCoolDown;

    public float knockBackForce;
    public float knockBackCounter;
    public float knockBackTime;
    public bool knockDirRight;
    
    private void Start() {
        _fallSpeedYDampingChangeThres = CameraManager.instance._fallSpeedYDampingChangeThreshold;
        mybody=GetComponent<Rigidbody2D>();
        _camFollowC = _camFollowCam.GetComponent<CamFollowPlayerCam>();
        currentMoveSpeed = MoveSpeed;
    }

    private void FixedUpdate() {
        if(MoveX>0||MoveX<0){
            TurnCheck();
        }
        if(knockBackCounter <= 0){
            Moving();
        }else{
            if(knockDirRight == true){
                mybody.velocity = new Vector2(-knockBackForce, knockBackForce);
            }
            if(knockDirRight == false){
                mybody.velocity = new Vector2(knockBackForce, knockBackForce);
            }
            knockBackCounter -= Time.deltaTime;
        }
        
        
    }
    void Update()
    {
        if(isCoolDown){
            coolDownUI.fillAmount += 1/coolDown *Time.deltaTime;
            if(coolDownUI.fillAmount >=1){
                coolDownUI.fillAmount = 0;
                isCoolDown = false;
            }
        }
        Debug.Log("cool: "+ isCoolDown);
        IsGround = Physics2D.OverlapCircle(feetPos.position,feetRadius,WhatIsGround);
        Jumping();
        Dashing();
        if(Time.time >= nextAttackTime){
            Attacking();   
        }
        if(Input.GetKeyDown(KeyCode.L) &&isCoolDown ==false){
            Shooting();
            isCoolDown = true; 
        }
        
        //camera
        //nếu đang rơi
        if(mybody.velocity.y <_fallSpeedYDampingChangeThres && !CameraManager.instance.IsLerpingYDamping &&!CameraManager.instance.LearpingFromPlayerFalling){
            CameraManager.instance.LerpYDamping(true);
        }
        //nếu đang đứng im hoặc di chuyển
        if(mybody.velocity.y >=0f &&!CameraManager.instance.IsLerpingYDamping &&CameraManager.instance.LearpingFromPlayerFalling){
            //đặt lại điều kiện lerp để có thể gọi lại lần nữa
            CameraManager.instance.LearpingFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false);
        }
        
    }
    void Moving()
    {
        MoveX = Input.GetAxisRaw("Horizontal");
        // transform.position += new Vector3(MoveX, 0f, 0f) * Time.deltaTime * MoveSpeed;
        mybody.velocity = new Vector2(MoveX * currentMoveSpeed, mybody.velocity.y);
        
    }

    void Jumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGround ==true)
        {
            mybody.velocity = new Vector2(mybody.velocity.x, jumpHeight);
            // mybody.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
            IsJumping = true;
            jumpTimeCounter = jumpTime;
        }
        if(Input.GetKey(KeyCode.Space)&& IsJumping == true){
            
            if(jumpTimeCounter>0){
                mybody.velocity = new Vector2(mybody.velocity.x, jumpHeight);
                jumpTimeCounter -= Time.deltaTime;
            }else{
                IsJumping = true;
            }
        }
        if(Input.GetKeyUp(KeyCode.Space)){
            IsJumping = false;
        }
        
    }
    
    void TurnCheck(){
        if(MoveX > 0 && !IsFacingRight){
            Turn();
        }else if( MoveX <0 && IsFacingRight){
            Turn();
        }
    }
    void Turn(){
        if(IsFacingRight){
            Vector3 rotator = new Vector3(transform.rotation.x, 180f,transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
            //quay cam theo playercam
            _camFollowC.CallTurn();

        }else{
            Vector3 rotator = new Vector3(transform.rotation.x, 0f,transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
            _camFollowC.CallTurn();
        }
    }
    void Dashing(){
        if(Input.GetKeyDown(KeyCode.K)){
            if(dashCountDownCounter <= 0 && dashCounter <=0){
                currentMoveSpeed = dashSpeed;
                dashCounter = dashTime;
                mybody.gravityScale = 0f;
                tr.emitting = true;
            }
        }
        if(dashCounter >0){
            dashCounter -= Time.deltaTime;
            if(dashCounter <= 0){
                currentMoveSpeed = MoveSpeed;
                dashCountDownCounter = dashCountDown;
                mybody.gravityScale = 5f;
                tr.emitting = false;
            }
        }
        if(dashCountDownCounter >0){
            dashCountDownCounter -= Time.deltaTime;
        }
    }
    void Attacking(){
        if(Input.GetKeyDown(KeyCode.J)){
            nextAttackTime = Time.time + 1f/attackRate;
            animator.SetTrigger("Attack");
            Collider2D[] hitEnemy= Physics2D.OverlapCircleAll(attackPos.position,attackRange,enemyLayer);
            foreach(Collider2D enemy in hitEnemy){
                if(enemy.CompareTag("Enemy"))
                    enemy.GetComponent<EnemyHealth>().TakeDmg(1);
                if(enemy.CompareTag("RangeEnemy"))
                    enemy.GetComponent<RangeEnemyHealth>().TakeDmg(1);
            }
        }  
    }
    void Shooting(){
        Instantiate(bullet,rangeAttackPos.position, rangeAttackPos.rotation);
    }
    private void OnDrawGizmosSelected() {
        if(attackPos == null)
            return;
        Gizmos.DrawWireSphere(attackPos.position,attackRange);
    }
    
}
