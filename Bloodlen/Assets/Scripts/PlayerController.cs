using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float gravity; 
    public float jumpSpeed;
    public float jumpHeight;
    public float jumpLimitTime;
    public GroundCheck ground; 
    public GroundCheck head;
    public AnimationCurve dashCurve; 
    public AnimationCurve jumpCurve;

    private Animator anim = null;
    private Rigidbody2D rb = null;
    private bool isGround = false;
    private bool isJump = false;
    private bool isHead = false;
    private float jumpPos = 0.0f;
    private float dashTime, jumpTime; 
    private float beforeKey;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isGround = ground.IsGround();
        isHead = head.IsGround();
        MoveUpdate();
    }

    private void MoveUpdate()
    {
        float horizontalKey = Input.GetAxis("Horizontal");
        float xSpeed = 0.0f;
        float ySpeed = -gravity;
        float verticalKey = Input.GetAxis("Vertical");
        if (isGround)
        {
            if (verticalKey > 0)
            {
                ySpeed = jumpSpeed;
                jumpPos = transform.position.y;
                isJump = true;
                jumpTime = 0.0f;
            }
            else
            {
                isJump = false;
            }
        }
        else if (isJump)
        {
            bool pushUpKey = verticalKey > 0;
            bool canHeight = jumpPos + jumpHeight > transform.position.y;
            bool canTime = jumpLimitTime > jumpTime;

            if (pushUpKey && canHeight && canTime && !isHead)
            {
                ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime;
            }
            else
            {
                isJump = false;
                jumpTime = 0.0f;
            }
        }
        if (horizontalKey > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            anim.SetBool("walk", true);
            dashTime += Time.deltaTime;
            xSpeed = speed;
        }
        else if (horizontalKey < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            anim.SetBool("walk", true);
            dashTime += Time.deltaTime;
            xSpeed = -speed;
        }
        else
        {
            anim.SetBool("walk", false);
            xSpeed = 0.0f;
            dashTime = 0.0f;
        }
        if (horizontalKey > 0 && beforeKey < 0)
        {
            dashTime = 0.0f;
        }
        else if (horizontalKey < 0 && beforeKey > 0)
        {
            dashTime = 0.0f;
        }
        beforeKey = horizontalKey;
        xSpeed *= dashCurve.Evaluate(dashTime);
        if (isJump)
        {
            ySpeed *= jumpCurve.Evaluate(jumpTime);
        }
        anim.SetBool("jump", isJump); 
        anim.SetBool("fall", isGround); 
        rb.velocity = new Vector2(xSpeed, ySpeed);
    }
}
