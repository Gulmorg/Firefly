using UnityEngine;

public class GameMenuController : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;

    public static bool IsPaused { get; private set; }

	private void Start()
	{
        IsPaused = false;
        pausePanel.SetActive(false);
	}

	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            Pause();
		}
    }

	public void Pause()
	{
        if (!IsPaused)
		{
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            IsPaused = true;
        }
		else
		{
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
            IsPaused = false;
		}
    }

    public void QuitGame()
	{
        Application.Quit();
	}
}
