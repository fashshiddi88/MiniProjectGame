using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    
    private enum State {idle, running, hurt}
    private State state = State.idle;


    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int diamonds = 0;
    [SerializeField] private TextMeshProUGUI diamondText;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private int health;
    [SerializeField] private TextMeshProUGUI healthAmount;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        healthAmount.text = health.ToString();
    }

    // Update is called once per frame
    private void Update()
    {
        if(state != State.hurt)
        {
           Move();
        }

        AnimationState();
        anim.SetInteger("state", (int)state);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collactable")
        {
            Destroy(collision.gameObject);
            diamonds += 1;
            diamondText.text = diamonds.ToString();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            state = State.hurt;
            health -= 1;
            healthAmount.text = health.ToString();
            HandleHealth();
            if (other.gameObject.transform.position.x > transform.position.x)
            {
                rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(hurtForce, rb.velocity.y);
            }
        }
    }

    private void HandleHealth()
    {
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Move()
    {
        float hDirection = Input.GetAxis("Horizontal");
        if (hDirection < 0)
        {
            //move left
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        else if (hDirection > 0)
        {
            //move right
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        //jump
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void AnimationState()
    {
        if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            //Moving
            state = State.running;
        }
        else
        {
            //Switch to idle
            state = State.idle;
        }

    }
}
