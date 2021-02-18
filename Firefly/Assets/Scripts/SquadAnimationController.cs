using System.Collections;
using UnityEngine;

public class SquadAnimationController : MonoBehaviour
{
	#region Fields
	[Header("Settings")]
	[SerializeField] private float wiggleRange = 1f;
	[SerializeField] private float minimumTravel = 1f;
	[SerializeField] private float wiggleInterval = 0.5f;
	[SerializeField] private float wiggleSpeed = 1f;

	private Transform[] fireflies = new Transform[5];
	
	private float[] wiggleCounters = new float[5];
	#endregion

	private void Awake()
	{
		for (int i = 0; i < fireflies.Length; i++)
		{
			fireflies[i] = transform.GetChild(i);
		}
	}

	private void Update()
	{
		for (int i = 0; i < fireflies.Length; i++)
		{
			PartialUpdate(i);
		}
	}

	private void PartialUpdate(int index)
	{
		wiggleCounters[index] -= Time.deltaTime;
		if (wiggleCounters[index] > 0)
			return;

		wiggleCounters[index] = Random.Range(wiggleInterval - (wiggleInterval / 5), wiggleInterval + (wiggleInterval / 5));	// Get a random duration to be waited before next move command

		var targetPos = new Vector2();
		var iteration = 0;

		do
		{
			targetPos.x = Random.Range(-wiggleRange, wiggleRange);
			targetPos.y = Random.Range(-wiggleRange, wiggleRange);
		}
		while (Vector2.Distance(fireflies[index].position, targetPos) < minimumTravel && ++iteration < 100);
		
		StartCoroutine(MoveToNewPosition(index, targetPos));
	}

	private IEnumerator MoveToNewPosition(int i, Vector2 target)
	{
		while (wiggleCounters[i] > 0)
		{
			fireflies[i].Translate((target - (Vector2) fireflies[i].localPosition).normalized * wiggleSpeed *  Time.deltaTime);
			wiggleCounters[i] -= Time.deltaTime;
			yield return null;
		}
		yield break;
	}
}
