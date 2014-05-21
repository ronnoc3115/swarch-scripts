//Connor Richards	54689185
//Jonathan Stevens	61356189

using UnityEngine;
using System.Collections;

public class Pellet : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//change position on client
	//called by gamestate on updates
	public void spawn(float spawnX, float spawnY)
	{
		//fancy math to adjust proportions of position properly
		float newSpawnX = (((GameObject.Find("Arena").transform.renderer.bounds.size.x)*spawnX/120) - ((GameObject.Find("Arena").transform.renderer.bounds.size.x)/2));
		float newSpawnY = (((GameObject.Find("Arena").transform.renderer.bounds.size.y)*spawnY/100) - ((GameObject.Find("Arena").transform.renderer.bounds.size.y)/2));
		transform.position = new Vector2 (newSpawnX, newSpawnY);
	}
}
