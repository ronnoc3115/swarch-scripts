//Connor Richards	54689185
//Jonathan Stevens	61356189 - Hello git!

using UnityEngine;
using System.Collections;

public class Login : MonoBehaviour {
	public GameObject player;
	public GameObject pellet;

	private string username;
	private string password;
	private bool showGUI;


	// Use this for initialization
	void Start () {
		showGUI = true;
		username = "Username";
		password = "Password";
		//while(true)
		//{

		//}
	}
	
	// Update is called once per frame
	void Update () {
	
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
				Debug.Log(username + "  " + password);
				GameObject.Find("Name").guiText.text = username;
				showGUI = false;
				Instantiate(player);
				for(int i = 0; i < 4; i++)
					Instantiate(pellet);
			}
		}
	}
}
