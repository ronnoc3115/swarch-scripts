//Connor Richards	54689185
//Jonathan Stevens	61356189

using UnityEngine;
using System.Collections;

public class Pellet : MonoBehaviour {

	//private int pelletID;

	// Use this for initialization
	void Start () {
		//pelletID = 0;
		//gameObject.name = "Pellet" + pelletID;
		//spawn();
	}
	
	// Update is called once per frame
	void Update () {
	

	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.name == "Player(Clone)")
		{
			//spawn();
			//col.gameObject.GetComponent<Player>().grow();
		}
	}

	public void spawn(float spawnX, float spawnY)
	{
		//transform.position = randSpawnPoint();
		float newSpawnX = (((GameObject.Find("Arena").transform.renderer.bounds.size.x)*spawnX/120) - ((GameObject.Find("Arena").transform.renderer.bounds.size.x)/2));
		float newSpawnY = (((GameObject.Find("Arena").transform.renderer.bounds.size.y)*spawnY/100) - ((GameObject.Find("Arena").transform.renderer.bounds.size.y)/2));
		transform.position = new Vector2 (newSpawnX, newSpawnY);
	}

//	public void setPelletID(int newID)
//	{
//		pelletID = newID;
//	}

//	public static Vector2 randSpawnPoint()
//	{
//		return new Vector2(Random.Range(-GameObject.Find("Arena").transform.renderer.bounds.size.x/2.25f, 
//		                                GameObject.Find("Arena").transform.renderer.bounds.size.x/2.25f),
//		                   Random.Range(-GameObject.Find("Arena").transform.renderer.bounds.size.y/2.25f, 
//		             GameObject.Find("Arena").transform.renderer.bounds.size.y/2.25f));
//	}
}
