using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour 
{
	public float timer = 10.0f;
	
	public Texture boomTex;
	
	float interp = 0.0f;
	bool interpUp = true;
	
	bool done = false;
	
	WorldScript world;
	
	// Use this for initialization
	void Start () 
	{
		world = GameObject.Find("World").GetComponent<WorldScript>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(done) 
			return;
		
		//Only update if it's on-screen
		if(transform.position.x > 0.5 && transform.position.x <= world.visibleWidth + 1 && transform.position.z > 0.5f && transform.position.z <= world.visibleHeight + 1)
		{
			//Update timer
			timer -= Time.deltaTime;
			
			//Update visuals
			float interpAmt = 0;
			if(timer < 6.0f)
				interpAmt = Time.deltaTime;
			if(timer < 3.0f)
				interpAmt = Time.deltaTime * (8.0f - timer + 2.0f);
			
			if(interpUp)
			{
				interp += interpAmt;
				if(interp >= 1.0f)
				{
					interp = 1.0f;
					interpUp = false;
				}
			}
			else
			{
				interp -= interpAmt;
				if(interp <= 0.0f)
				{
					interp = 0.0f;
					interpUp = true;
				}				
			}			
			float scale = 0.8f + 0.4f * interp;
			transform.localScale = new Vector3(scale, transform.localScale.y, scale);
			renderer.material.color = Color.Lerp(Color.white, Color.red, interp);			
			
			//EXPLODE!
			if(timer <= 0.0f)
			{
				GameObject.Find("SFXPlayer").SendMessage("PlayClip", "Explosion");
				transform.localScale=  new Vector3(3.0f, 3.0f, 3.0f);
				renderer.material.mainTexture = boomTex;
				Destroy(gameObject, 1.0f);
				renderer.material.mainTextureScale = new Vector2(-1,-1);
				
				done = true;
				
				//Pass the explosion on
				int centerX = Mathf.RoundToInt(transform.position.x);
				int centerY = Mathf.RoundToInt(transform.position.z);
				for(int x = centerX - 1; x  <= centerX + 1; ++x)
				{
					for(int y = centerY - 1; y <= centerY + 1; ++y)
					{
						world.worldTiles[x,y].BroadcastMessage("HitByBomb", SendMessageOptions.DontRequireReceiver);
					}
				}
				
				Transform rocketParent = GameObject.Find("RocketParent").transform;
				foreach(Transform rocket in rocketParent)
				{
					if(Vector3.Distance(rocket.position, transform.position) < 2.5f)
						rocket.SendMessage("HitByBomb", SendMessageOptions.DontRequireReceiver);
				}
				
				Transform player = GameObject.Find("Player").transform;
				if(Vector3.Distance(player.position, transform.position) < 1.5f)
					player.SendMessage("HitByBomb", SendMessageOptions.DontRequireReceiver);
			}
		}
		else
		{
			interp = 0;	
		}
	}
}
