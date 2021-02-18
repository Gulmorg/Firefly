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
	[SerializeField] private Transform hiveLight;

	private float wiggleCounter;
	#endregion

	private void Update()
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
		while (Vector2.Distance(hiveLight.position, targetPos) < minimumTravel && ++iteration < 100);

		StartCoroutine(MoveToNewPosition(targetPos));
	}
	private IEnumerator MoveToNewPosition(Vector2 target)
	{
		while (wiggleCounter > 0)
		{
			hiveLight.Translate((target - (Vector2) hiveLight.localPosition).normalized * wiggleSpeed * Time.deltaTime);
			wiggleCounter -= Time.deltaTime;
			yield return null;
		}
		yield break;
	}
}
