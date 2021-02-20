using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadTargetController : MonoBehaviour
{
	private Transform player;
	public Transform followingSquad;
	private float safetyCounter;
	private bool teleported;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("PlayerMain"))
		{
			player = collision.transform;
		}
	}

	void Update()
    {
		if (player != null)
		{
			transform.position = player.position;
		}

		if (Vector2.Distance(transform.position, followingSquad.position) > 1 && !teleported)
		{
			safetyCounter += Time.deltaTime;

			if (safetyCounter > 5)
			{
				teleported = true;
				transform.position = followingSquad.transform.position;
			}
		}
    }
}
