using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 7f;
    [SerializeField] float rayDistance = 5f;
    [SerializeField] Color rayColor;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector3 rayOrigin;

    Rigidbody2D rb2D; 
    SpriteRenderer spr;
    Animator anim;

    GameInputs gameInputs;

    void Awake() 
    {
        rb2D = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        gameInputs = new GameInputs();
    }

    void OnEnable()
    {
        gameInputs.Enable();
    }
    
    void OnDisable()
    {
        gameInputs.Disable();
    }

    void Start()
    {
        gameInputs.Gameplay.Jump.performed += _=> Jump();
    }

    void FixedUpdate() 
    {
        rb2D.position += Vector2.right * Horizontal * moveSpeed * Time.fixedDeltaTime;
    }

    void Update()
    {
        spr.flipX = FlipSpriteX;

    }

    void LateUpdate() 
    {
        anim.SetFloat("AxisX", Mathf.Abs(Horizontal));
        anim.SetBool("Ground", IsGrounding);
    }

    void Jump()
    {
        if(IsGrounding)
        {
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger("Jump");
        }
        
    }

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
            GameManager.instance.GetScore.AddPoints(coin.GetPoints);
            Debug.Log(coin.GetPoints);
            Destroy(other.gameObject);
        }
    }

    float Horizontal => gameInputs.Gameplay.Horizontal.ReadValue<float>();
    bool IsGrounding => Physics2D.Raycast(transform.position + rayOrigin, Vector2.down, rayDistance, groundLayer);
    bool FlipSpriteX => Horizontal > 0f ? false : Horizontal < 0f ? true : spr.flipX;

}
