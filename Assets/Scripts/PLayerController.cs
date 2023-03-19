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
    [SerializeField] LayerMask WhatIsGround;
    private CamFollowPlayerCam _camFollowC;
    
    private void Awake()
    {
        mybody=GetComponent<Rigidbody2D>();
        _camFollowC = _camFollowCam.GetComponent<CamFollowPlayerCam>();
    }

    private void FixedUpdate() {
        if(MoveX>0||MoveX<0){
            TurnCheck();
        }
        Moving();
    }
    void Update()
    {
        IsGround = Physics2D.OverlapCircle(feetPos.position,feetRadius,WhatIsGround);
        Jumping();
        
    }
    void Moving()
    {
        MoveX = Input.GetAxisRaw("Horizontal");
        // transform.position += new Vector3(MoveX, 0f, 0f) * Time.deltaTime * MoveSpeed;
        mybody.velocity = new Vector2(MoveX * MoveSpeed, mybody.velocity.y);
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
}
