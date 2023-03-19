using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowPlayerCam : MonoBehaviour
{
    [SerializeField] Transform _playerTrans;
    PLayerController _pLayer;
    [SerializeField] float _flipYRotationTime = 0.5f;
    Coroutine _turnCoroutine;
    bool _IsFacingRight;
    private void Awake() {
        _pLayer = _playerTrans.gameObject.GetComponent<PLayerController>();
        _IsFacingRight = _pLayer.IsFacingRight;
    }
    private void Update() {
        //điều khiển cam hướng theo vị trí của player
        transform.position =_playerTrans.position;
    }

    public void CallTurn(){
        _turnCoroutine = StartCoroutine(FlipYLerp());
    }

    IEnumerator FlipYLerp(){
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = SetEnndRotation();
        float yRotation =0f;

        float elapsedTime =0f;
        while(elapsedTime <_flipYRotationTime){
            elapsedTime += Time.deltaTime;
            //lerp trục y
            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime/ _flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }
    }

    float SetEnndRotation(){
        _IsFacingRight = !_IsFacingRight;
        if(_IsFacingRight){
            return 180f;
        }else{
            return 0f;
        }
    }
}
