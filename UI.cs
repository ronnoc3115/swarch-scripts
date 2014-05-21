//Connor Richards	54689185
//Jonathan Stevens	61356189 - Hello git!

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI : MonoBehaviour {
	public GameObject player;
	public GameObject pellet;

	private string username;
	private string password;
	private string opponentName;
	private string IP;
	public bool showGUI;
	private bool gamePlaying;
	private bool loginSuccessful;
	private bool newUser;
	private bool passwordCorrect;
	private int playerID;
	private int opponentID;
	private int currentScore;
	private int highScore;
	private int opponentScore;

	// Use this for initialization
	void Start () {
		showGUI = true;
		username = "Username";
		password = "Password";
		opponentName = "";
		IP = "IP Address";
		playerID = 0;
		opponentID = 0;
		currentScore = 0;
		highScore = 0;
		opponentScore = 0;
		gamePlaying = false;
		loginSuccessful = false;
		newUser = false;
		passwordCorrect = true;
		
		//while(true)
		//{

		//}
	}
	
	// Update is called once per frame
	void Update () {
	
		if(currentScore > highScore)
		{
			highScore = currentScore;
		}

		if(!gamePlaying && loginSuccessful)
		{
			if(newUser)
			{
				GameObject.Find("Name").guiText.text = "Player " + playerID + ": " + username + ", Welcome new user!";
			}
			else
			{
				GameObject.Find("Name").guiText.text = "Player " + playerID + ": " + username;
			}
			gamePlaying = true;
		}

		if(!gamePlaying && !passwordCorrect)
		{
			GameObject.Find("Name").guiText.text = "Incorrect Password. Please try again.";
		}

		if(loginSuccessful)
		{
			GameObject.Find("OpponentName").guiText.text = "Player " + opponentID + ": " + opponentName;
		}

		GameObject.Find("CurrentScores").guiText.text = "Current Score: " + currentScore + "     Opponent Score: " + opponentScore;
		GameObject.Find("HighScore").guiText.text = "High Score: " + highScore;
	}
	
	void OnGUI()
	{
		int vertOffset = -30;
		if(showGUI)
		{
			username = GUI.TextField(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 15 + vertOffset, 200, 20), username);
			password = GUI.PasswordField(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 15 + vertOffset, 200, 20), password, '*');

			if (GUI.Button(new Rect(Screen.width / 2 - 35, Screen.height / 2 + 45 + vertOffset, 70, 20), "Login"))
			{
				if (username.Contains(";"))
			    {
					GameObject.Find("Name").guiText.text = "Invalid username: No \";\" allowed";
				}
				else
				{
					gameObject.GetComponent<NetHandler>().sendLogin(username, password);
				}
			}
		}

		IP = GUI.TextField(new Rect(Screen.width / 2 - 100, Screen.height - 50, 200, 20), IP);
		if (GUI.Button(new Rect(Screen.width / 2 - 35, Screen.height - 20, 80, 20), "Change IP"))
		{
			gameObject.GetComponent<NetHandler>().setIP(IP);
		}

		if (GUI.Button(new Rect(Screen.width - 40, Screen.height - 20, 40, 20), "Quit"))
		{
			Application.Quit();
		}

		if (GUI.Button(new Rect(0, Screen.height - 20, 60, 20), "Restart"))
		{
			Application.LoadLevel(0);
		}
	}

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
	public void setOpponentName(string opponent)
	{
		opponentName = opponent;
	}
	public void setPlayerID(int pID)
	{
		playerID = pID;
	}
	public void setOpponentID(int oID)
	{
		opponentID = oID;
	}
	public void setCurrentScore(int cscore)
	{
		currentScore = cscore;
	}
	public void setHighScore(int hscore)
	{
		highScore = hscore;
	}
	public void setOpponentScore(int oscore)
	{
		opponentScore = oscore;
	}

	public int getPlayerID()
	{
		return playerID;
	}
}
