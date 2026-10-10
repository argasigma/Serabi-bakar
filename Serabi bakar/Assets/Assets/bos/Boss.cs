using UnityEngine;
using System.Collections;
using System;

public class Boss : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;
    public UnityEngine.UI.Slider healthBarSlider; // opsional

    [Header("Movement / Chase")]
    public float moveSpeed;
    public float chaseRange;
    public float attackRange;
    [SerializeField] private float radiusPatrol;

    private Vector2 tujuanPatrol;
    private Vector2 titikAwal;
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
    private StateBoss state = StateBoss.IDLE;

    public static event Action<Boss> OnZombieMati;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bossCollider = GetComponent<Collider2D>();
        currentHealth = maxHealth;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        UpdateHealthBar();
    }

    void Update()
    {
        PeriksaTransisi();
        
        switch(state)
        {
            case StateBoss.IDLE: PerilakuIdle(); break;
            case StateBoss.PATROL: PerilakuPatrol(); break;
            case StateBoss.CHASE: PerilakuChase(); break;
            case StateBoss.ATTACK: PerilakuAttack(); break;
        }
    }

    public float JarakKePlayer()
    {
        if (player == null) return Mathf.Infinity   ;
        return Vector2.Distance(transform.position, player.position);
    }

    void PeriksaTransisi()
    {
        float jarak = JarakKePlayer();

        if (jarak <= attackRange)
        {
            state = StateBoss.ATTACK;
            state = StateBoss.CHASE;
        }
        else if (jarak <= chaseRange)
        {
            state = StateBoss.CHASE;
        }
        else
        {
            state = StateBoss.IDLE;
        }  
    }

    void PerilakuIdle()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }

    void PerilakuChase()
    {
        if (player == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            moveSpeed * Time.deltaTime
        );
    }

    void PerilakuPatrol()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            tujuanPatrol,
            moveSpeed * 0.5f * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, tujuanPatrol) < 0.1f)
        {
            PilihTujuanPatrolBaru();
        }
    }

    void PilihTujuanPatrolBaru()
    {
        // DIUBAH: memilih Random milik Unity agar tidak ambigu dengan System.Random.
        Vector2 acak = UnityEngine.Random.insideUnitCircle * radiusPatrol;
        tujuanPatrol = titikAwal + acak;
    }

    void PerilakuAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
        {
            lastAttackTime = Time.time;
            ShootFireball();
        }
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