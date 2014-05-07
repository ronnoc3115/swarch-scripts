using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class GameState : MonoBehaviour {

	private NetHandler netHandler;
	private Thread queueThread;
	private Login login;

	// Use this for initialization
	void Start () {
	
		netHandler = gameObject.GetComponent<NetHandler>();
		login = gameObject.GetComponent<Login>();
		queueThread = new Thread(new ThreadStart(readFromQueue));
		queueThread.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
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
					login.setLoginSuccessful(command[1].Equals("True"));
					login.setPasswordCorrect(command[1].Equals("True"));
					login.setPlayerID (int.Parse(command[2]));
					login.setNewUser(command[3].Equals("True"));
					break;
				default:
					break;
				}
			}
			Thread.Sleep(100);
		}
	}
}
