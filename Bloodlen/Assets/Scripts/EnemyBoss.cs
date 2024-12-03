using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
	public float life = 20;
	public NPC npc1;
	public NPC npc2;
	private bool isPlat;
	private bool isObstacle;
	private Transform fallCheck;
	//private Transform wallCheck;
	public LayerMask turnLayerMask;
	private Rigidbody2D rb;

	public EnemyWall checkCollision;

	private SpriteRenderer sr = null;

	private bool facingRight = true;

	public float speed = 5f;

	public bool isInvincible = false;
	private bool isHitted = false;

	void Awake()
	{
		fallCheck = transform.Find("FallCheck");
		//wallCheck = transform.Find("WallCheck");
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{

		if (life <= 0)
		{
			StartCoroutine(DestroyEnemy());
		}

		isPlat = Physics2D.OverlapCircle(fallCheck.position, .2f, 1 << LayerMask.NameToLayer("Default"));
		//isObstacle = Physics2D.OverlapCircle(wallCheck.position, .2f, turnLayerMask);

		if (sr.isVisible)
		{
			if (checkCollision.isOn)
			{
				Flip();
			}

			if (!isHitted && life > 0 && Mathf.Abs(rb.velocity.y) < 0.5f)
			{
				if (isPlat) //&& !isObstacle && !isHitted)
				{
					if (facingRight)
					{
						rb.velocity = new Vector2(-speed, rb.velocity.y);
					}
					else
					{
						rb.velocity = new Vector2(speed, rb.velocity.y);
					}
				}
				else
				{
					Flip();
				}
			}
		}
	}

	void Flip()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void ApplyDamage(float damage)
	{
		if (!isInvincible)
		{
			float direction = damage / Mathf.Abs(damage);
			damage = Mathf.Abs(damage);
			transform.GetComponent<Animator>().SetBool("Hit", true);
			life -= damage;
			rb.velocity = Vector2.zero;
			rb.AddForce(new Vector2(direction * 500f, 100f));
			StartCoroutine(HitTime());
		}
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player" && life > 0)
		{
			collision.gameObject.GetComponent<CharacterController2D>().ApplyDamage(3f, transform.position);
			transform.GetComponent<Animator>().SetBool("Ataque", true);
		}

	}

	IEnumerator HitTime()
	{
		isHitted = true;
		isInvincible = true;
		yield return new WaitForSeconds(0.1f);
		isHitted = false;
		isInvincible = false;
	}

	IEnumerator DestroyEnemy()
	{
		npc1.dialogue[0] = "Obrigado! Você salvou a nossa vila!";
		npc2.dialogue[0] = "Obrigado!";
		CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
		yield return new WaitForSeconds(0.25f);
		rb.velocity = new Vector2(0, rb.velocity.y);
		yield return new WaitForSeconds(1f);
		Destroy(gameObject);
	}
}
