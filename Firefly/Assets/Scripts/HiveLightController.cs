using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class HiveLightController : MonoBehaviour
{
	#region Fields
	[Header("Settings")]
	[SerializeField] private float wiggleRange = 1f;
	[SerializeField] private float minimumTravel = 1f;
	[SerializeField] private float wiggleInterval = 0.5f;
	[SerializeField] private float wiggleSpeed = 1f;

	[Header("References")]
	[SerializeField] private Transform hiveLightTransform;
	[SerializeField] private Light2D hiveLight;
	[SerializeField] private PlayerController playerController;

	private float wiggleCounter;
	private float multiplier;
	#endregion


	private void Start()
	{
		var maxRadius = hiveLight.pointLightOuterRadius;
		multiplier = playerController.Fuel / maxRadius;
	}

	private void Update()
	{
		hiveLight.pointLightOuterRadius = playerController.Fuel / multiplier; // f == 100  r == 7.5 ~ f == 50 r == 3.75

		WiggleAround();
	}

	private void WiggleAround()
	{
		wiggleCounter -= Time.deltaTime;
		if (wiggleCounter > 0)
			return;

		wiggleCounter = Random.Range(wiggleInterval - (wiggleInterval / 5), wiggleInterval + (wiggleInterval / 5)); // Get a random duration to be waited before next move command

		var targetPos = new Vector2();
		var iteration = 0;

		do
		{
			targetPos.x = Random.Range(-wiggleRange, wiggleRange);
			targetPos.y = Random.Range(-wiggleRange, wiggleRange);
		}
		while (Vector2.Distance(hiveLightTransform.position, targetPos) < minimumTravel && ++iteration < 100);

		StartCoroutine(MoveToNewPosition(targetPos));
	}

	private IEnumerator MoveToNewPosition(Vector2 target)
	{
		while (wiggleCounter > 0)
		{
			hiveLightTransform.Translate((target - (Vector2) hiveLightTransform.localPosition).normalized * wiggleSpeed * Time.deltaTime);
			wiggleCounter -= Time.deltaTime;
			yield return null;
		}
		yield break;
	}
}
