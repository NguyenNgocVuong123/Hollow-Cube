using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{  
    public static CameraManager instance;
    [SerializeField] private CinemachineVirtualCamera[] _allVirtualCams;
    [SerializeField] float _fallDampingAmount = 0.25f;
    [SerializeField] float _fallYDampingTime = 0.35f;
    public float _fallSpeedYDampingChangeThreshold = -15f;

    public bool IsLerpingYDamping {get; private set;}
    public bool LearpingFromPlayerFalling {get;set;}

    private Coroutine _lerpYDampingCoroutine;
    private CinemachineFramingTransposer _framingTrans;
    private CinemachineVirtualCamera _currentCam;
    private float _normalYDampingAmount;
    private void Awake() {
        if(instance == null){
            instance = this;

        }
        for (int i =0; i< _allVirtualCams.Length;i++){
            if(_allVirtualCams[i].enabled){
                //đặt cam đang hoạt động vào
                _currentCam = _allVirtualCams[i];
                //đặt bộ chuyển đổi khung hình
                _framingTrans = _currentCam.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }
        //đặt Ydamping mặc định dựa theo cài đặt
        _normalYDampingAmount = _framingTrans.m_YDamping;
    }

    public void LerpYDamping(bool isPLayerFalling){
        _lerpYDampingCoroutine = StartCoroutine(LerpYAction(isPLayerFalling));
    }
    public IEnumerator LerpYAction(bool isPLayerFalling){
        IsLerpingYDamping = true;
        //đặt mức damp ban đầu
        float startDampingAmount = _framingTrans.m_YDamping;
        float endDampingAmount = 0f;
        //đặt khi nào thì dừng damping
        if(isPLayerFalling){
            endDampingAmount = _fallDampingAmount;
            LearpingFromPlayerFalling = true;
        }else{
            endDampingAmount = _normalYDampingAmount;
        }
        //lerp damp amount
        float elapsedTime = 0f;
        while(elapsedTime< _fallYDampingTime){
            elapsedTime += Time.deltaTime;
            float lerpDampingAmount = Mathf.Lerp(startDampingAmount, endDampingAmount, (elapsedTime /  _fallYDampingTime));
            _framingTrans.m_YDamping = lerpDampingAmount;
            yield return null;
        }
        IsLerpingYDamping = false;
        
    }
}