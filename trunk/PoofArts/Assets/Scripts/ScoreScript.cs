using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour 
{
	public int currentScore = 0;
	int dispScore = 0;
	public int scoreIncSpeed = 20;
	
	public GameObject pipPrefab;
	public GlobalProfile globalProfile = null;

	// Use this for initialization
	void Start () 
	{
		GameObject globalObj = GameObject.Find("GlobalProfile");
		if(globalObj != null)
		{
			globalProfile = globalObj.GetComponent<GlobalProfile>();
			currentScore = dispScore = globalProfile.score;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(globalProfile != null)
		{
			globalProfile.score = currentScore;
		}
		
		if(dispScore < currentScore)
			dispScore += scoreIncSpeed;
		dispScore = Mathf.Clamp(dispScore, 0, currentScore);
		
		guiText.text = dispScore.ToString()/*.PadLeft(8, '0')*/;
		//Clone the text to all child shadows
		foreach(Transform t in transform)
		{
			if(t.guiText)
			{
				t.guiText.text = guiText.text;
			}
		}
	}
	
	
}
