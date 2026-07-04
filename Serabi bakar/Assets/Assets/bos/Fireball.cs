using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 10;
    public float lifeTime = 5f; // auto destroy kalau gak kena apa-apa

    private Vector2 direction;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // PENTING: pastikan prefab Fireball punya Rigidbody2D dengan Body Type = Kinematic
        // Kalau gak ada Rigidbody2D sama sekali, OnTriggerEnter2D bisa gak konsisten kepanggil,
        // dan kalau ada tapi Dynamic, physics bakal "menimpa" transform.position manual kita.
        if (rb == null)
        {
            Debug.LogWarning("[Fireball] Rigidbody2D tidak ditemukan di prefab! Tambahkan Rigidbody2D (Kinematic) + Collider2D (Is Trigger = true).");
        }
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;

        // opsional: rotate sprite biar menghadap arah gerak
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        // Gerakkan lewat Rigidbody2D biar gak "ketimpa" physics dan trigger tetap akurat.
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
        else
        {
            // fallback kalau memang gak pakai Rigidbody2D sama sekali
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
        }
    }

    // Dipanggil dari Boss.cs setelah Instantiate, biar fireball gak nabrak collider boss sendiri
    // saat baru muncul (ini bisa bikin fireball "nyangkut" di titik spawn).
    public void IgnoreCollisionWith(Collider2D other)
    {
        Collider2D myCollider = GetComponent<Collider2D>();
        if (myCollider != null && other != null)
        {
            Physics2D.IgnoreCollision(myCollider, other);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // 1 hati per kena bola api
            }
            else
            {
                Debug.LogWarning("[Fireball] Kena Player tapi komponen PlayerHealth tidak ditemukan di object dengan tag Player!");
            }

            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall") || other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}