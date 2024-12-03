using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
	private new Rigidbody2D rigidbody2D;
	private SpriteRenderer spriteRenderer;

	[HideInInspector] public float xSpeed;
	[HideInInspector] public bool rightFacing;
	void Start()
    {
		rigidbody2D = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();

		rightFacing = true;
	}

    void Update()
    {
		MoveUpdate();
		JumpUpdate();
	}

	private void MoveUpdate()
	{
		if (Input.GetKey(KeyCode.RightArrow))
		{
			xSpeed = 6.0f;
			rightFacing = true;
			spriteRenderer.flipX = false;
		}
		else if (Input.GetKey(KeyCode.LeftArrow))
		{
			xSpeed = -6.0f;
			rightFacing = false;
			spriteRenderer.flipX = true;
		}
		else
		{
			xSpeed = 0.0f;
		}
	}

	private void JumpUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			float jumpPower = 10.0f;
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpPower);
		}
	}

	private void FixedUpdate()
	{
		Vector2 velocity = rigidbody2D.velocity;
		velocity.x = xSpeed;

		rigidbody2D.velocity = velocity;
	}
}
