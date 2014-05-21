//Connor Richards	54689185
//Jonathan Stevens	61356189

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float startSpeed;
	private float speed;
	private bool trailRendered = true;
	private int ID;
	public string input;

	// Use this for initialization
	void Start () {
		input = "";
		//gameObject.name = "Player" + ID;
		startSpeed = 3.0f;
		ID = 0;
		//respawn();
	}
	
	// Update is called once per frame	
	void Update () {
//		if(rigidbody2D.velocity.magnitude < 0.1f)
//			respawn();

		//gameObject.name = "Player" + ID;

		if (GameObject.Find("GameLogic").GetComponent<UI>().getPlayerID() != ID)
		{
			gameObject.GetComponent<SpriteRenderer>().color = Color.red;
		}

		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			input = "up";
			//gameObject.GetComponent<NetHandler>().sendInput(input);
			GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 1.0f * speed);
		}
		else if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			input = "down";
			//gameObject.GetComponent<NetHandler>().sendInput(input);
			GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -1.0f * speed);
		}
		else if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			input = "left";
			//gameObject.GetComponent<NetHandler>().sendInput(input);
			GetComponent<Rigidbody2D>().velocity = new Vector2(-1.0f * speed, 0.0f);
		}
		else if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			input = "right";
			//gameObject.GetComponent<NetHandler>().sendInput(input);
			GetComponent<Rigidbody2D>().velocity = new Vector2(1.0f * speed, 0.0f);
		}

		if (trailRendered == false)
		{
			GetComponent<TrailRenderer>().enabled = false;
		}
	}

	public void changeSize(float newSize)
	{
		transform.localScale = new Vector3(newSize, newSize);
	}

	public void respawn(float respawnX, float respawnY)
	{
		float newRespawnX = (((GameObject.Find("Arena").transform.renderer.bounds.size.x)*respawnX/120) - ((GameObject.Find("Arena").transform.renderer.bounds.size.x)/2));
		float newRespawnY = (((GameObject.Find("Arena").transform.renderer.bounds.size.y)*respawnY/100) - ((GameObject.Find("Arena").transform.renderer.bounds.size.y)/2));
		transform.position = new Vector2 (newRespawnX, newRespawnY);
		transform.localScale = new Vector2(6.0f, 6.0f);
		speed = startSpeed;
		GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -1.0f * speed);
		//StartCoroutine("spawnTrail");
	}

	public void setID(int newID)
	{
		ID = newID;
	}

//	private Vector2 randSpawnPoint()
//	{
//		return new Vector2(Random.Range(-GameObject.Find("Arena").transform.renderer.bounds.size.x/3, 
//		                                              GameObject.Find("Arena").transform.renderer.bounds.size.x/3),
//		                                 Random.Range(-GameObject.Find("Arena").transform.renderer.bounds.size.y/3, 
//		             GameObject.Find("Arena").transform.renderer.bounds.size.y/3));
//	}

	/*
	private IEnumerator spawnTrail()
	{
		trailRendered = false;
		yield return new WaitForSeconds(0.5f);
		trailRendered = true;
		GetComponent<TrailRenderer>().enabled = true;
	}*/
}
