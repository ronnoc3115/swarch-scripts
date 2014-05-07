//Connor Richards	54689185
//Jonathan Stevens	61356189 - Hello git!

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Login : MonoBehaviour {
	public GameObject player;
	public GameObject pellet;

	private string username;
	private string password;
	private bool showGUI;
	private bool gamePlaying;
	private bool loginSuccessful;
	private bool newUser;
	private bool passwordCorrect;
	private int playerID;

	// Use this for initialization
	void Start () {
		showGUI = true;
		username = "Username";
		password = "Password";
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
	
		if(!gamePlaying && loginSuccessful)
		{
			showGUI = false;
			Instantiate(player);
			for(int i = 0; i < 4; i++)
			{
				Instantiate(pellet);
			}
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
	}
	
	void OnGUI()
	{
		if(showGUI)
		{
			int vertOffset = -30;
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
	public void setPlayerID(int ID)
	{
		playerID = ID;
	}
}
