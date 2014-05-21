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
	//to call ui methods easier
	private UI ui;
	//number of players in a game
	private int numPlayers;
	//currently player being looked at, used in for loops that iterate through all player info
	private int playerNum;
	//pellet positions
	//had to make individual variables to avoid thread colissions
	private float pellet1X;
	private float pellet1Y;
	private float pellet2X;
	private float pellet2Y;
	private float pellet3X;
	private float pellet3Y;
	private float pellet4X;
	private float pellet4Y;
	//info on player being looked at
	private int playerID;
	private string playerName;
	private float playerX;
	private float playerY;
	private float playerSize;
	//info on client's user info
	//used to give UI info to display, and change colors appropriately
	private int userID;
	private string userName;
	//bools to start set methods in update
	//not able to call GameObject.Find() inside side thread, so these are necessary
	private bool settingPlayerID;
	private bool settingPlayerName;
	private bool settingPelletSpawn;
	private bool settingPlayerSpawn;
	private bool settingPlayerSize;

	private bool settingUserInfo;
	//instantiate pellets and players
	private bool gameStart;

	// Use this for initialization
	void Start () {
	
		netHandler = gameObject.GetComponent<NetHandler>();
		ui = gameObject.GetComponent<UI>();
		numPlayers = 0;
		playerNum = 0;
		pellet1X = 0.0f;
		pellet1Y = 0.0f;
		pellet2X = 0.0f;
		pellet2Y = 0.0f;
		pellet3X = 0.0f;
		pellet3Y = 0.0f;
		pellet4X = 0.0f;
		pellet4Y = 0.0f;

		playerID = 0;
		playerName = "";
		playerX = 0.0f;
		playerY = 0.0f;
		playerSize = 0.0f;

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
			//create as many players as the server says are connected
			for(int i = 0; i < numPlayers; i++)
			{
				GameObject newPlayer = (GameObject)Instantiate(player);
				//each player gets their ID on their name
				newPlayer.name = ("Player" + (i+1));
				//newPlayer.GetComponent<Player>().setID(i+1);
			}
			//create 4 pellets
			for(int i = 0; i < 4; i++)
			{
				GameObject newPellet = (GameObject)Instantiate(pellet);
				//give a unique "ID" name to each pellet for easier tracking
				newPellet.name = ("Pellet" + (i+1));
			}
			//hide GUI when login succeeds
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
		//set positions for each pellet
		GameObject.Find("Pellet1").GetComponent<Pellet>().spawn(pellet1X, pellet1Y);
		GameObject.Find("Pellet2").GetComponent<Pellet>().spawn(pellet2X, pellet2Y);
		GameObject.Find("Pellet3").GetComponent<Pellet>().spawn(pellet3X, pellet3Y);
		GameObject.Find("Pellet4").GetComponent<Pellet>().spawn(pellet4X, pellet4Y);
		settingPelletSpawn = false;
	}

	private void setPlayerID()
	{
		//set ID on player's Player.cs
		GameObject.Find("Player" + playerID).GetComponent<Player>().setID(playerID);
		settingPlayerID = false;
	}

	private void setPlayerName()
	{
		//set name on UI
		GameObject.Find("Game Logic").GetComponent<UI>().setNametoChange(playerName);
		GameObject.Find("Game Logic").GetComponent<UI>().setIDNametoChange(playerID);
		settingPlayerName = false;
	}

	//bad name. only sets position
	private void setPlayerSpawn()
	{
		GameObject.Find("Player" + playerID).GetComponent<Player>().respawn(playerX, playerY);
		settingPlayerSpawn = false;
	}

	private void setPlayerSize()
	{
		GameObject.Find("Player" + playerID).GetComponent<Player>().changeSize(playerSize);
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
			//read from queue when there are things to be read
			if(netHandler.readQueue.Count > 0)
			{
				string[] command = netHandler.readQueue.Dequeue();

				//command statements based on first command
				switch(command[0])
				{
					//only sent on initial login
				case "login":
					//timestamp needed here
					//set if login is successful in ui
					ui.setLoginSuccessful(command[1].Equals("True"));
					//set if password is correct in ui
					ui.setPasswordCorrect(command[1].Equals("True"));
					//give this client's user their ID
					ui.setPlayerID (int.Parse(command[2]));
					//highscore (ui.setHighScore(int.Parse(command[x]))); NEEDED HERE
					//if this user's account is newly created
					ui.setNewUser(command[3].Equals("True"));
					//only go through this if the login succeeded
					if (command[1].Equals("True"))
					{
						//# of players server said are connected
						numPlayers = (int.Parse(command[12]));
						gameStart = true;
						//set pellet positions
						pellet1X = (float.Parse(command[4]));
						pellet1Y = (float.Parse(command[5]));
						pellet2X = (float.Parse(command[6]));
						pellet2Y = (float.Parse(command[7]));
						pellet3X = (float.Parse(command[8]));
						pellet3Y = (float.Parse(command[9]));
						pellet4X = (float.Parse(command[10]));
						pellet4Y = (float.Parse(command[11]));
						settingPelletSpawn = true;
						//set player info based on how many players server said are connected
						for(int i = 0; i < numPlayers; i++)
						{
							playerID = (int.Parse(command[13+i]));
							playerName = ((command[14+i]).ToString());
							//if the ID of player being looked at matches the ID of this client's user, then this is the user's name and ID
							if(int.Parse(command[13+i])==int.Parse(command[2]))
							{
								//send this info to UI for personalization of client aesthetics
								userID = (int.Parse(command[13+i]));
								userName = ((command[14+i]).ToString());
								settingUserInfo = true;
							}
							//set info of player being looked at
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
					//sent constantly by server to keep client on track
				case "gamestate":
					//timestamp needed here
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
						playerID = (int.Parse(command[10+i]));
						playerName = ((command[11+i]).ToString());
						playerX = (float.Parse(command[12+i]));
						playerY = (float.Parse(command[13+i]));
						playerSize = (float.Parse(command[14+i]));
						settingPlayerSpawn = true;
						settingPlayerSize = true;
					}
					break;
				case "updateScore":
					//timestamp needed here
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
