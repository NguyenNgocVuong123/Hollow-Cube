using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyHealth : MonoBehaviour
{
    [SerializeField] int Health;
    [SerializeField] Animator animator;
    [SerializeField] RangeEnemyController rangeEnemyController;
    int currentHealth;

    private void Start() {
        currentHealth = Health;
        rangeEnemyController = GetComponent<RangeEnemyController>();
    }
    public void TakeDmg(int dmg){
        currentHealth -= dmg;
        animator.SetTrigger("TakeDmg");
        if(currentHealth<=0){
            currentHealth = 0;
            Die();
        }
    }
    void Die(){
        Debug.Log("dierange");
        animator.SetBool("IsDead", true);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 1;
        this.enabled = false;
        rangeEnemyController.enabled = false;
    }
}
