using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{
    public const int maxHealth = 100;
    [SerializeField] RectTransform healthBar;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    Animator animator;
    Player playerScript;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerScript = GetComponent<Player>();
    }

    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            RpcDeath();
        }
    }

    public void Heal(int amount)
    {
        if (!isServer)
            return;
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    [ClientRpc]
    void RpcDeath()
    {
            animator.SetTrigger("Death");
            playerScript.DisablePlayer();
    }

    void OnChangeHealth(int health)
    {
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
    }
}
