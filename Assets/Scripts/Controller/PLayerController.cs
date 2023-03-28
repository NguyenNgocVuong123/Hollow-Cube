using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PLayerController : MonoBehaviour
{
    #region var
    [Header("Player Setting")]
    [SerializeField] private float MoveSpeed = 10f,feetRadius,jumpHeight =11f, jumpTime;
    private float jumpTimeCounter;
    private float MoveX;
    private Rigidbody2D mybody;
    [Header("Jumping && Ground check")]
    [SerializeField] Transform feetPos;
    private bool IsGround,isStandPos,IsJumping;
    public bool IsFacingRight;
    [SerializeField] LayerMask WhatIsGround;
    [SerializeField] LayerMask WhatIsStandPos;
    [SerializeField] TrailRenderer tr;
    [SerializeField] Animator animator;
    //
    [Header("Dashing")]
    private float currentMoveSpeed;
    public float dashSpeed;
    public float dashTime= 0.5f,dashCountDown = 1f;
    private float dashCounter;
    private float dashCountDownCounter;
    [Header("Player Normal Attack")]
    public Transform attackPos;
    public float attackRange = 0.5f;
    public float attackRate=2f;
    float nextAttackTime =0f;
    public LayerMask enemyLayer;
    [Header("Fire Skill")]
    public Transform rangeAttackPos;
    [SerializeField] GameObject bullet;
    [SerializeField] Image coolDownUI;
    public float coolDown =5f;
    private bool isCoolDown;
    [Header("Player KnockBack When Get hit")]
    public float knockBackForce;
    public float knockBackCounter;
    public float knockBackTime;
    public bool knockDirRight;
    [Header("Camera")]
    private float _fallSpeedYDampingChangeThres;
    private CamFollowPlayerCam _camFollowC;
    public GameObject _camFollowCam;
    #endregion
    private void Start() {
        _fallSpeedYDampingChangeThres = CameraManager.instance._fallSpeedYDampingChangeThreshold;
        mybody=GetComponent<Rigidbody2D>();
        _camFollowC = _camFollowCam.GetComponent<CamFollowPlayerCam>();
        currentMoveSpeed = MoveSpeed;
    }
    private void FixedUpdate() {
        //flip player 
        if(MoveX>=0||MoveX<=0 ){
            TurnCheck();
        }
        //check knockback
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
    private void Update()
    {
        //UI fire ball
        if(isCoolDown){
            coolDownUI.fillAmount += 1/coolDown *Time.deltaTime;
            if(coolDownUI.fillAmount >=1){
                coolDownUI.fillAmount = 0;
                isCoolDown = false;
            }
        }
        //check ground bằng vi tri của ojb FeetPos, nếu ojb này chạm vào ojb có layer là Ground thì = true
        IsGround = Physics2D.OverlapCircle(feetPos.position,feetRadius,WhatIsGround);
        isStandPos = Physics2D.OverlapCircle(feetPos.position,feetRadius,WhatIsStandPos);
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
        //Điều khiển tương tác player
        Jumping();
        Dashing();
        if(Time.time >= nextAttackTime){
            Attacking();   
        }
        if(Input.GetKeyDown(KeyCode.L) &&isCoolDown ==false){
            Shooting();
            isCoolDown = true; 
        }
        
    }
    #region Player Action
    void Moving()
    {
        MoveX = Input.GetAxisRaw("Horizontal");
        // transform.position += new Vector3(MoveX, 0f, 0f) * Time.deltaTime * MoveSpeed;
        mybody.velocity = new Vector2(MoveX * currentMoveSpeed, mybody.velocity.y);
        
    }
    void Jumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (IsGround ==true || isStandPos == true))
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
    void Dashing(){
        if(Input.GetKeyDown(KeyCode.K)){
            if(dashCountDownCounter <= 0 && dashCounter <=0){
                currentMoveSpeed = dashSpeed;
                // mybody.AddForce(transform.right * dashSpeed, ForceMode2D.Impulse);
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
                if(enemy.CompareTag("Boss"))
                    enemy.GetComponent<BossHealth>().TakeDmg(1);
            }
        }  
    }
    void Shooting(){
        Instantiate(bullet,rangeAttackPos.position, rangeAttackPos.rotation);
    }
    #endregion
    #region flip player
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
    #endregion
    private void OnDrawGizmosSelected() {
        if(attackPos == null)
            return;
        Gizmos.DrawWireSphere(attackPos.position,attackRange);
    }
    
}
