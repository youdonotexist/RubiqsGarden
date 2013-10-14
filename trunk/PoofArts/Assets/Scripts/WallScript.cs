using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void HitByBomb()
	{
		GameObject.Find("World").GetComponent<WorldScript>().ChangeTo(Mathf.RoundToInt(transform.position.x),Mathf.RoundToInt(transform.position.z),0);		
		GameObject.Find("Score").GetComponent<ScoreScript>().currentScore += 50;
	}
}
