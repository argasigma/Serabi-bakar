using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    [SerializeField] private PlayerData playerData;

    private float currentHP;
    private float speed;
    private float originalSpeed;
    private bool isAttacking;

    private PlayerInput playerInput;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    private float attackInput;
    private float previousAttackInput;

    // Reload systems
    [SerializeField] private int maxBullet = 4;
    [SerializeField] private float reloadTime = 5f;
    private int bulletFired = 0;
    private bool isReloading = false;
    
    private float attackDuration = 1f;

    void Start()
    {
        // Get components
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();

        // Biar player gak jatuh dan gak muter
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        if (playerData == null)
        {
            Debug.LogError("PlayerData belum di-assign di Inspector!");
            return;
        }

        currentHP = playerData.maxHP;
        speed = playerData.moveSpeed;
        originalSpeed = speed;
    }

    void Update()
    {

        if (GameManager.Instance != null && GameManager.Instance.currentState != GameState.Playing)
            return;

        if (playerInput != null)
        {
            moveInput = playerInput.actions["Move"].ReadValue<Vector2>();

            animator.SetFloat("Speed", moveInput.magnitude);

            if (moveInput != Vector2.zero)
            {
                animator.SetFloat("MoveX", moveInput.x);
                animator.SetFloat("MoveY", moveInput.y);

                animator.SetFloat("LastX", moveInput.x);
                animator.SetFloat("LastY", moveInput.y);
            }

            if (moveInput.x < 0)
                spriteRenderer.flipX = true;
            else if (moveInput.x > 0)
                spriteRenderer.flipX = false;

            attackInput = playerInput.actions["Attack"].ReadValue<float>();

            bool aiming = Keyboard.current.digit1Key.isPressed;

            if (aiming && previousAttackInput == 0 && attackInput > 0)
            {
                Shoot();
            }

            previousAttackInput = attackInput;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput.normalized * speed;
    }

    void Shoot()
    {
        if (isAttacking || isReloading) return;

        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        speed = originalSpeed * 0.1f; // 10% dari speed normal

        animator.SetTrigger("Attack");

        // Tunggu sampai animasi selesai
        yield return new WaitForSeconds(attackDuration);

        speed = originalSpeed;

        SpawnBullet();

        isAttacking = false;
    }

    IEnumerator ReloadRoutine()
    {
        isReloading = true;

        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        bulletFired = 0;
        isReloading = false;

        Debug.Log("Reload Done!");
    }

    void SpawnBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Bullet prefab not assigned!");
            return;
        }

        Vector3 spawnPos = bulletSpawnPoint != null
            ? bulletSpawnPoint.position
            : transform.position;

        GameObject bulletObj = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.SetDirectionToCursor();
            bullet.SetOwner(this);
        }

        bulletFired++;

        if (bulletFired >= maxBullet)
        {
            StartCoroutine(ReloadRoutine());
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            TakeDamage(0.1f);
        }
    }

    void TakeDamage(float dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0 && GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
    }
}