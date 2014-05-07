//Connor Richards	54689185
//Jonathan Stevens	61356189

using UnityEngine;
using System.Collections;

public class Pellet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.name = "Pellet";
		spawn();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.name == "Player(Clone)")
		{
			spawn();
			col.gameObject.GetComponent<Player>().grow();
		}
	}

	private void spawn()
	{
		transform.position = randSpawnPoint();
	}

	public static Vector2 randSpawnPoint()
	{
		return new Vector2(Random.Range(-GameObject.Find("Arena").transform.renderer.bounds.size.x/2.25f, 
		                                GameObject.Find("Arena").transform.renderer.bounds.size.x/2.25f),
		                   Random.Range(-GameObject.Find("Arena").transform.renderer.bounds.size.y/2.25f, 
		             GameObject.Find("Arena").transform.renderer.bounds.size.y/3));
	}
}
