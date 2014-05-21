//Connor Richards	54689185
//Jonathan Stevens	61356189

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float startSpeed;
	private float speed;
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
		ID = 0;
		//respawn();

		arenaBoundsX = (GameObject.Find("Arena").transform.renderer.bounds.size.x);
		arenaBoundsY = (GameObject.Find("Arena").transform.renderer.bounds.size.y);

		GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -1.0f * speed);

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

	//change position on client
	//called by gamestate on updates
	public void respawn(float respawnX, float respawnY)
	{
		//fancy math to adjust proportions of position properly
		float newRespawnX = ((arenaBoundsX*respawnX/120) - (arenaBoundsX/2));
		float newRespawnY = ((arenaBoundsY*respawnY/100) - (arenaBoundsY/2));
		transform.position = new Vector2 (newRespawnX, newRespawnY);

		Debug.Log("Moving to " + newRespawnX + " " + newRespawnY);
		//I think this is unnecessary, but leaving it commented out until we know for sure it can be deleted
		//transform.localScale = new Vector2(6.0f, 6.0f);
		//speed = startSpeed;
	}

	//set ID of this specific player cube
	//helper to adjust color if this ID matches with userID given by server
	public void setID(int newID)
	{
		ID = newID;
	}
}
