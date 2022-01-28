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

    [SerializeField]
    Vector3 rayOrigin;

    [SerializeField]
    Score score;

    Rigidbody2D rb2D; 
    SpriteRenderer spr;
    Animator anim;

    void Awake() 
    {
        rb2D = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void FixedUpdate() 
    {
        rb2D.position += Vector2.right * Axis.x * moveSpeed * Time.fixedDeltaTime;
    }

    void Update()
    {
        //transform.Translate(Vector2.right * Axis.x * moveSpeed * Time.deltaTime);
        spr.flipX = FlipSpriteX;

        if(JumpButton && IsGrounding)
        {
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger("Jump");
        }

    }

    void LateUpdate() 
    {
        anim.SetFloat("AxisX", Mathf.Abs(Axis.x));
        anim.SetBool("Ground", IsGrounding);
    }

    Vector2 Axis => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    bool JumpButton => Input.GetButtonDown("Jump");
    bool IsGrounding => Physics2D.Raycast(transform.position + rayOrigin, Vector2.down, rayDistance, groundLayer);
    bool FlipSpriteX => Axis.x > 0f ? false : Axis.x < 0f ? true : spr.flipX;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = rayColor;
        Gizmos.DrawRay(transform.position + rayOrigin, Vector2.down * rayDistance);

    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("coin"))
        {
            Coin coin = other.GetComponent<Coin>();
            score.AddPoints(coin.GetPoints);
            Debug.Log(coin.GetPoints);
            Destroy(other.gameObject);
        }
    }


}
