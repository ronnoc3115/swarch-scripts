//Connor Richards	54689185
//Jonathan Stevens	61356189

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float startSpeed;
	private float speed;

	// Use this for initialization
	void Start () {
		startSpeed = 3.0f;
		respawn();
	}
	
	// Update is called once per frame
	void Update () {
		if(rigidbody2D.velocity.magnitude < 0.1f)
			respawn();

		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 1.0f * speed);
		}
		else if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -1.0f * speed);
		}
		else if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(-1.0f * speed, 0.0f);
		}
		else if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(1.0f * speed, 0.0f);
		}

	}

	public void grow()
	{
		transform.localScale *= 1.1f;
		speed *= 0.9f;
	}

	public void respawn()
	{
		transform.position = randSpawnPoint();
		transform.localScale = new Vector2(6.0f, 6.0f);
		speed = startSpeed;
		GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -1.0f * speed);
	}

	private Vector2 randSpawnPoint()
	{
		return new Vector2(Random.Range(-GameObject.Find("Arena").transform.renderer.bounds.size.x/3, 
		                                              GameObject.Find("Arena").transform.renderer.bounds.size.x/3),
		                                 Random.Range(-GameObject.Find("Arena").transform.renderer.bounds.size.y/3, 
		             GameObject.Find("Arena").transform.renderer.bounds.size.y/3));
	}
}
