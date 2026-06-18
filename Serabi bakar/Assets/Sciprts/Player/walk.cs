using UnityEngine;

public class walk : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    private Vector2 input;
    private Vector2 lastinput;
    public GameObject UIpanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIpanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        prosesinput();   
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = input * speed;
    }

    void prosesinput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if ((moveX == 0 && moveY == 0) && (input.x != 0 || input.y != 0))
        {
            lastinput = input;
        }

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        input.Normalize();
    }
}
