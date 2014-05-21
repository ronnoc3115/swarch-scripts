//Connor Richards	54689185
//Jonathan Stevens	61356189

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI : MonoBehaviour {
	public GameObject player;
	public GameObject pellet;

	//inputted by user on login
	private string username;
	private string password;
	//changing IP dynamically is not currently working
	private string IP;

	//login checks and stuff
	public bool showGUI;
	private bool gamePlaying;
	private bool loginSuccessful;
	private bool newUser;
	private bool passwordCorrect;

	//this user's name and ID to adjust the WelcomeMessage
	//and possibly enhance/bold this user's player name and score textbox
	private string playerName;
	private int playerID;

	private int highScore;

	//temp info for what needs to be changed
	private string nametoChange;
	private int scoretoChange;
	//ID to decide which textbox to alter
	private int idNametoChange;
	private int idScoretoChange;


	// Use this for initialization
	void Start () {
		showGUI = true;
		username = "Username";
		password = "Password";
		IP = "IP Address";
		playerName = "";
		playerID = 0;
		highScore = 0;
		gamePlaying = false;
		loginSuccessful = false;
		newUser = false;
		passwordCorrect = true;

		nametoChange = "";
		scoretoChange = 0;
		idNametoChange = 0;
		idScoretoChange = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
//		if(currentScore > highScore)
//		{
//			highScore = currentScore;
//		}

		//add a little message at the center of the screen
		//adjust slightly if the user is a new account or not
		if(!gamePlaying && loginSuccessful)
		{
			if(newUser)
			{
				GameObject.Find("WelcomeMessage").guiText.text = "Welcome newcomer " + username + "! You are Player#" + playerID;
			}
			else
			{
				GameObject.Find("WelcomeMessage").guiText.text = "Welcome back, " + username + "! You are Player#" + playerID;
			}
			gamePlaying = true;
		}

		//if the server says the the database says the username exists but the password doesn't match with it
		if(!gamePlaying && !passwordCorrect)
		{
			GameObject.Find("WelcomeMessage").guiText.text = "Incorrect Password. Please try again.";
		}

		//default for highscore text
		//NEED TO UPDATE FOR MILESTONE 5
		GameObject.Find("HighScore").guiText.text = "Your High Score: " + highScore;

		//change name and ID displayed in corresponding player# textbox
		//variables are changed by gamestate updates
		//ONLY PLAYER1 STUF IS CHANGING RIGHT NOW
		if(idNametoChange != 0)
		{
			GameObject.Find("Player" + idNametoChange + "Name").guiText.text = "Player" + idNametoChange + ": " + nametoChange;
			idNametoChange = 0;
		}
		//same stuff but for score
		//NOT IMPLEMENTED YET
		if(idScoretoChange != 0)
		{
			GameObject.Find("Player" + idScoretoChange + "Score").guiText.text = "Score" + idScoretoChange + ": " + scoretoChange;
			idScoretoChange = 0;
		}
	}

	//initial login GUI
	void OnGUI()
	{
		int vertOffset = -30;
		if(showGUI)
		{
			//username and pw text fields
			username = GUI.TextField(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 15 + vertOffset, 200, 20), username);
			password = GUI.PasswordField(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 15 + vertOffset, 200, 20), password, '*');

			//login button creation and features
			if (GUI.Button(new Rect(Screen.width / 2 - 35, Screen.height / 2 + 45 + vertOffset, 70, 20), "Login"))
			{
				//stops the user from using any semicolons(our delimeter)
				//doesn't even send it to the server to verify. stops it client-side
				if (username.Contains(";"))
			    {
					GameObject.Find("WelcomeMessage").guiText.text = "Invalid username: No \";\" allowed";
				}
				else
				{
					//send login and input stuff to nethandler to tell server
					gameObject.GetComponent<NetHandler>().sendLogin(username, password);
				}
			}
		}

		//dynamically change IP connection to server
		//NOT CURRENTLY WORKING
		IP = GUI.TextField(new Rect(Screen.width / 2 - 100, Screen.height - 50, 200, 20), IP);
		if (GUI.Button(new Rect(Screen.width / 2 - 35, Screen.height - 20, 80, 20), "Change IP"))
		{
			gameObject.GetComponent<NetHandler>().setIP(IP);
		}

		//closes app window
		//only works on fully built versions
		if (GUI.Button(new Rect(Screen.width - 40, Screen.height - 20, 40, 20), "Quit"))
		{
			Application.Quit();
		}

		//resets entire scene
		//handy for testing or wanting to reset
		if (GUI.Button(new Rect(0, Screen.height - 20, 60, 20), "Restart"))
		{
			Application.LoadLevel(0);
		}
	}

	//lots of small set methods used to change names and id's and scores
	public void setLoginSuccessful(bool setSuccess)
	{
		loginSuccessful = setSuccess;
	}
	public void setNewUser(bool setNew)
	{
		newUser = setNew;
	}
	public void setPasswordCorrect(bool setCorrect)
	{
		passwordCorrect = setCorrect;
	}
	public void setPlayerID(int pID)
	{
		playerID = pID;
	}
	public void setHighScore(int hscore)
	{
		highScore = hscore;
	}
	public void setPlayerName(string newPlayerName)
	{
		playerName = newPlayerName;
	}

	public void setNametoChange(string ntc)
	{
		nametoChange = ntc;
	}
	public void setScoretoChange(int stc)
	{
		scoretoChange = stc;
	}
	public void setIDNametoChange(int idntc)
	{
		idNametoChange = idntc;
	}
	public void setIDScoretoChange(int idstc)
	{
		idScoretoChange = idstc;
	}

	//the lonely get method
	public int getPlayerID()
	{
		return playerID;
	}
}
