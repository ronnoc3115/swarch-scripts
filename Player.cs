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

	// Use this for initialization
	void Start () {
		//colorSet = false;
		input = "";
		//gameObject.name = "Player" + ID;
		startSpeed = 3.0f;
		ID = 0;
		//respawn();
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
			//gameObject.GetComponent<NetHandler>().sendInput(input);
		}
		else if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			input = "down";
			//gameObject.GetComponent<NetHandler>().sendInput(input);
		}
		else if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			input = "left";
			//gameObject.GetComponent<NetHandler>().sendInput(input);
		}
		else if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			input = "right";
			//gameObject.GetComponent<NetHandler>().sendInput(input);
		}

		//trying to cut-off trailrenderer on respawn
		//DOESN'T WORK
//		if (trailRendered == false)
//		{
//			GetComponent<TrailRenderer>().enabled = false;
//		}
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
		float newRespawnX = (((GameObject.Find("Arena").transform.renderer.bounds.size.x)*respawnX/120) - ((GameObject.Find("Arena").transform.renderer.bounds.size.x)/2));
		float newRespawnY = (((GameObject.Find("Arena").transform.renderer.bounds.size.y)*respawnY/100) - ((GameObject.Find("Arena").transform.renderer.bounds.size.y)/2));
		transform.position = new Vector2 (newRespawnX, newRespawnY);

		//I think this is unnecessary, but leaving it commented out until we know for sure it can bee deleted
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
