﻿//Connor Richards	54689185
//Jonathan Stevens	61356189

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Security.Cryptography;

public class NetHandler : MonoBehaviour {

	//private Socket socket;
	private Thread thread;
	private TcpClient client;
	private string IP;
	private int port;
	private NetworkStream netStream;
	public Queue<string[]> readQueue;

	private string lastInput;

	// Use this for initialization
	void Start () {
	
		IP = "169.234.35.4";
		port = 8888;
		//socket = new Socket();
		thread = new Thread(new ThreadStart(updateClient));
		client = new TcpClient(IP, port);
		readQueue = new Queue<string[]>();
		netStream = client.GetStream();
		thread.Start();

		sendString("I am here!");
	}

	private void sendString(string message)
	{
		byte[] byteMessage = Encoding.UTF8.GetBytes(message + ";");
		
		netStream.Write(byteMessage, 0, byteMessage.Length);
		netStream.Flush();
	}

	private string encodePassword(string originalName, string originalPass)
	{
		MD5 hasher = MD5.Create();
		byte[] bytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(originalName + originalPass));

		// Create a new Stringbuilder to collect the bytes 
		// and create a string.
		StringBuilder sBuilder = new StringBuilder();
		
		// Loop through each byte of the hashed data  
		// and format each one as a hexadecimal string. 
		for (int i = 0; i < bytes.Length; i++)
		{
			sBuilder.Append(bytes[i].ToString("x2"));
		}
		
		// Return the hexadecimal string. 
		return sBuilder.ToString();
	}

	public void sendLogin(string name, string pass)
	{
		string encodedPass = encodePassword(name, pass);
		sendString("logincommand;" + name + ";" + encodedPass);
	}

	public void sendInput(string input)
	{
		if(input!=lastInput)
		{
			lastInput = input;
			sendString("inputcommand;" + input);
		}
	}

	public void sendScore(string name, string pass, int score)
	{

	}

	private void updateClient()
	{
		while(true)
		{
			if(netStream.DataAvailable)
			{
				byte[] bytes = new byte[client.ReceiveBufferSize];
				int cnt = netStream.Read(bytes, 0, bytes.Length);
				StringBuilder sb = new StringBuilder();
				sb.Append(Encoding.UTF8.GetString(bytes, 0, cnt));
				string str = sb.ToString ();
				string[] stringArray = str.Split(new char[] { ';' });

				//so we can read exactly what the client is reading from the server
				foreach(string s in stringArray)
				{
					Debug.Log(s);
				}

				readQueue.Enqueue(stringArray);
			}
			Thread.Sleep(20);
		}
	}

	void OnApplicationQuit()
	{
		disconnect();
	}

	private void disconnect()
	{
		netStream.Close();
		client.Close();
	}

	//trying to dynamically change IP used for server
	//NOT CURRENTLY WORKING
	public void setIP(string IPadd)
	{
		thread.Abort();
		IP = IPadd;
		client = new TcpClient(IP, port);
		netStream = client.GetStream();
		thread.Start();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
