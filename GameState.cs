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
	private float playerX;
	private float playerY;
	private float playerSize;
//	private Pellet1 pellet1;
//	private Pellet2 pellet2;
//	private Pellet3 pellet3;
//	private Pellet4 pellet4;
//	private Player1 player1;
//	private Player2 player2;

	private bool settingPelletSpawn;
	private bool settingPlayerSpawn;
	private bool settingPlayerSize;
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
		playerX = 0.0f;
		playerY = 0.0f;
		playerSize = 0.0f;
//		pellet1 = GameObject.Find("Pellet1");
//		pellet2 = GameObject.Find("Pellet2");
//		pellet3 = GameObject.Find("Pellet3");
//		pellet4 = GameObject.Find("Pellet4");
//		player1 = GameObject.Find("Player1");
//		player2 = GameObject.Find("Player2");
		queueThread = new Thread(new ThreadStart(readFromQueue));
		queueThread.Start();

		settingPelletSpawn = false;
		settingPlayerSpawn = false;
		settingPlayerSize = false;
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
				newPlayer.GetComponent<Player>().setID(i+1);
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

		if(settingPlayerSpawn)
		{
			setPlayerSpawn();
		}

		if(settingPlayerSize)
		{
			setPlayerSize();
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
							playerX = (float.Parse(command[13+i]));
							playerY = (float.Parse(command[14+i]));
							playerSize = 6.0f;
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
						playerX = (float.Parse(command[10+i]));
						playerY = (float.Parse(command[11+i]));
						playerSize = (float.Parse(command[12+i]));
						settingPlayerSpawn = true;
						settingPlayerSize = true;
					}
					break;
				case "turn":
					//timestamp
					//playerID (check with user)
					//proceed in appropriate direction
					//position coordinate
					break;
				case "eat":
					//timestamp
					//playerID of eater(check with user)
					//level (determines growth and slow amount)
					//position of eaten
					//pellet/ID# (determines if a pellet, or which ID was eaten)
					//new spawn point of eaten (if a player then reset size, speed, and score to base)
					//score increase by int variable (for ID of eater)
					break;
				case "walldie":
					//timestamp
					//playerID (check with user)
					//new spawn point of dead (reset size, speed, and score to base)
					break;
				default:
					break;
				}
			}
			Thread.Sleep(100);
		}
	}
}
