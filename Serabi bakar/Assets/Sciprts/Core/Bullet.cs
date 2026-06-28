using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;

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

        owner?.OnBulletDestroyed();
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        owner?.OnBulletDestroyed();
        Destroy(gameObject);
    }
}