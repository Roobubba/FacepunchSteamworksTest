#if FACEPUNCH_STEAMWORKS
using UnityEngine;
using Steamworks;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton class to initialize Facepunch.Steamworks client and shotdown on application quit
/// </summary>
public class FacepunchSteamworksController : MonoBehaviour
{
	public static FacepunchSteamworksController facepunchSteamworksController;

	private const int STEAM_APP_ID = 480;

	private void Awake()
	{
		if (facepunchSteamworksController == null)
		{
			facepunchSteamworksController = this;
			DontDestroyOnLoad(gameObject);

			try
			{
				SteamClient.Init(STEAM_APP_ID);
			}
			catch (System.Exception e)
			{
				Debug.Log("Could not initialize SteamClient. Exception:");
				Debug.LogException(e);
			}
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	private void OnApplicationQuit()
	{
		if (facepunchSteamworksController == this)
			SteamClient.Shutdown();
	}
}
#endif
