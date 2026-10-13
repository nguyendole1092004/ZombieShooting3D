using UnityEngine;
using UnityEngine.AI;

public class ZombieHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Rewards")]
    public int moneyReward = 50; // Tiền thưởng khi giết zombie

    [Header("Effects")]
    public GameObject deathEffect;
    public AudioClip hitSound;
    public AudioClip deathSound;

    private Animator animator;
    private ZombieAI zombieAI;
    private NavMeshAgent navMeshAgent;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();
        zombieAI = GetComponent<ZombieAI>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        if (damage <= 0)
        {
            Debug.LogError("Zombie damage must be greater than zero.", this);
            return;
        }

        currentHealth -= damage;

        Debug.Log(gameObject.name + " took " + damage + " damage. Health: " + currentHealth, this);
        if (hitSound != null)
            AudioSource.PlayClipAtPoint(hitSound, transform.position);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Play hit animation nếu có
            if (animator != null)
            {
                try
                {
                    animator.SetTrigger("Hit");
                }
                catch
                {
                    // Animator không có trigger Hit
                }
            }
        }
    }

    void Die()
    {
        isDead = true;

        if (zombieAI != null)
            zombieAI.enabled = false;
        if (navMeshAgent != null && navMeshAgent.isActiveAndEnabled)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
        }

        // Play death animation
        if (animator != null)
        {
            try
            {
                animator.SetTrigger("Die");
            }
            catch
            {
                // Animator không có trigger Die
            }
        }

        // Play death sound
        if (deathSound != null)
            AudioSource.PlayClipAtPoint(deathSound, transform.position);

        // Spawn death effect
        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }

        // Thưởng tiền cho player
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.AddMoney(moneyReward);
            gameManager.AddKill();
        }

        // Destroy zombie sau 3 giây
        Destroy(gameObject, 3f);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
