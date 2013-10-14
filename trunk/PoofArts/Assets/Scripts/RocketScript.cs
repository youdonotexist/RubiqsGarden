using UnityEngine;
using System.Collections;

public class RocketScript : MonoBehaviour 
{
	public bool active = false;
	public float moveSpeed = 5.0f;
	Vector3 moveDir = new Vector3(0,0,1);
	Vector3 destination;
	
	WorldScript world;

	// Use this for initialization
	void Start () 
	{
		world = GameObject.Find("World").GetComponent<WorldScript>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Activate when on-screen
		if(active == false)
		{
			if(transform.position.x > 0.5 && transform.position.x <= world.visibleWidth + 1 && transform.position.z > 0.5f && transform.position.z <= world.visibleHeight + 1)
			{
				renderer.enabled = true;
				if(collider)
					collider.enabled = true;
				transform.parent = null;	
				
				destination = new Vector3(Mathf.RoundToInt(transform.position.x), transform.position.y, Mathf.RoundToInt(transform.position.z));
				gameObject.name = "Rocket_Active";
				transform.parent = GameObject.Find("RocketParent").transform;
				
				active = true;
			}
		}
		
		//Move
		else
		{
			if(TryMoveForward() == false) 
			{
				moveDir = Quaternion.Euler(0,90,0) * moveDir;	
				if(TryMoveForward() == false) 
				{
					moveDir = Quaternion.Euler(0,180,0) * moveDir;
					if(TryMoveForward() == false) 
					{
						moveDir = Quaternion.Euler(0,-90,0) * moveDir;
						if(TryMoveForward() == false) 
						{
							moveDir = Quaternion.Euler(0,-90,0) * moveDir;
						}
					}
				}
			}
		}
	}
	
	bool TryMoveForward()
	{
		transform.forward = moveDir;
		transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
		if(transform.position == destination)
		{
			int tileX = Mathf.RoundToInt(transform.position.x + moveDir.x);
			int tileY = Mathf.RoundToInt(transform.position.z + moveDir.z);
			if(tileX > 0 && tileX < world.visibleWidth + 1 && tileY > 0 && tileY < world.visibleHeight + 1 && world.worldTiles[tileX, tileY].name.StartsWith("Floor"))
			{
				destination += moveDir;
			}
			else
			{
				return false;	
			}
		}
		
		return true;
	}
	
	void HitByLaser()
	{
		GameObject.Find("SFXPlayer").SendMessage("PlayClip", "FlyDie");
		Destroy(gameObject);	
		GameObject.Find("Score").GetComponent<ScoreScript>().currentScore += 250;
	}
	
	
	void HitByBomb()
	{
		GameObject.Find("SFXPlayer").SendMessage("PlayClip", "FlyDie");
		Destroy(gameObject);	
		GameObject.Find("Score").GetComponent<ScoreScript>().currentScore += 250;
	}
	
	
	void OnCollisionEnter(Collision collision)
	{
		collision.gameObject.SendMessage("HitByRocket", SendMessageOptions.DontRequireReceiver);
		Destroy(gameObject);
	}
}
