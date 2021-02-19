using UnityEngine;

public class GameManager : MonoBehaviour
{
	private GameManager instance;
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this);
		}
	}
	private void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Confined;
	}
}
