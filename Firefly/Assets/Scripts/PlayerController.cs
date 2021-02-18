using System.Runtime.CompilerServices;
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
	[SerializeField, Tooltip("Amount of force applied to the player per jump mode.")] private float flightForce = 1f;
	[SerializeField, Tooltip("Amount of performable jumps that the player can use before needing to land. \nSet to 1 for disabling mid-air jumps. \nUseless for Flight Mode.")] private int maximumJumps = 1;

	[Header("References")]
	[SerializeField] new private Rigidbody2D rigidbody;
	
	private float horizontalInput;
	private bool jumpIntent;
	private int availableJumps;
	private float lastFrameVelocity;
	private float minimumSpeed;
	private ForceMode2D forceMode;
	#endregion

	private void Start()
	{
		Physics2D.gravity = new Vector2(0f, -9.81f * gravity);
		if (accelerationMode == AccelerationMode.Instant)
		{
			forceMode = ForceMode2D.Impulse;
			acceleration = topSpeed;
			minimumSpeed = topSpeed / 2;
		}
		else
		{
			forceMode = ForceMode2D.Force;
			minimumSpeed = topSpeed / 10;
		}

		availableJumps = maximumJumps;
	}

	private void Update()
	{
		horizontalInput = Input.GetAxisRaw("Horizontal");

		if (availableJumps > 0 && Input.GetKey(KeyCode.Space))
		{
			jumpIntent = true;
		}
	}

	private void FixedUpdate()
	{
		Move();

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
		if (jumpMode == JumpMode.Jump)
		{
			rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0f);
			rigidbody.AddForce(Vector2.up * flightForce, ForceMode2D.Impulse);
			availableJumps--;		
		}
		else
		{
			rigidbody.AddForce(Vector2.up * flightForce, ForceMode2D.Force);
			//IMPLEMENT FUEL SYSTEM!!!!
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)	/// Ground check.
	{
		if (collision.CompareTag("Ground"))
		{
			availableJumps = maximumJumps;
		}
	}
}
