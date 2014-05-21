//Connor Richards	54689185
//Jonathan Stevens	61356189

using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		switch(gameObject.name)
		{
		case "Wall-Bottom":
			transform.position = new Vector2(transform.position.x, 
			                                 (-transform.parent.renderer.bounds.size.y / 2) + (transform.renderer.bounds.size.y / 2));
			transform.localScale = new Vector2(1.0f, transform.localScale.y);
			break;
		case "Wall-Left":
			transform.position = new Vector2((-transform.parent.renderer.bounds.size.x / 2) + (transform.renderer.bounds.size.x / 2), 
			                                 transform.position.y);
			transform.localScale = new Vector2(transform.localScale.x, 1.0f);
			break;
		case "Wall-Right":
			transform.position = new Vector2((transform.parent.renderer.bounds.size.x / 2) - (transform.renderer.bounds.size.x / 2), 
			                                 transform.position.y);
			transform.localScale = new Vector2(transform.localScale.x, 1.0f);
			break;
		case "Wall-Top":
			transform.position = new Vector2(transform.position.x, 
			                                 (transform.parent.renderer.bounds.size.y / 2) - (transform.renderer.bounds.size.y / 2));
			transform.localScale = new Vector2(1.0f, transform.localScale.y);
			break;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
