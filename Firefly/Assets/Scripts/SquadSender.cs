using Pathfinding;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SquadSender : MonoBehaviour
{
	#region Fields
	[Header("References")]
	[SerializeField] GameObject squadPrefab;
	[SerializeField] GameObject targetPrefab;
	[SerializeField] Transform cursor;
	[Header("Renderer")]
	[SerializeField] SpriteRenderer spriteRenderer;
	[SerializeField] Light2D hiveLight;
	[SerializeField] Sprite[] hiveGraphics = new Sprite[25];

	private const int MAXIMUM_SQUADS = 24;
	private const int SQUAD_COST = 6;
	private int squadCount;
	private bool canCollect;
	private bool collectedThisFrame;
	private float lightIntensity;
	#endregion

	private void Start()
	{
		squadCount = MAXIMUM_SQUADS;
		lightIntensity = hiveLight.intensity;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && squadCount > 0)
		{
			var tempFuel = squadCount;
			squadCount -= SQUAD_COST;
			lightIntensity--;
			SendFirefly();
			StartCoroutine(SpawnGracePeriod());
			UpdateGraphics();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)	// FIX DOUBLE TRIGGERS!!!
	{
		if (canCollect && !collectedThisFrame && collision.CompareTag("Squad"))
		{
			Debug.Log($"Hit a squad, at time: {Time.time}, with a vleocity of: {GetComponent<Rigidbody2D>().velocity.magnitude}");
			CollectFirefly(collision);
			squadCount += SQUAD_COST;
			UpdateGraphics();
			lightIntensity++;
			if (squadCount == MAXIMUM_SQUADS)
				print("---- RESET ----");
			StartCoroutine(CollectCooldown());		
		}
	}

	private IEnumerator CollectCooldown()	// DOES NOT WORK, FIX THE PIECE OF SHIT!!!
	{
		collectedThisFrame = true;
		yield return new WaitForFixedUpdate();
		collectedThisFrame = false;
		yield return null;
	}
	private IEnumerator SpawnGracePeriod()
	{
		canCollect = false;
		yield return new WaitForSeconds(1);
		canCollect = true;
	}

	private void SendFirefly()
	{
		var target = Instantiate(targetPrefab, cursor.position, Quaternion.identity);
		var squad = Instantiate(squadPrefab, transform.position, Quaternion.identity);
		squad.GetComponent<AIDestinationSetter>().target = target.transform;
	}
	private void CollectFirefly(Collider2D collision)
	{
		Destroy(collision.GetComponent<AIDestinationSetter>().target.gameObject);
		Destroy(collision.gameObject);
	}

	private void UpdateGraphics()
	{
		spriteRenderer.sprite = hiveGraphics[MAXIMUM_SQUADS - squadCount];
		hiveLight.intensity = lightIntensity;
	}
}
