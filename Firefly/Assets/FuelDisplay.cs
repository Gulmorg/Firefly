using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelDisplay : MonoBehaviour
{
	[SerializeField] private PlayerController player;
	private void Update()
	{
		GetComponent<Slider>().value = player.Fuel / 100;
	}
}
