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
    bool isOutOfZone = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerScript = GetComponent<Player>();
    }


    private void FixedUpdate()
    {
        if (isOutOfZone)
        {
            TakeDamage(1);
        }
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
        GameManager.instance.cantPlayers--;
        animator.SetTrigger("Death");
        playerScript.DisablePlayer();
        isOutOfZone = false;
    }

    public void SetOutOfZone(bool _isOutOfZone)
    {
        isOutOfZone = _isOutOfZone;
    }

    void OnChangeHealth(int health)
    {
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
    }
}
