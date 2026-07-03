using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;
    public int damage = 10; // FIX: bullet butuh nilai damage buat dikirim ke boss

    private Rigidbody2D rb;
    private Vector2 direction;
    private PlayerController owner;

    public void SetOwner(PlayerController player)
    {
        owner = player;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        StartCoroutine(AutoDestroy());
    }

    void Update()
    {
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
        else
        {
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
        }
    }

    // dipanggil dari script shooter
    public void SetDirectionToCursor()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        direction = (mousePos - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(lifeTime);

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // FIX: sebelumnya method ini cuma "Destroy(gameObject)" tanpa cek apa-apa,
        // jadi peluru hancur duluan tanpa pernah ngasih damage ke Boss.

        if (collision.CompareTag("Boss"))
        {
            Boss boss = collision.GetComponent<Boss>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }
            else
            {
                Debug.LogWarning("[Bullet] Kena object bertag 'Boss' tapi komponen Boss tidak ditemukan!");
            }

            Destroy(gameObject);
            return;
        }

        if (collision.CompareTag("Wall") || collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
            return;
        }

        // Kalau kena trigger lain (misal area/pickup non-solid), peluru dibiarkan lewat
        // biar gak hancur sia-sia. Kalau maunya peluru hancur kena APAPUN, tinggal
        // ganti bagian ini jadi Destroy(gameObject) tanpa syarat.
    }
}