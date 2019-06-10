#if FACEPUNCH_STEAMWORKS
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Threading.Tasks;
using Steamworks;

public class PlayerList : MonoBehaviour
{
	public ScrollRect players;
	public PlayerListEntry playerListEntryTemplate;
	public RectTransform playerListContentRect;
	public Button quitButton;
	public Text selectedLobbyText;
	public Text instructionsText;

	private const string LOBBY_TEXT = "Steam Lobby ID: ";
	private const string CLIENT_INSTRUCTIONS = "You are a Client.\r\nAll clients should be shown in the list below when they join the steam lobby.";

	private List<PlayerListItemData> playerList = new List<PlayerListItemData>();
	private float playerListEntryTemplateHeight;
	private float nextListUpdateTime = 0f;
	private TestMultiplayerMenu mpMenu;
	private bool isHost = false;

	private void Start()
	{
		// Init the MainThreadManager
		MainThreadManager.Create();

		mpMenu = FindObjectOfType<TestMultiplayerMenu>();
		isHost = mpMenu.GetIsHost();
		playerListEntryTemplateHeight = ((RectTransform)playerListEntryTemplate.transform).rect.height;
		RefreshPlayerList();
		if (!isHost)
		{
			instructionsText.text = CLIENT_INSTRUCTIONS;
		}
		selectedLobbyText.text = LOBBY_TEXT + mpMenu.lobbyToJoin.Id.ToString();
	}

	private void Update()
	{
		if (Time.time > nextListUpdateTime)
		{
			RefreshPlayerList();
			nextListUpdateTime = Time.time + 5.0f + UnityEngine.Random.Range(0.0f, 1.0f);
		}
	}

	/// <summary>
	/// Add a player to the list of players in this lobby
	/// </summary>
	/// <param name="steamId">The SteamId of the player to add to the list</param>
	private void AddPlayer(SteamId steamId)
	{
		for (int i = 0; i < playerList.Count; ++i)
		{
			var player = playerList[i];
			if (player.steamId.Value == steamId.Value)
			{
				// Already have that player listed nothing else to do
				UpdateItem(player);
				return;
			}
		}

		var playerListItemData = new PlayerListItemData {
			listItem = GameObject.Instantiate<PlayerListEntry>(playerListEntryTemplate, players.content),
			steamId = steamId,
		};
		playerListItemData.listItem.gameObject.SetActive(true);

		UpdateItem(playerListItemData);
		playerListItemData.nextUpdate = Time.time + 5.0f + UnityEngine.Random.Range(0.0f, 1.0f);

		playerList.Add(playerListItemData);

		RepositionItems();
	}

	/// <summary>
	/// Remove a player from the list
	/// </summary>
	/// <param name="item">Lobby listItemData to remove</param>
	private void RemovePlayer(PlayerListItemData item)
	{
		Destroy(item.listItem.gameObject);
		playerList.Remove(item);
		RepositionItems();
	}

	/// <summary>
	/// Reposition the server list items after a add/remove operation
	/// </summary>
	private void RepositionItems()
	{
		for (int i = 0; i < playerList.Count; i++)
		{
			PositionItem(playerList[i].listItem.gameObject, i);
		}

		var sizeDelta = playerListContentRect.sizeDelta;
		sizeDelta.y = playerList.Count * playerListEntryTemplateHeight;
		playerListContentRect.sizeDelta = sizeDelta;
	}

	/// <summary>
	/// Set the position of an item in the server list
	/// </summary>
	/// <param name="item"></param>
	/// <param name="index"></param>
	private void PositionItem(GameObject item, int index)
	{
		var rectTransform = (RectTransform)item.transform;
		rectTransform.localPosition = new Vector3(0.0f, -playerListEntryTemplateHeight * index, 0.0f);
	}


	/// <summary>
	/// Update a specific server's details on the server list.
	/// </summary>
	/// <param name="option">The server display information to update</param>
	private void UpdateItem(PlayerListItemData option, Friend friend = default(Friend))
	{
		option.steamId = friend.Id;
		option.listItem.steamId.text = option.steamId.Value.ToString();

		option.playerName = friend.Name;
		option.listItem.playerName.text = option.playerName;
		//ping
		//isClientConnected
	}

	/// <summary>
	/// Refresh the player list
	/// </summary>
	private void RefreshPlayerList()
	{
		var lobby = mpMenu.lobbyToJoin;
		foreach (var friend in lobby.Members)
		{
			bool haveThisFriend = false;
			for (int i = 0; i < playerList.Count; i++)
			{
				if (playerList[i].steamId.Value == friend.Id.Value)
				{
					haveThisFriend = true;
					UpdateItem(playerList[i], friend);
					continue;
				}
			}
			if (haveThisFriend)
				continue;
			AddPlayer(friend.Id);
		}
	}
}

internal class PlayerListItemData
{
	public string playerName;
	public PlayerListEntry listItem;
	public float nextUpdate;
	public SteamId steamId;
	public bool isClientConnected;
	public int ping;
}
#endif
