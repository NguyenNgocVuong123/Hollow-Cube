using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    private void Awake()
    {
        mybody=GetComponent<Rigidbody2D>();
        _camFollowC = _camFollowCam.GetComponent<CamFollowPlayerCam>();
        _fallSpeedYDampingChangeThres = CameraManager.instance._fallSpeedYDampingChangeThreshold;
        currentMoveSpeed = MoveSpeed;
    }

    private void FixedUpdate() {
        if(MoveX>0||MoveX<0){
            TurnCheck();
        }
        Moving();
        
    }
    void Update()
    {
        Debug.Log("moveX: "+ MoveX);
        IsGround = Physics2D.OverlapCircle(feetPos.position,feetRadius,WhatIsGround);
        Jumping();
        Dashing();
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
}
