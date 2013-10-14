using UnityEngine;
using System.Collections;

public class TreasureScript : MonoBehaviour 
{
	public int pointsWorth = 100;
	public GameObject coinPrefab;	
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.name == "Player")
		{
			other.GetComponent<PlayerControl>().treasure += 1;
			
			for(int i = 0; i < 10; ++i)
			{
				Instantiate(coinPrefab, transform.position + new Vector3(0,5,0) + Random.onUnitSphere * 0.5f, Quaternion.identity);				
			}
			
			GameObject.Find("SFXPlayer").SendMessage("PlayClip", "TreasureCollected");
			
			ScoreScript scoreScript = GameObject.Find("Score").GetComponent<ScoreScript>();
			scoreScript.currentScore += pointsWorth;
			Instantiate(scoreScript.pipPrefab, Camera.main.WorldToViewportPoint(transform.position), Quaternion.identity);
			
			Destroy(gameObject);	
		}
	}
}
