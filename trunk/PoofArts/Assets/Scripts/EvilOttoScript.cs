using UnityEngine;
using System.Collections;

public class EvilOttoScript : MonoBehaviour 
{
	public float timeUntilBoom = 1.0f;

	bool active = false;
	public float moveSpeed = 0.5f;
	
	float timeUnderPlayer = 0.0f;
	
	WorldScript world;
	public GameObject burrowObject;
	
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
			if(transform.position.x > 0.5 && transform.position.x <= world.visibleWidth && transform.position.z > 0.5f && transform.position.z <= world.visibleHeight)
			{
				renderer.enabled = true;
				if(collider)
					collider.enabled = true;
				transform.parent = null;	
				
				gameObject.name = "EvilOtto_Active";
				
				active = true;
			}
		}
		
		//Move
		else
		{
			if(transform.position.x < 0.5 || transform.position.x > world.visibleWidth || transform.position.z < 0.5f || transform.position.z > world.visibleHeight)
			{
				active = false;
				return;
			}
			
			//Seek the player
			GameObject player = GameObject.Find("Player");
			Vector3 playerPos = player.transform.position;
			Vector3 toPlayer = playerPos - transform.position;
			toPlayer.y = 0;
			toPlayer.Normalize();
			
			transform.Translate(toPlayer * moveSpeed * Time.deltaTime, Space.World);
			
			//Am I under the player?
			Vector3 playerPosProj = new Vector3(playerPos.x, 0, playerPos.z);
			Vector3 ottoPosProj = new Vector3(transform.position.x, 0, transform.position.z);
			if(Vector3.Distance(playerPosProj, ottoPosProj) < 0.5f)
			{
				timeUnderPlayer += Time.deltaTime;	
				if(timeUnderPlayer > timeUntilBoom)
				{
					player.SendMessage("HitByOtto");
					
					burrowObject.SetActiveRecursively(false);
					Animation ani = gameObject.GetComponent<Animation>();
					gameObject.renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
					if (ani != null) {
						ani.enabled = true;
					}
					
					
					Destroy(gameObject, 1.2f);
				}
			}
			else
			{
				timeUnderPlayer = 0.0f;
			}
			
			//Set my parent to the tile I'm on
			GameObject tile = world.getTileAtIndex(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
			transform.parent = tile.transform;
		}
	}
	
	void HitByBomb()
	{
		Destroy(gameObject);
		GameObject.Find("Score").GetComponent<ScoreScript>().currentScore += 500;
	}
	
	
}
