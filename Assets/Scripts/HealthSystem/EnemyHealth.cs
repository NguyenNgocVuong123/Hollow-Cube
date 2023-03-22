using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int Health;
    [SerializeField] Animator animator;
    EnemyController enemyController;
    int currentHealth;

    private void Start() {
        currentHealth = Health;
        enemyController = GetComponent<EnemyController>();
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
        Debug.Log("die");
        animator.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 1;
        this.enabled = false;
        enemyController.enabled = false;
    }
}
