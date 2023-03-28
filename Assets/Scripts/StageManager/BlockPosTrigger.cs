using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPosTrigger : MonoBehaviour
{
    public GameObject blockPos;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag =="Player"){
            Destroy(gameObject);
            blockPos.SetActive(true);
        }
    }
}
