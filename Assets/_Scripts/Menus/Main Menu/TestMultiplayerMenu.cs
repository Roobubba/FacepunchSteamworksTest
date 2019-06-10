using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestMultiplayerMenu : FacepunchMultiplayerMenu
{
	private bool isHost = false;
	private Canvas canvas;

	private void Awake()
	{
		DontDestroyOnLoad(this);
		canvas = GetComponent<Canvas>();
		SceneManager.sceneLoaded += SceneLoaded;
	}

	public bool GetIsHost()
	{
		return isHost;
	}

	public void HostGame()
	{
		isHost = true;
		Host();
	}

	public void SceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.buildIndex == 1)
		{
			canvas.enabled = true;
		}
		else
		{
			canvas.enabled = false;
		}
	}

	private void OnApplicationQuit()
	{
		SceneManager.sceneLoaded -= SceneLoaded;
	}
}
