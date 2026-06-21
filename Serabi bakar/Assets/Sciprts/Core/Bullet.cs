using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D rb;
    private Vector3 direction;

    void OnEnable()
    {
        StartCoroutine(DeactivateRoutine());
    }

    IEnumerator DeactivateRoutine()
{
    yield return new WaitForSeconds(5f);
    // Kembalikan peluru ke kolam dengan menonaktifkannya
    gameObject.SetActive(false); 
}


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log($"Bullet spawned with speed: {speed}, direction: {direction}");
        // Destroy bullet after 5 seconds if it hasn't been destroyed already
        // Destroy(gameObject, 5f);
    }

    void Update()
    {
        if (rb != null)
        {
            // Move using Rigidbody2D
            rb.linearVelocity = direction * speed;
        }
        else
        {
            // Fallback to manual movement if no Rigidbody2D
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
        Debug.Log($"Bullet SetDirection called with: {newDirection}, normalized: {direction}");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Destroy bullet on collision
        Debug.Log("Bullet hit: " + collision.gameObject.name);
        // Destroy(gameObject);
        gameObject.SetActive(false);
    }
}