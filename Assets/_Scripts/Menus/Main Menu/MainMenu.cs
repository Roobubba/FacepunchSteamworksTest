using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
	private TestMultiplayerMenu mpMenu;

	private void Start()
	{
		mpMenu = GetComponentInParent<TestMultiplayerMenu>();	
	}

	/// <summary>
	/// Button press in Unity to host a new game
	/// </summary>
	public void HostButton()
	{
		mpMenu.HostGame();
	}

	/// <summary>
	/// Button press in Unity to load JoinMenu scene
	/// to search for and join someone else's game
	/// </summary>
	public void JoinButton()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
	}

	/// <summary>
	/// Button press in Unity to quit the application
	/// </summary>
	public void QuitButton()
	{
		Application.Quit();
	}

}
