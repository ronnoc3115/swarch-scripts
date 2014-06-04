//Connor Richards	54689185
//Jonathan Stevens	61356189

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	private float startSpeed;
	private float speed;
	private float startSize;
	private float size;
	//the level that adjusts the speed/size
	public float level;
	//private bool trailRendered = true;
	private int ID;
	//private bool colorSet;
	public string input;

	private float arenaBoundsX;
	private float arenaBoundsY;

	private GameObject gameLogic;

	// Use this for initialization
	void Start () {
		//colorSet = false;
		input = "";
		//gameObject.name = "Player" + ID;
		startSpeed = 3.0f;
		speed = startSpeed;
		startSize = 1.0f;
		size = startSize;

		level = 1.0f;

		ID = 0;
		//respawn();

		GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -1.0f * speed);

		arenaBoundsX = (GameObject.Find("Arena").transform.renderer.bounds.size.x);
		arenaBoundsY = (GameObject.Find("Arena").transform.renderer.bounds.size.y);

		gameLogic = GameObject.Find("Game Logic");
	}
	
	// Update is called once per frame	
	void Update () {

		//Trying to set color to blue if it is user's cube
		//NOT CURRENTLY WORKING
		if (GameObject.Find("Game Logic").GetComponent<UI>().getPlayerID() != ID)
		{
			gameObject.GetComponent<SpriteRenderer>().color = Color.red;
		}
		else
		{
			gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
		}
		//set speed/size based on level, which is gained from eating
		speed = startSpeed*(Mathf.Pow(0.9f, (level-1.0f)));
		size = startSize*(Mathf.Pow(1.1f, (level-1.0f)));
		transform.localScale = new Vector3(size, size) *6;

		//input commands
		//NEED TO SENDINPUT TO NETHANDLER TO TELL SERVER
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			input = "up";
			gameLogic.GetComponent<NetHandler>().sendInput(input);
		}
		else if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			input = "down";
			gameLogic.GetComponent<NetHandler>().sendInput(input);
		}
		else if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			input = "left";
			gameLogic.GetComponent<NetHandler>().sendInput(input);
		}
		else if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			input = "right";
			gameLogic.GetComponent<NetHandler>().sendInput(input);
		}

		//trying to cut-off trailrenderer on respawn
		//DOESN'T WORK
//		if (trailRendered == false)
//		{
//			GetComponent<TrailRenderer>().enabled = false;
//		}
	}

	//turning direction methods
	public void up()
	{
		GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 1.0f * speed);
	}
	public void down()
	{
		GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -1.0f * speed);
	}
	public void left()
	{
		GetComponent<Rigidbody2D>().velocity = new Vector2(-1.0f * speed, 0.0f);
	}
	public void right()
	{
		GetComponent<Rigidbody2D>().velocity = new Vector2(1.0f * speed, 0.0f);
	}

	//change size aesthetically on client
	//called by gamestate on updates
	public void changeSize(float newSize)
	{
		transform.localScale = new Vector3(newSize, newSize) *6;
	}

	//change position on client and reset stuff to normal
	//called by gamestate on updates
	public void respawn(float respawnX, float respawnY)
	{
		//make sure it's visible
		gameObject.GetComponent<SpriteRenderer>().enabled = true;
		//set the level back to 1 to make sure speed/size are normal
		level = 1;
		//fancy math to adjust proportions of position properly
		float newRespawnX = ((arenaBoundsX*respawnX/120) - (arenaBoundsX/2));
		float newRespawnY = ((arenaBoundsY*respawnY/100) - (arenaBoundsY/2));
		transform.position = new Vector2 (newRespawnX, newRespawnY);

		//Debug.Log("Moving to " + newRespawnX + " " + newRespawnY);
		//I think this is unnecessary, but leaving it commented out until we know for sure it can be deleted
		//transform.localScale = new Vector2(6.0f, 6.0f);
		//speed = startSpeed;
	}

	public void setPosition(float posX, float posY)
	{
		//fancy math to adjust proportions of position properly
		float newPosX = ((arenaBoundsX*posX/120) - (arenaBoundsX/2));
		float newPosY = ((arenaBoundsY*posY/100) - (arenaBoundsY/2));
		transform.position = new Vector2 (newPosX, newPosY);
	}

	//set ID of this specific player cube
	//helper to adjust color if this ID matches with userID given by server
	public void setID(int newID)
	{
		ID = newID;
	}
}
