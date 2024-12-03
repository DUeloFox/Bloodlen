using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[SerializeField] private PhysicsMaterial2D noFrictionMaterial;
	[SerializeField] private PhysicsMaterial2D FrictionMaterial;
	public CharacterController2D controller;
	public Animator animator;
	private Rigidbody2D m_Rigidbody2D;
	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
	bool dash = false;
	private bool canMove;

    //bool dashAxis = false;

    // Update is called once per frame
    private void Awake()
    {
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}
    void Update () {

		if(horizontalMove > 0)
        {
			m_Rigidbody2D.sharedMaterial = noFrictionMaterial;
        }
		else if(horizontalMove < 0)
        {
			m_Rigidbody2D.sharedMaterial = noFrictionMaterial;
		}
		else
        {
			m_Rigidbody2D.sharedMaterial = FrictionMaterial;
		}

			horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
			animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		

		if (Input.GetKeyDown(KeyCode.Space))
		{
			jump = true;
		}

        if (Input.GetKeyDown(KeyCode.Q))
        {
            dash = true;
        }

        /*if (Input.GetAxisRaw("Dash") == 1 || Input.GetAxisRaw("Dash") == -1) //RT in Unity 2017 = -1, RT in Unity 2019 = 1
		{
			if (dashAxis == false)
			{
				dashAxis = true;
				dash = true;
			}
		}
		else
		{
			dashAxis = false;
		}
		*/

    }

	public void OnFall()
	{
		animator.SetBool("IsJumping", true);
	}

	public void OnLanding()
	{
		animator.SetBool("IsJumping", false);
	}

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
		jump = false;
		dash = false;
	}
}
