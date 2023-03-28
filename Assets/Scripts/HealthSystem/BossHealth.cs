using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] BossController BossController;
    [SerializeField] int _health = 30;
    public static BossHealth instance;
    public int currentHealth;

    void MakeInstance(){
        if(instance == null){
            instance = this;
        }
    }
    private void Start() {
        currentHealth = _health;
        BossController = GetComponent<BossController>();
    }
    public void TakeDmg(int dmg){
        currentHealth -= dmg;
        animator.SetTrigger("TakeDmg");
        if(currentHealth<=0){
            currentHealth = 0;
            Die();
            Invoke("Ending",2f);
        }
    }
    void Ending(){
        FindObjectOfType<GameManager>().Win();
    }
    void Die(){
        animator.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 1;
        this.enabled = false;
        BossController.enabled = false;
    }
}
