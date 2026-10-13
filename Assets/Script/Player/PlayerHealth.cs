using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Regeneration")]
    public bool canRegenerate = true;
    public float regenDelay = 5f; // Thời gian đợi trước khi hồi máu
    public int regenAmount = 5; // Máu hồi mỗi lần
    public float regenRate = 1f; // Thời gian giữa mỗi lần hồi

    private float lastDamageTime;
    private float nextRegenTime;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Tự động hồi máu nếu không bị damage
        if (canRegenerate && currentHealth < maxHealth && !isDead)
        {
            if (Time.time - lastDamageTime >= regenDelay && Time.time >= nextRegenTime)
            {
                RegenerateHealth();
                nextRegenTime = Time.time + regenRate;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        lastDamageTime = Time.time;

        Debug.Log("Player took " + damage + " damage. Health: " + currentHealth);

        // Update UI
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
            uiManager.UpdateHealthBar(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void RegenerateHealth()
    {
        currentHealth += regenAmount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        // Update UI
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
            uiManager.UpdateHealthBar(currentHealth, maxHealth);
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || isDead)
            return;

        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
            uiManager.UpdateHealthBar(currentHealth, maxHealth);
    }

    public bool UseHealthPack()
    {
        if (isDead || currentHealth >= maxHealth)
            return false;

        if (!GameManager.TryConsumeHealthPack())
            return false;

        Heal(GameManager.HealthPackHealAmount);
        return true;
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Player Died!");

        // Thông báo cho GameManager
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
            gameManager.PlayerDied();
    }

    public bool IsDead()
    {
        return isDead;
    }
}
