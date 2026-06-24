using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    [SerializeField] private PlayerData playerData;
    public float shootCooldown = 0.2f;
    private float nextShootTime = 0f;
    private float currentHP;
    private float speed;
    private PlayerInput playerInput;
    private Vector2 moveInput;
    private float attackInput;
    private float previousAttackInput;
    private Rigidbody2D rb;
    public int maxBullet = 3;
    private int currentBullet = 0;

    void Start()
    {
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
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentState != GameState.Playing)
            return;

        if (playerInput != null)
        {
            moveInput = playerInput.actions["Move"].ReadValue<Vector2>();

            attackInput = playerInput.actions["attack"].ReadValue<float>();

           if (previousAttackInput == 0 && attackInput > 0)
            {
                if (Time.time >= nextShootTime)
                {
                    Shoot();
                    nextShootTime = Time.time + shootCooldown;
                }
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

        Vector3 spawnPos = bulletSpawnPoint != null ? bulletSpawnPoint.position : transform.position;

        GameObject bulletObj = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

        Bullet bullet = bulletObj.GetComponent<Bullet>();
      if (bullet != null)
{
    bullet.SetDirectionToCursor();

    bullet.SetOwner(this); // TAMBAH INI
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