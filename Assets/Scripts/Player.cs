using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;

    [SerializeField]
    float jumpForce = 7f;

    [SerializeField]
    float rayDistance = 5f;

    [SerializeField]
    Color rayColor;

    [SerializeField]
    LayerMask groundLayer;

    Rigidbody2D rb2D; 

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * Axis.x * moveSpeed * Time.deltaTime);
        if(JumpButton && IsGrounding)
        {
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    Vector2 Axis => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

    bool JumpButton => Input.GetButtonDown("Jump");

    bool IsGrounding => Physics2D.Raycast(transform.position, Vector2.down, rayDistance, groundLayer);

    void OnDrawGizmosSelected()
    {
        Gizmos.color = rayColor;
        Gizmos.DrawRay(transform.position, Vector2.down * rayDistance);

    }

}
