using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadTargetController : MonoBehaviour
{
	private Transform player;

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
    }
}
