using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelDisplay : MonoBehaviour
{
	[SerializeField] private PlayerController player;

	private void Update()	// For anyone reading, this is a placeholder script and GetComponent should be cached or better, be referenced via inspector. Calling it each frame is highly inefficient.
	{
		GetComponent<Slider>().value = player.Fuel / 100;
	}
}
