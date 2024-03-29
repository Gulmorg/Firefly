﻿using System.Runtime.CompilerServices;
using UnityEngine;

[SelectionBase, RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	#region Fields
	[Header("Movement")]
	[SerializeField, Tooltip("The method in which the acceleration values will be applied. \nChanges during play mode will not take effect.")] private AccelerationMode accelerationMode = AccelerationMode.Instant;
	[SerializeField, Tooltip("Top speed of the player on horizontal axis. \nChanging the value drastically during play mode may cause issues with deceleration.")] private float topSpeed = 1f;
	[SerializeField, Tooltip("Amount of acceleration applied per second. \nUseless for Instant acceleration mode.")] private float acceleration = 1f;
	[Header("Jumping")]
	[SerializeField, Tooltip("The way in which upwards input will be read.")] private JumpMode jumpMode = JumpMode.Jump;
	[SerializeField, Tooltip("Game gravity multiplier. Set to 1 for standard gravity.")] private float gravity = 10f;
	[SerializeField, Tooltip("Amount of force applied to the player per jump mode.")] private float upwardsForce = 1f;
	[SerializeField, Tooltip("Amount of fuel players have in their resevoir.")] private float maximumFuel = 100f;
	[SerializeField, Tooltip("Amount of \"fuel\" drained per second.")] private float fuelDrain = 50f;
	[SerializeField, Tooltip("Amount of \"fuel\" replenished per second.")] private float fuelReplenish = 50f;
	[SerializeField, Tooltip("Amount of performable jumps that the player can use before needing to land. \nSet to 1 for disabling mid-air jumps. \nUseless for flight mode.")] private int maximumJumps = 1;

	[Header("References")]
	[SerializeField] new private Rigidbody2D rigidbody;
	[SerializeField] private UIManager uiManager;	// GET RID OF THIS GARBAGE DEPENDENCY (after the jam)

	public float Fuel => fuel;

	private float fuel;
	private float horizontalInput;
	private bool jumpIntent;
	private bool flightIntent;
	private int availableJumps;
	private float lastFrameVelocity;
	private float minimumSpeed;
	private ForceMode2D forceMode;
	#endregion

	private void Awake()
	{
		Physics2D.gravity = new Vector2(0f, -9.81f * gravity);
		switch (accelerationMode)
		{
			case AccelerationMode.Instant:
				forceMode = ForceMode2D.Impulse;
				acceleration = topSpeed;
				minimumSpeed = topSpeed / 2;
				break;
			default:
				forceMode = ForceMode2D.Force;
				minimumSpeed = topSpeed / 10;
				break;
		}

		switch (jumpMode)
		{
			case JumpMode.Jump:
				availableJumps = maximumJumps;
				break;
			case JumpMode.Flight:
				fuel = maximumFuel;
				upwardsForce *= 2;
				break;
		}

	}

	private void Update()
	{
		horizontalInput = Input.GetAxisRaw("Horizontal");

		switch (jumpMode)
		{
			case JumpMode.Jump:
				if (availableJumps > 0 && Input.GetKeyDown(KeyCode.Space))
				{
					jumpIntent = true;
				}
				break;
			case JumpMode.Flight:
				if (fuel > 10 && Input.GetKeyDown(KeyCode.Space))
				{
					flightIntent = true;
				}
				if (Input.GetKeyUp(KeyCode.Space) || fuel <= 0)
				{
					flightIntent = false;
				}
				break;
		}
	}

	private void FixedUpdate()
	{
		Move();
		
		if (flightIntent)
		{
			fuel -= fuelDrain * Time.deltaTime;
			Fly();
		}
		else if (fuel < 100)
		{
			fuel += fuelReplenish * Time.deltaTime;
		}
		if (jumpIntent)
		{
			jumpIntent = false;
			Jump();
		}
	}

	private void Move()
	{
		if (horizontalInput != 0 && (Mathf.Abs(rigidbody.velocity.x) < topSpeed || rigidbody.velocity.x * horizontalInput < 0)) /// If there is input and player is slower than top speed or input changed direction then accelerate.
		{
			rigidbody.AddForce(new Vector2(horizontalInput * acceleration, 0f), forceMode);
		}
		
		if (Mathf.Abs(rigidbody.velocity.x) > topSpeed)																			 /// If player is faster than top speed then set its speed to top speed.
		{
			var topSpeed = rigidbody.velocity.x > 0 ? this.topSpeed : -this.topSpeed;
			rigidbody.velocity = new Vector2(topSpeed, rigidbody.velocity.y);
		}

		if (horizontalInput == 0 && rigidbody.velocity.x != 0)																	 /// If there is no input and player is moving and
		{																														 
			if (Mathf.Abs(rigidbody.velocity.x) > minimumSpeed && lastFrameVelocity * rigidbody.velocity.x >= 0)				 /// If speed is higher than minimum sensitivity and player didn't change directions 
			{																													 /// (for cases where physics engine applies excess force) then apply counter force.
				var counterForce = rigidbody.velocity.x > 0 ? -1 : 1;															 
				rigidbody.AddForce(new Vector2(counterForce * acceleration, 0f), forceMode);									 
			}																													 
			else																												 /// If speed is lower than minimum sensitivity or player changed directions while slowing down then set its horizontal speed to zero.
			{
				rigidbody.velocity = new Vector2(0f, rigidbody.velocity.y);
			}
		}
		lastFrameVelocity = rigidbody.velocity.x;
	}

	private void Jump()
	{
		rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0f);
		rigidbody.AddForce(Vector2.up * upwardsForce, ForceMode2D.Impulse);
		availableJumps--;
	}
	private void Fly()
	{
		rigidbody.AddForce(Vector2.up * upwardsForce, ForceMode2D.Force);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Ground"))
		{
			availableJumps = maximumJumps;
		}
		
		if (collision.CompareTag("FinishTag"))
		{
			uiManager.instance.ActivateEndgameWindow();
		}
		else if (collision.CompareTag("Shuttle"))
		{
			uiManager.instance.ActivateLore(0);
		}
		else if (collision.CompareTag("Helmet"))
		{
			uiManager.instance.ActivateLore(1);
		}
		else if (collision.CompareTag("Potato"))
		{
			uiManager.instance.ActivateLore(2);
		}
		else if (collision.CompareTag("Parchment"))
		{
			uiManager.instance.ActivateLore(3);
		}
		else if (collision.CompareTag("Stone"))
		{
			uiManager.instance.ActivateLore(4);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Shuttle"))
		{
			uiManager.instance.DeactivateLore();
		}
		else if (collision.CompareTag("Helmet"))
		{
			uiManager.instance.DeactivateLore();
		}
		else if (collision.CompareTag("Potato"))
		{
			uiManager.instance.DeactivateLore();
		}
		else if (collision.CompareTag("Parchment"))
		{
			uiManager.instance.DeactivateLore();
		}
		else if (collision.CompareTag("Stone"))
		{
			uiManager.instance.DeactivateLore();
		}
	}
}
