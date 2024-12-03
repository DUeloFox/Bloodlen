using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyHoming : MonoBehaviour
{
	public GameObject player;
    Transform playerTr;
    [SerializeField] float speed = 5;
	public float life = 5;

	private SpriteRenderer sr = null;
	private Rigidbody2D rb;

	private bool isHitted = false;
	public bool isInvincible = false;

	// Start is called before the first frame update
	void Awake()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
    }

	// Update is called once per frame
	void FixedUpdate()
	{
		Vector3 scale = transform.localScale;

		if (player.transform.position.x > transform.position.x)
        {
			scale.x = Mathf.Abs(scale.x);

        }
		else
        {
			scale.x = Mathf.Abs(scale.x) * -1;
        }

		transform.localScale = scale;

		if (Vector2.Distance(transform.position, playerTr.position) < 0.1f)
            return;

		if (life <= 0)
		{
			transform.GetComponent<Animator>().SetBool("IsDead", true);
            StartCoroutine(DestroyEnemy());
        }

        if (sr.isVisible)
		{

			if (!isHitted && life > 0)
			{
				transform.position = Vector2.MoveTowards(
		        transform.position,
		        new Vector2(playerTr.position.x, playerTr.position.y + 0.5f),
		        speed * Time.deltaTime);
			}
		}
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
			StartCoroutine(HitTime());
		}
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player" && life > 0)
		{
			collision.gameObject.GetComponent<CharacterController2D>().ApplyDamage(2f, transform.position);
			transform.GetComponent<Animator>().SetBool("Ataque", true);
		}

	}

	IEnumerator HitTime()
	{
		isHitted = true;
		isInvincible = true;
		yield return new WaitForSeconds(1f);
		isHitted = false;
		isInvincible = false;
	}

	IEnumerator DestroyEnemy()
	{
		CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
		capsule.size = new Vector2(1f, 0.25f);
		capsule.offset = new Vector2(0f, -0.8f);
		capsule.direction = CapsuleDirection2D.Horizontal;
		yield return new WaitForSeconds(0.25f);
		rb.velocity = new Vector2(0, rb.velocity.y);
		yield return new WaitForSeconds(1f);
		Destroy(gameObject);
	}
}
