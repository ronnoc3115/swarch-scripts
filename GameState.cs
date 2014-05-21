//Connor Richards	54689185
//Jonathan Stevens	61356189

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class GameState : MonoBehaviour {
	public GameObject player;
	public GameObject pellet;

	private NetHandler netHandler;
	private Thread queueThread;
	private UI ui;
	private int numPlayers;
	//private int pelletNum;
	private int playerNum;
	private float pellet1X;
	private float pellet1Y;
	private float pellet2X;
	private float pellet2Y;
	private float pellet3X;
	private float pellet3Y;
	private float pellet4X;
	private float pellet4Y;

	private int playerID;
	private string playerName;
	private float playerX;
	private float playerY;
	private float playerSize;

	private int userID;
	private string userName;
//	private Pellet1 pellet1;
//	private Pellet2 pellet2;
//	private Pellet3 pellet3;
//	private Pellet4 pellet4;
//	private Player1 player1;
//	private Player2 player2;

	private bool settingPlayerID;
	private bool settingPlayerName;
	private bool settingPelletSpawn;
	private bool settingPlayerSpawn;
	private bool settingPlayerSize;

	private bool settingUserInfo;

	private bool gameStart;
	//private bool showLogin;
	//private bool loginSuccessful;

	// Use this for initialization
	void Start () {
	
		netHandler = gameObject.GetComponent<NetHandler>();
		ui = gameObject.GetComponent<UI>();
		numPlayers = 0;
		//pelletNum = 0;
		playerNum = 0;
		pellet1X = 0.0f;
		pellet1Y = 0.0f;
		pellet2X = 0.0f;
		pellet2Y = 0.0f;
		pellet3X = 0.0f;
		pellet3Y = 0.0f;
		pellet4X = 0.0f;
		pellet4Y = 0.0f;

		//info for player being looked at
		playerID = 0;
		playerName = "";
		playerX = 0.0f;
		playerY = 0.0f;
		playerSize = 0.0f;

		//info for the user of this specific client
		userID = 0;
		userName = "";

		queueThread = new Thread(new ThreadStart(readFromQueue));
		queueThread.Start();

		settingPlayerID = false;
		settingPlayerName = false;
		settingPelletSpawn = false;
		settingPlayerSpawn = false;
		settingPlayerSize = false;

		settingUserInfo = false;

		gameStart = false;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(gameStart)
		{
			for(int i = 0; i < numPlayers; i++)
			{
				GameObject newPlayer = (GameObject)Instantiate(player);
				newPlayer.name = ("Player" + (i+1));
				//newPlayer.GetComponent<Player>().setID(i+1);
			}
			for(int i = 0; i < 4; i++)
			{
				GameObject newPellet = (GameObject)Instantiate(pellet);
				newPellet.name = ("Pellet" + (i+1));
			}
			ui.showGUI = false;
			gameStart = false;
		}

		if(settingPelletSpawn)
		{
			setPelletSpawn();
		}

		if(settingPlayerID)
		{
			setPlayerID();
		}

		if(settingPlayerName)
		{
			setPlayerName();
		}

		if(settingPlayerSpawn)
		{
			setPlayerSpawn();
		}

		if(settingPlayerSize)
		{
			setPlayerSize();
		}

		if(settingUserInfo)
		{
			setUserInfo();
		}
	}

	private void setPelletSpawn()
	{
		GameObject.Find("Pellet1").GetComponent<Pellet>().spawn(pellet1X, pellet1Y);
		GameObject.Find("Pellet2").GetComponent<Pellet>().spawn(pellet2X, pellet2Y);
		GameObject.Find("Pellet3").GetComponent<Pellet>().spawn(pellet3X, pellet3Y);
		GameObject.Find("Pellet4").GetComponent<Pellet>().spawn(pellet4X, pellet4Y);
		settingPelletSpawn = false;
	}

	private void setPlayerID()
	{
		//set ID on player's Player.cs
		GameObject.Find("Player" + playerNum).GetComponent<Player>().setID(playerID);
		settingPlayerID = false;
	}

	private void setPlayerName()
	{
		//set name on UI
		GameObject.Find("Game Logic").GetComponent<UI>().setNametoChange(playerName);
		GameObject.Find("Game Logic").GetComponent<UI>().setIDNametoChange(playerID);
		settingPlayerName = false;
	}

	private void setPlayerSpawn()
	{
		GameObject.Find("Player" + playerNum).GetComponent<Player>().respawn(playerX, playerY);
		settingPlayerSpawn = false;
	}

	private void setPlayerSize()
	{
		GameObject.Find("Player" + playerNum).GetComponent<Player>().changeSize(playerSize);
		settingPlayerSize = false;
	}

	private void setUserInfo()
	{
		//set UI based on user ID and name
		GameObject.Find("Game Logic").GetComponent<UI>().setPlayerID(userID);
		GameObject.Find("Game Logic").GetComponent<UI>().setPlayerName(userName);
		settingUserInfo = false;
	}

	private void readFromQueue()
	{
		while(true)
		{
			if(netHandler.readQueue.Count > 0)
			{
				string[] command = netHandler.readQueue.Dequeue();

				switch(command[0])
				{
				case "login":
					//timestamp
					ui.setLoginSuccessful(command[1].Equals("True"));
					ui.setPasswordCorrect(command[1].Equals("True"));
					ui.setPlayerID (int.Parse(command[2]));
					//highscore (ui.setHighScore(int.Parse(command[x])));
					ui.setNewUser(command[3].Equals("True"));
					if (command[1].Equals("True"))
					{
						numPlayers = (int.Parse(command[12]));
						gameStart = true;

						pellet1X = (float.Parse(command[4]));
						pellet1Y = (float.Parse(command[5]));
						pellet2X = (float.Parse(command[6]));
						pellet2Y = (float.Parse(command[7]));
						pellet3X = (float.Parse(command[8]));
						pellet3Y = (float.Parse(command[9]));
						pellet4X = (float.Parse(command[10]));
						pellet4Y = (float.Parse(command[11]));
						settingPelletSpawn = true;

						for(int i = 0; i < numPlayers; i++)
						{
							playerNum = i+1;
							playerID = (int.Parse(command[13+i]));
							playerName = ((command[14+i]).ToString());
							if(int.Parse(command[13+i])==int.Parse(command[2]))
							{
								userID = (int.Parse(command[13+i]));
								userName = ((command[14+i]).ToString());
								settingUserInfo = true;
							}
							playerX = (float.Parse(command[15+i]));
							playerY = (float.Parse(command[16+i]));
							playerSize = (float.Parse(command[17+i]));
							settingPlayerID = true;
							settingPlayerName = true;
							settingPlayerSpawn = true;
							settingPlayerSize = true;
						}
					}
					break;
				case "gamestate":
					//timestamp
					pellet1X = (float.Parse(command[1]));
					pellet1Y = (float.Parse(command[2]));
					pellet2X = (float.Parse(command[3]));
					pellet2Y = (float.Parse(command[4]));
					pellet3X = (float.Parse(command[5]));
					pellet3Y = (float.Parse(command[6]));
					pellet4X = (float.Parse(command[7]));
					pellet4Y = (float.Parse(command[8]));

					numPlayers = (int.Parse(command[9]));

					for(int i = 0; i < numPlayers; i++)
					{
						playerNum = i+1;
						playerID = (int.Parse(command[10+i]));
						//playerName = ((command[11+i]).ToString());
						playerX = (float.Parse(command[12+i]));
						playerY = (float.Parse(command[13+i]));
						playerSize = (float.Parse(command[14+i]));
						settingPlayerSpawn = true;
						settingPlayerSize = true;
					}
					break;
				case "updateScore":
					//timestamp
					//player ID (belongs to whose score is being updated)
					//new score
					break;
				default:
					break;
				}
			}
			Thread.Sleep(100);
		}
	}
}
