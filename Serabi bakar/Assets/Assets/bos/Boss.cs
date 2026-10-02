using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;
    public UnityEngine.UI.Slider healthBarSlider; // opsional

    [Header("Movement / Chase")]
    public float moveSpeed = 2f;
    public float chaseRange = 9000f;
    public float attackRange = 6f;
    private Transform player;
    private Rigidbody2D rb;

    [Header("Attack")]
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float attackCooldown = 2f;
    private float lastAttackTime = -999f;

    [Header("Drop")]
    public GameObject crystalPrefab;
    public Transform crystalSpawnPoint;

    [Header("Portal")]
    public BossPortal portalToOpen; // drag object portal (yang ada script BossPortal) ke sini

    private bool isDead = false;
    private Collider2D bossCollider;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        bossCollider = GetComponent<Collider2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        UpdateHealthBar();
    }

    void Update()
    {
        if (isDead || player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= attackRange)
        {
            StopMoving();
            TryAttack();
        }
        else if (dist <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            StopMoving();
        }
    }

    void ChasePlayer()
    {
        if (player == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            moveSpeed * Time.deltaTime
        );
    }

    void StopMoving()
    {
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;
        lastAttackTime = Time.time;
        ShootFireball();
    }

    void ShootFireball()
    {
        if (fireballPrefab == null || player == null) return;

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        Vector2 dirToPlayer = (player.position - spawnPos).normalized;

        GameObject fb = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
        Fireball fireballScript = fb.GetComponent<Fireball>();
        if (fireballScript != null)
        {
            fireballScript.SetDirection(dirToPlayer);

            // FIX: cegah fireball nabrak collider boss sendiri saat baru muncul
            // (spawnPos sering overlap dengan collider boss / firePoint yang nempel di boss)
            if (bossCollider != null)
                fireballScript.IgnoreCollisionWith(bossCollider);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthBar();

        if (currentHealth <= 0)
            Die();
    }

    void UpdateHealthBar()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        StopMoving();

        // drop 1 crystal per boss
        if (crystalPrefab != null)
        {
            Vector3 spawnPos = crystalSpawnPoint != null ? crystalSpawnPoint.position : transform.position;
            Instantiate(crystalPrefab, spawnPos + Vector3.up * 0.5f, Quaternion.identity);
        }

        // FIX: buka portal setelah boss dikalahkan
        if (portalToOpen != null)
        {
            portalToOpen.Open();
        }
        else
        {
            Debug.LogWarning("[Boss] portalToOpen belum di-assign di Inspector!");
        }

        StartCoroutine(DieSequence());
    }

    IEnumerator DieSequence()
    {
        // Animator anim = GetComponent<Animator>();
        // if (anim != null) anim.SetTrigger("Die");

        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}