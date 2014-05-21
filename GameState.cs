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
	//player positions
	//had to make individual variables to avoid thread colissions
	private float player1X;
	private float player1Y;
	private int player1ID;
	private string player1Name;
	private float player1Size;
	private float player2X;
	private float player2Y;
	private int player2ID;
	private string player2Name;
	private float player2Size;
	private float player3X;
	private float player3Y;
	private int player3ID;
	private string player3Name;
	private float player3Size;
	private float player4X;
	private float player4Y;
	private int player4ID;
	private string player4Name;
	private float player4Size;
	
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

	private GameObject[] players;

	// Use this for initialization
	void Start () {
		
		netHandler = gameObject.GetComponent<NetHandler>();
		ui = gameObject.GetComponent<UI>();
		numPlayers = 0;
		pellet1X = 0.0f;
		pellet1Y = 0.0f;
		pellet2X = 0.0f;
		pellet2Y = 0.0f;
		pellet3X = 0.0f;
		pellet3Y = 0.0f;
		pellet4X = 0.0f;
		pellet4Y = 0.0f;
		
		player1X = 0.0f;
		player1Y = 0.0f;
		player1ID = 0;
		player1Name = "";
		player1Size = 1;
		player2X = 0.0f;
		player2Y = 0.0f;
		player2ID = 0;
		player2Name = "";
		player2Size = 1;
		player3X = 0.0f;
		player3Y = 0.0f;
		player3ID = 0;
		player3Name = "";
		player3Size = 1;
		player4X = 0.0f;
		player4Y = 0.0f;
		player4ID = 0;
		player4Name = "";
		player4Size = 1;
		
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

		players = new GameObject[4];
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
				players[i] = newPlayer;
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
		//GameObject.Find("Player" + playerID).GetComponent<Player>().setID(playerID);
		settingPlayerID = false;
	}
	
	private void setPlayerName()
	{
		//set name on UI
		if(GameObject.Find("Player1")!=null)
		{
			GameObject.Find("Game Logic").GetComponent<UI>().setNametoChange(player1Name);
			GameObject.Find("Game Logic").GetComponent<UI>().setIDNametoChange(player1ID);
		}
		if(GameObject.Find("Player2")!=null)
		{
			GameObject.Find("Game Logic").GetComponent<UI>().setNametoChange(player2Name);
			GameObject.Find("Game Logic").GetComponent<UI>().setIDNametoChange(player2ID);
		}
		if(GameObject.Find("Player3")!=null)
		{
			GameObject.Find("Game Logic").GetComponent<UI>().setNametoChange(player3Name);
			GameObject.Find("Game Logic").GetComponent<UI>().setIDNametoChange(player3ID);
		}
		if(GameObject.Find("Player4")!=null)
		{
			GameObject.Find("Game Logic").GetComponent<UI>().setNametoChange(player4Name);
			GameObject.Find("Game Logic").GetComponent<UI>().setIDNametoChange(player4ID);
		}
		settingPlayerName = false;
	}
	
	//bad name. only sets position
	private void setPlayerSpawn()
	{

		//if(players[0]!=null)
			players[0].GetComponent<Player>().respawn(player1X, player1Y);
			//GameObject.Find("Player1").GetComponent<Player>().respawn(player1X, player1Y);
		//if(players[1]!=null)
			players[1].GetComponent<Player>().respawn(player2X, player2Y);
			//GameObject.Find("Player2").GetComponent<Player>().respawn(player2X, player2Y);
		//if(players[2]!=null)
			players[2].GetComponent<Player>().respawn(player3X, player3Y);
			//GameObject.Find("Player3").GetComponent<Player>().respawn(player3X, player3Y);
		//if(players[3]!=null)
			players[3].GetComponent<Player>().respawn(player4X, player4Y);
			//GameObject.Find("Player4").GetComponent<Player>().respawn(player4X, player4Y);
		//settingPlayerSpawn = false;
	}
	
	private void setPlayerSize()
	{
		if(GameObject.Find("Player1")!=null)
			GameObject.Find("Player1").GetComponent<Player>().changeSize(player1Size);
		if(GameObject.Find("Player2")!=null)
			GameObject.Find("Player2").GetComponent<Player>().changeSize(player2Size);
		if(GameObject.Find("Player3")!=null)
			GameObject.Find("Player3").GetComponent<Player>().changeSize(player3Size);
		if(GameObject.Find("Player4")!=null)
			GameObject.Find("Player4").GetComponent<Player>().changeSize(player4Size);
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
			Debug.Log("eh?");
			//read from queue when there are things to be read
			if(netHandler.readQueue.Count > 0)
			{
				string[] command = netHandler.readQueue.Dequeue();
				Debug.Log(command[0].Equals("turn") + " " + command[0] + " " + "turn");
				
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
						players = new GameObject[numPlayers];
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
						if(numPlayers >= 1)
						{
							player1ID = (int.Parse(command[13]));
							player1Name = ((command[14]).ToString());
							//if the ID of player being looked at matches the ID of this client's user, then this is the user's name and ID
							if(int.Parse(command[13])==int.Parse(command[2]))
							{
								//send this info to UI for personalization of client aesthetics
								userID = (int.Parse(command[13]));
								userName = ((command[14]).ToString());
								settingUserInfo = true;
							}
							//set info of player being looked at
							player1X = (float.Parse(command[15]));
							player1Y = (float.Parse(command[16]));
							player1Size = (float.Parse(command[17]));
						}
						if(numPlayers >= 2)
						{
							player2ID = (int.Parse(command[18]));
							player2Name = ((command[19]).ToString());

							if(int.Parse(command[18])==int.Parse(command[2]))
							{

								userID = (int.Parse(command[18]));
								userName = ((command[19]).ToString());
								settingUserInfo = true;
							}

							player2X = (float.Parse(command[20]));
							player2Y = (float.Parse(command[21]));
							player2Size = (float.Parse(command[22]));
						}
						if(numPlayers >= 3)
						{
							player3ID = (int.Parse(command[23]));
							player3Name = ((command[24]).ToString());

							if(int.Parse(command[23])==int.Parse(command[2]))
							{

								userID = (int.Parse(command[23]));
								userName = ((command[24]).ToString());
								settingUserInfo = true;
							}

							player3X = (float.Parse(command[25]));
							player3Y = (float.Parse(command[26]));
							player3Size = (float.Parse(command[27]));
						}
						if(numPlayers >= 4)
						{
							player4ID = (int.Parse(command[28]));
							player4Name = ((command[29]).ToString());

							if(int.Parse(command[28])==int.Parse(command[2]))
							{

								userID = (int.Parse(command[28]));
								userName = ((command[29]).ToString());
								settingUserInfo = true;
							}

							player4X = (float.Parse(command[30]));
							player4Y = (float.Parse(command[31]));
							player4Size = (float.Parse(command[32]));
						}
						settingPlayerID = true;
						settingPlayerName = true;
						//settingPlayerSpawn = true;
						setPlayerSpawn();
						settingPlayerSize = true;
					}
					break;
					//sent constantly by server to keep client on track
					//NOT ANYMORE CUZ THIS SUCKS
				case "gamestate":
					
					Debug.Log("HI THERE");
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
					
					if(numPlayers >= 1)
					{
						player1ID = (int.Parse(command[10]));
						player1Name = ((command[11]).ToString());
						player1X = (float.Parse(command[12]));
						player1Y = (float.Parse(command[13]));
						player1Size = (float.Parse(command[14]));
					}
					if(numPlayers >= 2)
					{
						player2ID = (int.Parse(command[15]));
						player2Name = ((command[16]).ToString());
						player2X = (float.Parse(command[17]));
						player2Y = (float.Parse(command[18]));
						player2Size = (float.Parse(command[19]));
					}
					if(numPlayers >= 3)
					{
						player3ID = (int.Parse(command[20]));
						player3Name = ((command[21]).ToString());
						player3X = (float.Parse(command[22]));
						player3Y = (float.Parse(command[23]));
						player3Size = (float.Parse(command[24]));
					}
					if(numPlayers >= 4)
					{
						player4ID = (int.Parse(command[25]));
						player4Name = ((command[26]).ToString());
						player4X = (float.Parse(command[27]));
						player4Y = (float.Parse(command[28]));
						player4Size = (float.Parse(command[29]));
					}
					//settingPlayerSpawn = true;
					setPlayerSpawn();
					settingPlayerSize = true;
					break;
					//sent when a player sends out an input
				case "turn":
					//timestamp
					numPlayers = (int.Parse(command[1]));
					Debug.Log("YOoooooooooo");
					for(int i = 0; i < numPlayers; i++)
					{
						switch(command[2 + i])
						{
						case "UP":
							players[i].GetComponent<Player>().up();
							break;
						case "DOWN":
							players[i].GetComponent<Player>().down();
							break;
						case "LEFT":
							players[i].GetComponent<Player>().left();
							break;
						case "RIGHT":
							players[i].GetComponent<Player>().right();
							break;
						}
					}
					break;
				case "updateScore":
					//timestamp needed here
					//player ID (belongs to whose score is being updated)
					//new score
					break;
				}
			}
			Thread.Sleep(100);
			Debug.Log("meh.");
		}
	}
}
