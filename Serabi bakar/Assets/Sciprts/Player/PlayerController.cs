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
    private float attackInput;
    private float previousAttackInput;
    private Rigidbody2D rb;
    public int maxBullet = 3;
    private int currentBullet = 0;
    private bool canShoot = false;

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

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            canShoot = true;
            Debug.Log("Shooting mode ON");
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput.normalized * speed;
    }

    void Shoot()
    {
        if (isAttacking) return;

        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        animator.SetTrigger("Attack");

        speed = originalSpeed * 0.2f; // jalan 20% dari speed normal

        // Tunggu sampai animasi selesai
        yield return new WaitForSeconds(
            animator.GetCurrentAnimatorStateInfo(0).length
        );

        speed = originalSpeed;

        SpawnBullet();

        isAttacking = false;
    }

    void SpawnBullet()
    {
        Debug.Log("Player is shooting!");

        if (bulletPrefab == null)
        {
            Debug.LogWarning("Bullet prefab not assigned!");
            return;
        }

        // BATAS 3 PELURU
        if (currentBullet >= maxBullet)
        {
            Debug.Log("Max bullet reached!");
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
            currentBullet++;
        }
    }

    public void OnBulletDestroyed()
    {
        currentBullet--;
        if (currentBullet < 0)
            currentBullet = 0;
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